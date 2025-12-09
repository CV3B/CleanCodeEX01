namespace Inlämningsuppgift_1.Api.DTOs.Responses
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<OrderItemResponse> Items { get; set; } = new();
        public decimal Total { get; set; }
    }
}
