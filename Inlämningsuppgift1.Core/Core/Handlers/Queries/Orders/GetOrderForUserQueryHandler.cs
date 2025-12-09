using Inlämningsuppgift_1.Core.Queries.Orders;
using Inlämningsuppgift_1.Data.Entities;
using Inlämningsuppgift_1.Data.Interfaces;
using Inlämningsuppgift1.Core.Core.Common;
using MediatR;

namespace Inlämningsuppgift_1.Core.Handlers.Queries.Orders
{
    public class GetOrderForUserQueryHandler : IRequestHandler<GetOrderForUserQuery, Result<Order?>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetOrderForUserQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<Result<Order?>> Handle(GetOrderForUserQuery request, CancellationToken cancellationToken)
        {
            var order = _unitOfWork.Orders.GetByOrderId(request.OrderId);

            if (order == null)
                return Task.FromResult<Result<Order?>>(Errors.NotFound);

            if (order.UserId != request.UserId)
                return Task.FromResult<Result<Order?>>(Errors.Unauthorized);

            return Task.FromResult<Result<Order?>>(order);
        }
    }
}
