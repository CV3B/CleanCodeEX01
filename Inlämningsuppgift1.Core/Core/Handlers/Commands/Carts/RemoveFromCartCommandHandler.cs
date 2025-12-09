using Inlämningsuppgift_1.Core.Commands.Carts;
using Inlämningsuppgift_1.Data.Interfaces;
using Inlämningsuppgift1.Core.Core.Common;
using MediatR;

namespace Inlämningsuppgift_1.Core.Handlers.Commands.Carts
{
    public class RemoveFromCartCommandHandler : IRequestHandler<RemoveFromCartCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RemoveFromCartCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<Result<bool>> Handle(RemoveFromCartCommand request, CancellationToken cancellationToken)
        {
            var cartItems = _unitOfWork.Carts.GetByUserId(request.UserId);
            if (cartItems == null || cartItems.Count == 0)
                return Task.FromResult<Result<bool>>(Errors.NotFound);

            var existing = cartItems.FirstOrDefault(ci => ci.ProductId == request.ProductId);
            if (existing != null)
            {
                cartItems.Remove(existing);
                _unitOfWork.Carts.Add(request.UserId, cartItems);
            }

            return Task.FromResult<Result<bool>>(true);
        }
    }
}
