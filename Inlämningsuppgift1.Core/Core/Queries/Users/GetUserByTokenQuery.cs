using Inlämningsuppgift_1.Data.Entities;
using Inlämningsuppgift1.Core.Core.Common;
using static Inlämningsuppgift_1.Core.Interfaces.IQueries;

namespace Inlämningsuppgift_1.Core.Queries.Users
{
    public class GetUserByTokenQuery : IQuery<Result<User?>>
    {
        public string Token { get; set; } = "";
    }
}
