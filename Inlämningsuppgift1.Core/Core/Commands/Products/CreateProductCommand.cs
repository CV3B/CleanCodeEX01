using Inlämningsuppgift_1.Data.Entities;
using Inlämningsuppgift1.Core.Core.Common;
using static Inlämningsuppgift_1.Core.Interfaces.ICommand;

namespace Inlämningsuppgift_1.Core.Commands.Products
{
    public class CreateProductCommand : ICommand<Result<Product?>>
    {
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
