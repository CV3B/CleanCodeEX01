using Inlämningsuppgift_1.Data.Entities;
using Inlämningsuppgift1.Core.Core.Common;
using static Inlämningsuppgift_1.Core.Interfaces.IQueries;

namespace Inlämningsuppgift_1.Core.Queries.Carts
{
    public class GetDetailedCartFromUserQuery : IQuery<Result<List<CartItemDetailed>?>>
    {
        public int UserId { get; set; }
    }
}
