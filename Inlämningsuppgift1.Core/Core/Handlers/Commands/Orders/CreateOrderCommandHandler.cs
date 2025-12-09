using Inlämningsuppgift_1.Core.Commands.Orders;
using Inlämningsuppgift_1.Data.Entities;
using Inlämningsuppgift_1.Data.Interfaces;
using Inlämningsuppgift1.Core.Core.Common;
using MediatR;

namespace Inlämningsuppgift_1.Core.Handlers.Commands.Orders
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Result<Order?>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateOrderCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<Result<Order?>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            _unitOfWork.BeginTransaction();

            try
            {
                var order = new Order
                {
                    UserId = request.UserId,
                    CreatedAt = DateTime.UtcNow,
                    OrderItems = request.OrderItems,
                    Total = request.Total
                };

                _unitOfWork.Orders.Add(order);
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
