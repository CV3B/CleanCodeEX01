using Inlämningsuppgift_1.Core.Commands.Orders;
using Inlämningsuppgift_1.Data.Entities;
using Inlämningsuppgift_1.Data.Interfaces;
using Inlämningsuppgift1.Core.Core.Common;
using MediatR;

namespace Inlämningsuppgift_1.Core.Handlers.Commands.Orders
{
    public class CreateOrderFromCartCommandHandler : IRequestHandler<CreateOrderFromCartCommand, Result<Order?>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateOrderFromCartCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<Result<Order?>> Handle(CreateOrderFromCartCommand request, CancellationToken cancellationToken)
        {
            var cart = _unitOfWork.Carts.GetByUserId(request.UserId);

            if (cart.Count == 0)
            {
                return Task.FromResult<Result<Order?>>(Errors.Validation);
            }

            _unitOfWork.BeginTransaction();

            try
            {

                var orderItems = new List<OrderItem>();
                var total = 0m;

                foreach (var item in cart)
                {
                    var product = _unitOfWork.Products.GetById(item.ProductId);
                    if (product == null)
                    {
                        return Task.FromResult<Result<Order?>>(Errors.NotFound);
                    }

                    if (product.Stock < item.Quantity)
                    {
                        return Task.FromResult<Result<Order?>>(Errors.InsufficientStock);
                    }

                    product.Stock -= item.Quantity;
                    _unitOfWork.Products.Update(product);

                    orderItems.Add(new OrderItem
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        Quantity = item.Quantity,
                        Price = product.Price
                    });

                    total += product.Price * item.Quantity;
                }

                var order = new Order
                {
                    UserId = request.UserId,
                    CreatedAt = DateTime.UtcNow,
                    OrderItems = orderItems,
                    Total = total
                };

                _unitOfWork.Orders.Add(order);

                _unitOfWork.Carts.Clear(request.UserId);

                _unitOfWork.Commit();
                return Task.FromResult<Result<Order?>>(order);
            }
            catch
            {
                _unitOfWork.Rollback();
                throw;
            }
        }
    }
}
