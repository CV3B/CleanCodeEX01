using Inlämningsuppgift_1.Core.Commands.Carts;
using Inlämningsuppgift_1.Data.Entities;
using Inlämningsuppgift_1.Data.Interfaces;
using Inlämningsuppgift1.Core.Core.Common;
using MediatR;

namespace Inlämningsuppgift_1.Core.Handlers.Commands.Carts
{
    public class AddToCartCommandHandler : IRequestHandler<AddToCartCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddToCartCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<Result<bool>> Handle(AddToCartCommand request, CancellationToken cancellationToken)
        {
            if (request.Quantity <= 0)
                return Task.FromResult<Result<bool>>(Errors.Validation);

            var product = _unitOfWork.Products.GetById(request.ProductId);
            if (product == null)
                return Task.FromResult<Result<bool>>(Errors.NotFound);

            var cartItems = _unitOfWork.Carts.GetByUserId(request.UserId) ?? new List<CartItem>();
            var existingItem = cartItems.FirstOrDefault(ci => ci.ProductId == request.ProductId);
            var totalQuantity = existingItem != null ? existingItem.Quantity + request.Quantity : request.Quantity;

            if (totalQuantity > product.Stock)
                return Task.FromResult<Result<bool>>(Errors.Validation);

            if (cartItems.Count == 0)
            {
                cartItems = new List<CartItem>
                {
                    new CartItem { ProductId = request.ProductId, Quantity = request.Quantity }
                };

                _unitOfWork.Carts.Add(request.UserId, cartItems);
            }
            else
            {
                if (existingItem == null)
                {
                    cartItems.Add(new CartItem { ProductId = request.ProductId, Quantity = request.Quantity });
                }
                else
                {
                    existingItem.Quantity += request.Quantity;
                }
                _unitOfWork.Carts.Add(request.UserId, cartItems);            
            }

            return Task.FromResult<Result<bool>>(true);
        }
    }
}
