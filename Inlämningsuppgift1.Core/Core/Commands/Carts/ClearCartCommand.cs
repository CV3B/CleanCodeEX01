using Inlämningsuppgift1.Core.Core.Common;
using static Inlämningsuppgift_1.Core.Interfaces.ICommand;

namespace Inlämningsuppgift_1.Core.Commands.Carts
{
    public class ClearCartCommand : ICommand<Result<bool>>
    {
        public int UserId { get; set; }
    }
}
