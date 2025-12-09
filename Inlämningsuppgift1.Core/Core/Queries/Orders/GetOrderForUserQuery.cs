using Inlämningsuppgift_1.Data.Entities;
using Inlämningsuppgift1.Core.Core.Common;
using static Inlämningsuppgift_1.Core.Interfaces.IQueries;

namespace Inlämningsuppgift_1.Core.Queries.Orders
{
    public class GetOrderForUserQuery : IQuery<Result<Order?>>
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
    }
}
