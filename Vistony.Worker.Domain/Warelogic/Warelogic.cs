namespace Vistony.Worker.Domain.Warelogic
{
    // ---- UOM ----
    public class UomPayload
    {
        public string source_system { get; set; } = "SAP";
        public string payload_version { get; set; } = "1.0";
        public string correlation_id { get; set; }
        public string event_type { get; set; } = "UPSERT";
        public List<Uom> units { get; set; }
    }

    public class Uom
    {
        public string external_id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string symbol { get; set; }
        public int decimal_precision { get; set; }
        public bool is_active { get; set; }
    }

    // ---- Items ----
    public class ItemPayload
    {
        public string source_system { get; set; } = "SAP";
        public string payload_version { get; set; } = "1.0";
        public string correlation_id { get; set; }
        public string event_type { get; set; } = "UPSERT";
        public List<Item> items { get; set; }
    }

    public class Item
    {
        public string external_id { get; set; }
        public string event_type { get; set; } = "UPSERT";
        public string source_system { get; set; } = "SAP";
        public string payload_version { get; set; } = "1.0";
        public string correlation_id { get; set; }
        public string item_code { get; set; }
        public string item_name { get; set; }
        public string description { get; set; }
        public string base_unit_code { get; set; }
        public string family_code { get; set; }
        public string primary_barcode { get; set; }
        public bool lot_control { get; set; }
        public bool serial_control { get; set; }
        public bool expiration_control { get; set; }
        public int shelf_life_days { get; set; }
        public List<ItemUnit> units { get; set; }
        public bool is_active { get; set; }
    }

    public class ItemUnit
    {
        public string unit_code { get; set; }
        public decimal conversion_factor { get; set; }
        public string barcode { get; set; }
        public bool is_base_unit { get; set; }
    }

    // ---- Outbound Orders ----
    public class NoutboundPayload
    {
        public string source_system { get; set; } = "SAP";
        public string payload_version { get; set; } = "1.0";
        public string correlation_id { get; set; }
        public string event_type { get; set; } = "UPSERT";
        public List<OutboundOrder> outbound_orders { get; set; }
    }

    public class OutboundOrder
    {
        public string external_id { get; set; }
        public string event_type { get; set; } = "UPSERT";
        public string source_system { get; set; } = "SAP";
        public string payload_version { get; set; } = "1.0";
        public string correlation_id { get; set; }
        public string order_number { get; set; }
        public string external_reference { get; set; }
        public string order_type { get; set; }
        public string branch_code { get; set; }
        public string warehouse_code { get; set; }
        public int priority { get; set; }
        public string customer_code { get; set; }
        public string customer_name { get; set; }
        public string ship_to_code { get; set; }
        public string ship_to_name { get; set; }
        public string ship_to_address { get; set; }
        public string ship_to_city { get; set; }
        public string ship_to_region { get; set; }
        public string ship_to_country_code { get; set; }
        public string source_document_type { get; set; }
        public string source_document_number { get; set; }
        public DateTime required_at { get; set; }
        public string notes { get; set; }
        public List<OutboundLine> lines { get; set; }
    }

    public class OutboundLine
    {
        public string external_line_id { get; set; }
        public int line_number { get; set; }
        public string item_code { get; set; }
        public string unit_code { get; set; }
        public decimal requested_quantity { get; set; }
        public string requested_lot_number { get; set; }
    }

    // ---- Receipts ----
    public class ReceiptPayload
    {
        public string source_system { get; set; } = "SAP";
        public string payload_version { get; set; } = "1.0";
        public string correlation_id { get; set; }
        public string event_type { get; set; } = "UPSERT";
        public List<Receipt> receipts { get; set; }
    }

    public class Receipt
    {
        public string external_id { get; set; }
        public string event_type { get; set; } = "UPSERT";
        public string source_system { get; set; } = "SAP";
        public string payload_version { get; set; } = "1.0";
        public string correlation_id { get; set; }
        public string receipt_number { get; set; }
        public string external_reference { get; set; }
        public string receipt_type { get; set; } = "PURCHASE";
        public string branch_code { get; set; }
        public string warehouse_code { get; set; }
        public string supplier_code { get; set; }
        public string supplier_name { get; set; }
        public string source_document_type { get; set; } = "PURCHASE_ORDER";
        public string source_document_number { get; set; }
        public DateTime scheduled_at { get; set; }
        public string notes { get; set; }
        public List<ReceiptLine> lines { get; set; }
    }

    public class ReceiptLine
    {
        public string external_line_id { get; set; }
        public int line_number { get; set; }
        public string item_code { get; set; }
        public string unit_code { get; set; }
        public decimal expected_quantity { get; set; }
        public string lot_number { get; set; }
        public DateTime? manufactured_at { get; set; }
        public DateTime? expires_at { get; set; }
        public string destination_location_code { get; set; }
    }
}