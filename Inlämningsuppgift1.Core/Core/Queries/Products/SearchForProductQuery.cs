using Inlämningsuppgift_1.Data.Entities;
using Inlämningsuppgift1.Core.Core.Common;
using static Inlämningsuppgift_1.Core.Interfaces.IQueries;

namespace Inlämningsuppgift_1.Core.Queries.Products
{
    public class SearchForProductQuery : IQuery<Result<List<Product>?>>
    {
        public string? Query { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
