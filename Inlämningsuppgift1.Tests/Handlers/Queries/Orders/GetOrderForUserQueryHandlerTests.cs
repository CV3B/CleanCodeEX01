using Inlämningsuppgift_1.Core.Handlers.Queries.Orders;
using Inlämningsuppgift_1.Core.Queries.Orders;
using Inlämningsuppgift_1.Data.Entities;
using Inlämningsuppgift_1.Data.Interfaces;
using Moq;

namespace Inlämningsuppgift1.Tests.Handlers.Queries.Orders
{
    public class GetOrderForUserQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly GetOrderForUserQueryHandler _handler;

        public GetOrderForUserQueryHandlerTests()
        {

            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockOrderRepository = new Mock<IOrderRepository>();

            _mockUnitOfWork.Setup(u => u.Orders).Returns(_mockOrderRepository.Object);

            _handler = new GetOrderForUserQueryHandler(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_ValidOrderIdAndUserId_ReturnsOrder()
        {
            var userId = 1;
            var orderId = 1;
            var order = new Order
            {
                Id = orderId,
                UserId = userId,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { ProductId = 1, ProductName = "Test", Quantity = 2, Price = 10.0m }
                }
            };

            _mockOrderRepository.Setup(o => o.GetByOrderId(orderId))
                .Returns(order);

            var query = new GetOrderForUserQuery { UserId = userId, OrderId = orderId };
            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(orderId, result.Value.Id);
            Assert.Equal(userId, result.Value.UserId);
        }

        [Fact]
        public async Task Handle_OrderNotFound_ReturnsFailure()
        {
            var userId = 2;
            var orderId = 999;

            _mockOrderRepository.Setup(o => o.GetByOrderId(orderId))
                .Returns((Order?)null);

            var query = new GetOrderForUserQuery { UserId = userId, OrderId = orderId };
            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task Handle_UserDoesNotOwnOrder_ReturnsFailure()
        {
            var userId = 1;
            var orderId = 1;
            var order = new Order { Id = orderId, UserId = 2 };

            _mockOrderRepository.Setup(o => o.GetByOrderId(orderId))
                .Returns(order);

            var query = new GetOrderForUserQuery { UserId = userId, OrderId = orderId };
            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.False(result.IsSuccess);
        }
    }
}
