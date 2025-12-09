using Inlämningsuppgift_1.Data.Entities;
using Inlämningsuppgift1.Core.Core.Common;
using static Inlämningsuppgift_1.Core.Interfaces.ICommand;

namespace Inlämningsuppgift_1.Core.Commands.Products
{
    public class UpdateProductCommand : ICommand<Result<bool>>
    {
        public Product Product { get; set; }
    }
}
