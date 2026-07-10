using Microsoft.Extensions.Configuration;
using Sap.Data.Hana;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Vistony.Worker.Application.WMS.Interfaces;
using Vistony.Worker.Domain.Warelogic;

namespace Vistony.Worker.Infrastructure.Hana
{
    public class WMSRepository : IWMSRepository
    {
        private readonly string _connectionString;
        private readonly string _connectionStringInduvis;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public WMSRepository(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("HanaConnectionSEIDOR_PERU");
            _connectionStringInduvis = configuration.GetConnectionString("HanaConnectionSEIDOR");
            _httpClient = httpClientFactory.CreateClient();
        }

        // ---------- Helpers ----------

        private string GetConnectionStringByDatabase(string database)
            => database == "B1H_VIST_PE" ? _connectionString : _connectionStringInduvis;

        private static string GetCountryCode(string database)
            => database.Split('_').Last(); // "B1H_VIST_PY" -> "PY"

        private string GetApiKey(string countryCode)
            => _configuration[$"WarelogicApi:ApiKeys:{countryCode}"];

        private async Task<List<Dictionary<string, object>>> EjecutarSpConReader(string database, string spName)
        {
            var resultList = new List<Dictionary<string, object>>();
            var connectionString = GetConnectionStringByDatabase(database);

            await using var connection = new HanaConnection(connectionString);
            await connection.OpenAsync();

            string query = $"CALL \"{database}\".\"{spName}\"()";
            await using var command = new HanaCommand(query, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var row = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                    row[reader.GetName(i)] = reader.GetValue(i);
                resultList.Add(row);
            }

            return resultList;
        }

        private async Task PostToWarelogic<T>(string urlConfigKey, T payload, string database)
        {
            var countryCode = GetCountryCode(database);
            var apiKey = GetApiKey(countryCode);

            if (string.IsNullOrEmpty(apiKey))
                throw new InvalidOperationException($"No se encontró ApiKey configurada para el país '{countryCode}' (database: {database}).");

            string url = _configuration[$"WarelogicApi:{urlConfigKey}"];
            string idempotencyKey = DateTime.Now.ToString("yyyyMMddHHmmss");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("X-API-Key", apiKey);
            _httpClient.DefaultRequestHeaders.Add("Idempotency-Key", idempotencyKey);
            _httpClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException(
                    $"Error en POST a Warelogic ({urlConfigKey}, {database}) [{(int)response.StatusCode}]: {responseBody}");
            }
        }

        // ---------- UnitsOfMeasure ----------

        public async Task EjecutarUnitsOfMeasure(string database)
        {
            var rawList = await EjecutarSpConReader(database, "P_VIS_GET_UOMS");
            if (rawList.Count == 0) return;

            var correlationId = $"BULK-UOM-{DateTime.Now:yyyyMMddHHmmss}";

            var payload = new UomPayload
            {
                correlation_id = correlationId,
                units = rawList
                    .DistinctBy(row => row["code"].ToString())
                    .Select(row => new Uom
                    {
                        external_id = $"SAP-UOM-{row["code"]}",
                        code = row["code"]?.ToString(),
                        name = row["name"]?.ToString(),
                        symbol = row["symbol"]?.ToString(),
                        decimal_precision = Convert.ToInt32(row["decimal_precision"]),
                        is_active = Convert.ToInt32(row["is_active"]) == 1
                    }).ToList()
            };

            await PostToWarelogic("UrlUom", payload, database);
        }

        // ---------- Items ----------

        public async Task EjecutarItems(string database)
        {
            var rawList = await EjecutarSpConReader(database, "P_VIS_GET_ITEMS");
            if (rawList.Count == 0) return;

            var correlationId = $"BULK-ITEMS-{DateTime.Now:yyyyMMddHHmmss}";

            var payload = new ItemPayload
            {
                correlation_id = correlationId,
                items = rawList
                    .GroupBy(row => row["ItemCode"].ToString())
                    .Select(g =>
                    {
                        var first = g.First();
                        return new Item
                        {
                            external_id = $"SAP-{first["ItemCode"]}",
                            correlation_id = correlationId,
                            item_code = first["ItemCode"]?.ToString(),
                            item_name = first["ItemName"]?.ToString(),
                            description = first["description"]?.ToString(),
                            base_unit_code = first["base_unit_code"]?.ToString(),
                            family_code = first["family_code"]?.ToString(),
                            primary_barcode = first["primary_barcode"]?.ToString(),
                            lot_control = Convert.ToInt32(first["lot_control"]) == 1,
                            serial_control = Convert.ToInt32(first["serial_control"]) == 1,
                            expiration_control = Convert.ToInt32(first["expiration_control"]) == 1,
                            shelf_life_days = Convert.ToInt32(Convert.ToDecimal(first["shelf_life_days"])),
                            is_active = Convert.ToInt32(first["is_active"]) == 1,
                            units = new List<ItemUnit>()
                        };
                    }).ToList()
            };

            await PostToWarelogic("UrlItems", payload, database);
        }

