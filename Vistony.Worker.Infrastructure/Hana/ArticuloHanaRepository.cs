using Microsoft.Extensions.Configuration;
using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.Articulo.Interfaces;

namespace Vistony.Worker.Infrastructure.Hana
{
    public class ArticuloHanaRepository : IArticuloRepository
    {
        private readonly string _connPeru;
        private readonly string _connInduvis;

        public ArticuloHanaRepository(IConfiguration configuration)
        {
            _connPeru = configuration.GetConnectionString("HanaConnectionSEIDOR_PERU")!;
            _connInduvis = configuration.GetConnectionString("HanaConnectionSEIDOR")!;
        }

        private string GetConnectionString(string location)
            => location == "PE" ? _connPeru : _connInduvis;

        public async Task<List<string>> ObtenerCodigosAsync(string location)
        {
            var result = new List<string>();

            await using var connection = new HanaConnection(GetConnectionString(location));
            await connection.OpenAsync();

            var query = $"CALL \"B1H_VIST_{location}\".\"API_WMS_WHS_GETITEM\"()";
            await using var command = new HanaCommand(query, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var itemCode = Convert.ToString(reader["ItemCode"]);
                if (!string.IsNullOrWhiteSpace(itemCode))
                    result.Add(itemCode);
            }
            return result;
        }

        public async Task<Domain.Articulo.Articulo?> ObtenerDetalleAsync(string location, string companyId, string itemCode)
        {
            await using var connection = new HanaConnection(GetConnectionString(location));
            await connection.OpenAsync();

            // parametrizado para evitar problemas con comillas en el ItemCode
            var query = $"CALL \"B1H_VIST_{location}\".\"API_WMS_WHS_GETITEM_PARAMETERS\"(?)";
            await using var command = new HanaCommand(query, connection);
            command.Parameters.Add(new HanaParameter { Value = itemCode });

            await using var reader = await command.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
                return null;

            return new Domain.Articulo.Articulo
            {
                CompanyId = companyId,
                CorpLine = Str(reader["U_VIS_Corp_Line"]),
                InventoryWeight = Dbl(reader["InventoryWeight"]),
                ItemCode = Str(reader["ItemCode"]),
                ItemName = Str(reader["ItemName"]),
                ItemsGroupCode = Str(reader["ItemsGroupCode"]),
                PurchaseUnitHeight = Str(reader["PurchaseUnitHeight"]),
                PurchaseUnitLength = Str(reader["PurchaseUnitLength"]),
                PurchaseUnitWidth = Str(reader["PurchaseUnitWidth"]),
                QtyPallet = Dbl(reader["QtyPallet"]),
                UoMGroupEntry = Str(reader["UoMGroupEntry"])
            };
        }

        private static string Str(object v) => v is DBNull ? string.Empty : Convert.ToString(v) ?? string.Empty;
        private static double Dbl(object v) => v is DBNull ? 0 : Convert.ToDouble(v);
    }
}
