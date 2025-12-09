using Inlämningsuppgift_1.Core.Commands.Products;
using Inlämningsuppgift_1.Data.Entities;
using Inlämningsuppgift_1.Data.Interfaces;
using Inlämningsuppgift1.Core.Core.Common;
using MediatR;

namespace Inlämningsuppgift_1.Core.Handlers.Commands.Products
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<Product?>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<Result<Product?>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return Task.FromResult<Result<Product?>>(Errors.Validation);

            if (request.Price <= 0)
                return Task.FromResult<Result<Product?>>(Errors.Validation);

            if (request.Stock < 0)
                return Task.FromResult<Result<Product?>>(Errors.Validation);

            _unitOfWork.BeginTransaction();

            var product = new Product
            {
                Name = request.Name,
                Price = request.Price,
                Stock = request.Stock
            };

            _unitOfWork.Products.Add(product);
            _unitOfWork.Commit();

            return Task.FromResult<Result<Product?>>(product);
        }
    }
}
