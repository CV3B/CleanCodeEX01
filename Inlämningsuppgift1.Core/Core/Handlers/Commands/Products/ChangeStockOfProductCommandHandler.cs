using Inlämningsuppgift_1.Core.Commands.Products;
using Inlämningsuppgift_1.Data.Interfaces;
using Inlämningsuppgift1.Core.Core.Common;
using MediatR;

namespace Inlämningsuppgift_1.Core.Handlers.Commands.Products
{
    public class ChangeStockOfProductCommandHandler : IRequestHandler<ChangeStockOfProductCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChangeStockOfProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<Result<bool>> Handle(ChangeStockOfProductCommand request, CancellationToken cancellationToken)
        {
            var product = _unitOfWork.Products.GetById(request.Id);

            if (product == null)
                return Task.FromResult<Result<bool>>(Errors.NotFound);
            if (product.Stock + request.Amount < 0)
                return Task.FromResult<Result<bool>>(Errors.InsufficientStock);

            product.Stock += request.Amount;

            _unitOfWork.Products.Update(product);

            return Task.FromResult<Result<bool>>(true);
        }
    }
}
