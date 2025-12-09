using Inlämningsuppgift_1.Core.Queries.Products;
using Inlämningsuppgift_1.Data.Entities;
using Inlämningsuppgift_1.Data.Interfaces;
using Inlämningsuppgift1.Core.Core.Common;
using MediatR;

namespace Inlämningsuppgift_1.Core.Handlers.Queries.Products
{
    public class SearchForProductQueryHandler : IRequestHandler<SearchForProductQuery, Result<List<Product>?>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SearchForProductQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<Result<List<Product>?>> Handle(SearchForProductQuery request, CancellationToken cancellationToken)
        {
            var Products = _unitOfWork.Products.GetAll();

            if (string.IsNullOrWhiteSpace(request.Query))
                return Task.FromResult<Result<List<Product>?>>(Products);

            var normalizedQuery = request.Query.ToLowerInvariant();

            var foundProducts = Products.Where(p => p.Name.ToLowerInvariant().Contains(normalizedQuery) 
                                    || p.Price.ToString().Contains(normalizedQuery)).ToList();

            if (request.MaxPrice.HasValue)
            {
                foundProducts = foundProducts.Where(p => p.Price <= request.MaxPrice.Value).ToList();
            }

            return Task.FromResult<Result<List<Product>?>>(foundProducts);
        }
    }
}
