using Inlämningsuppgift_1.Core.Queries.Products;
using Inlämningsuppgift_1.Data.Entities;
using Inlämningsuppgift_1.Data.Interfaces;
using Inlämningsuppgift1.Core.Core.Common;
using MediatR;

namespace Inlämningsuppgift_1.Core.Handlers.Queries.Products
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Result<Product?>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetProductByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<Result<Product?>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = _unitOfWork.Products.GetById(request.Id);

            if (product == null)
                return Task.FromResult<Result<Product?>>(Errors.NotFound);

            return Task.FromResult<Result<Product?>>(product);
        }
    }
}
