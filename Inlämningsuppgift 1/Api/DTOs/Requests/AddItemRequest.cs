using System.ComponentModel.DataAnnotations;

namespace Inlämningsuppgift_1.Api.DTOs.Requests
{
    public class AddItemRequest 
    { 
        public int ProductId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; } 
    }

}
