namespace Inlämningsuppgift_1.Api.DTOs.Responses
{
    public class OrderItemResponse
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = "";
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
