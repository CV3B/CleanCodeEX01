using Inlämningsuppgift_1.Core.Commands.Products;
using Inlämningsuppgift_1.Data.Interfaces;
using Inlämningsuppgift1.Core.Core.Common;
using MediatR;

namespace Inlämningsuppgift_1.Core.Handlers.Commands.Products
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<Result<bool>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var existingProduct = _unitOfWork.Products.GetById(request.Product.Id);

            if (existingProduct == null)
                return Task.FromResult<Result<bool>>(Errors.NotFound);

            _unitOfWork.BeginTransaction();

            existingProduct.Name = request.Product.Name;
            existingProduct.Price = request.Product.Price;
            existingProduct.Stock = request.Product.Stock;

            _unitOfWork.Products.Update(existingProduct);

            _unitOfWork.Commit();

            return Task.FromResult<Result<bool>>(true);
        }
    }
}
