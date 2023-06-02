namespace GrpcServiceDemo.Models
{
    public class Order
    {
        public long OrderId { get; set; }
        public string? ShipAddress { get; set; }
        public string? ShipCity { get; set; }
        public string? ShipPostalCode { get; set; }
        public int OrderQuantity { get; set; }
    }
}
