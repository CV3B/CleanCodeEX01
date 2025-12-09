using System.ComponentModel.DataAnnotations;

namespace Inlämningsuppgift_1.Api.DTOs.Requests
{
    public class CreateProductRequest 
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = "";
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; } 
        [Range(0, int.MaxValue)]
        public int Stock { get; set; } 
    }

}
