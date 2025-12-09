using Inlämningsuppgift_1.Data.Entities;
using Inlämningsuppgift1.Core.Core.Common;
using static Inlämningsuppgift_1.Core.Interfaces.ICommand;

namespace Inlämningsuppgift_1.Core.Commands.Orders
{
    public class CreateOrderCommand : ICommand<Result<Order?>>
    {
        public int UserId { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new();
        public decimal Total { get; set; }
    }
}