        // ---------- OutboundOrders ----------

        public async Task EjecutarOutbundOrders(string database)
        {
            var rawList = await EjecutarSpConReader(database, "P_VIS_GET_OUTBOUNDORDERS");
            if (rawList.Count == 0) return;

            var correlationId = $"BULK-OUTBOUND-{DateTime.Now:yyyyMMddHHmmss}";

            var payload = new NoutboundPayload
            {
                correlation_id = correlationId,
                outbound_orders = rawList
                    .GroupBy(row => row["DocEntry"].ToString())
                    .Select(g =>
                    {
                        var header = g.First();
                        return new OutboundOrder
                        {
                            external_id = header["DocEntry"].ToString(),
                            correlation_id = correlationId,
                            order_number = header["DocNum"].ToString(),
                            external_reference = header["NumAtCard"]?.ToString(),
                            order_type = MapOrderType(header["order_type"]?.ToString()),
                            branch_code = header["branch_code"]?.ToString(),
                            warehouse_code = header["warehouse_code"]?.ToString(),
                            priority = 5,
                            customer_code = header["CardCode"]?.ToString(),
                            customer_name = header["CardName"]?.ToString(),
                            ship_to_code = header["ship_to_code"]?.ToString(),
                            ship_to_name = header["ship_to_name"]?.ToString(),
                            ship_to_address = header["ship_to_address"]?.ToString(),
                            ship_to_city = header["ship_to_city"]?.ToString(),
                            ship_to_region = header["ship_to_region"]?.ToString(),
                            ship_to_country_code = header["ship_to_country_code"]?.ToString(),
                            source_document_type = "SALES_ORDER",
                            source_document_number = header["DocNum"].ToString(),
                            required_at = Convert.ToDateTime(header["required_at"]),
                            notes = header["Comments"]?.ToString(),
                            lines = g.Select(line => new OutboundLine
                            {
                                external_line_id = line["LineNum"]?.ToString(),
                                line_number = Convert.ToInt32(line["LineNum"]),
                                item_code = line["ItemCode"]?.ToString(),
                                unit_code = line["UomCode"]?.ToString(),
                                requested_quantity = Convert.ToDecimal(line["Quantity"]),
                                requested_lot_number = line["lot_number"]?.ToString()
                            }).ToList()
                        };
                    }).ToList()
            };

            await PostToWarelogic("UrlOutbound", payload, database);
        }

        private static string MapOrderType(string sapOrderType) => sapOrderType switch
        {
            "DELIVERY" => "SALES_ORDER",
            "TRANSFER_REQUEST" => "TRANSFER",
            _ => "OTHER"
        };

        // ---------- Receipts ----------

        public async Task EjecutarReceipts(string database)
        {
            var rawList = await EjecutarSpConReader(database, "P_VIS_GET_RECEIPTS");
            if (rawList.Count == 0) return;

            var correlationId = $"BULK-RECEIPTS-{DateTime.Now:yyyyMMddHHmmss}";

            var payload = new ReceiptPayload
            {
                correlation_id = correlationId,
                receipts = rawList
                    .GroupBy(row => row["DocEntry"])
                    .Select(g =>
                    {
                        var first = g.First();
                        return new Receipt
                        {
                            external_id = $"SAP-REC-{first["DocEntry"]}",
                            correlation_id = correlationId,
                            receipt_number = $"REC-{first["DocNum"]}",
                            external_reference = first["external_reference"]?.ToString(),
                            branch_code = first["branch_code"]?.ToString(),
                            warehouse_code = first["warehouse_code"]?.ToString(),
                            supplier_code = first["supplier_code"]?.ToString(),
                            supplier_name = first["supplier_name"]?.ToString(),
                            source_document_number = first["DocNum"].ToString(),
                            scheduled_at = Convert.ToDateTime(first["scheduled_at"]),
                            notes = first["notes"]?.ToString(),
                            lines = g.Select(row => new ReceiptLine
                            {
                                external_line_id = (Convert.ToInt32(row["LineNum"]) + 1).ToString(),
                                line_number = Convert.ToInt32(row["LineNum"]) + 1,
                                item_code = row["ItemCode"]?.ToString(),
                                unit_code = row["UomCode"]?.ToString(),
                                expected_quantity = Convert.ToDecimal(row["expected_quantity"]),
                                lot_number = row["lot_number"]?.ToString(),
                                manufactured_at = ToNullableDateTime(row["manufactured_at"]),
                                expires_at = ToNullableDateTime(row["expires_at"]),
                                destination_location_code = row["destination_location_code"]?.ToString()
                            }).ToList()
                        };
                    }).ToList()
            };

            await PostToWarelogic("UrlReceipts", payload, database);
        }

        private static DateTime? ToNullableDateTime(object value)
        {
            if (value == null || value == DBNull.Value) return null;
            if (value is DateTime dt) return dt;

            var str = value.ToString();
            if (string.IsNullOrWhiteSpace(str)) return null;

            return DateTime.TryParse(str, System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out var parsed) ? parsed : null;
        }
    }
}