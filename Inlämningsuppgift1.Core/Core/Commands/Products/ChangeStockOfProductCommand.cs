using Inlämningsuppgift1.Core.Core.Common;
using static Inlämningsuppgift_1.Core.Interfaces.ICommand;

namespace Inlämningsuppgift_1.Core.Commands.Products
{
    public class ChangeStockOfProductCommand : ICommand<Result<bool>>
    {
        public int Id { get; set; }
        public int Amount { get; set; }
    }
}
