using Inlämningsuppgift_1.Core.Queries.Products;
using Inlämningsuppgift_1.Data.Entities;
using Inlämningsuppgift_1.Data.Interfaces;
using Inlämningsuppgift1.Core.Core.Common;
using MediatR;

namespace Inlämningsuppgift_1.Core.Handlers.Queries.Products
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, Result<List<Product>?>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllProductsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<Result<List<Product>?>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var products = _unitOfWork.Products.GetAll();

            if (products == null)
                return Task.FromResult<Result<List<Product>?>>(Errors.NotFound);

            return Task.FromResult<Result<List<Product>?>>(products);
        }
    }
}
