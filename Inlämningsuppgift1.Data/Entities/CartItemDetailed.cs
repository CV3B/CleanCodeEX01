namespace Inlämningsuppgift_1.Data.Entities
{
    public class CartItemDetailed
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
    }
}
