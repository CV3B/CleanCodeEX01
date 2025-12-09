using Inlämningsuppgift_1.Core.Queries.Carts;
using Inlämningsuppgift_1.Data.Entities;
using Inlämningsuppgift_1.Data.Interfaces;
using Inlämningsuppgift1.Core.Core.Common;
using MediatR;

namespace Inlämningsuppgift_1.Core.Handlers.Queries.Carts
{
    public class GetDetailedCartFromUserQueryHandler : IRequestHandler<GetDetailedCartFromUserQuery, Result<List<CartItemDetailed>?>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetDetailedCartFromUserQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<Result<List<CartItemDetailed>?>> Handle(GetDetailedCartFromUserQuery request, CancellationToken cancellationToken)
        {
            var cart = _unitOfWork.Carts.GetByUserId(request.UserId);

            if (cart == null || !cart.Any())
                return Task.FromResult<Result<List<CartItemDetailed>?>>(new List<CartItemDetailed>());

            var detailedCart = cart.Select(ci =>
            {
                var product = _unitOfWork.Products.GetById(ci.ProductId);
                return new CartItemDetailed
                {
                    ProductId = ci.ProductId,
                    ProductName = product?.Name ?? "Unknown",
                    Quantity = ci.Quantity,
                    UnitPrice = product?.Price ?? 0
                };
            }).ToList();

            return Task.FromResult<Result<List<CartItemDetailed>?>>(detailedCart);
        }
    }
}
