namespace Inventory.Application.Model.Request
{
    public class UpdateInventoryOrderWithItems
    {
        public long? Id { get; set; }
        public string? ExternalOrderReferenceNumber { get; set; }
        public long? VendorId { get; set; }
        public decimal? Discount { get; set; }
        public decimal? ShippingFee { get; set; }
        public List<UpdateInventoryOrderItemForOrder>? Items { get; set; }
    }

    public class UpdateInventoryOrderItemForOrder
    {
        public long? Id { get; set; }
        public long? ProductId { get; set; }
        public int? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
    }
}