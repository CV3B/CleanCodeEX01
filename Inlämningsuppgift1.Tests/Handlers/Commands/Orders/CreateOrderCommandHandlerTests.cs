using Inlämningsuppgift_1.Core.Commands.Orders;
using Inlämningsuppgift_1.Core.Handlers.Commands.Orders;
using Inlämningsuppgift_1.Data.Entities;
using Inlämningsuppgift_1.Data.Interfaces;
using Moq;

namespace Inlämningsuppgift1.Tests.Handlers.Commands.Orders
{
    public class CreateOrderCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly CreateOrderCommandHandler _handler;

        public CreateOrderCommandHandlerTests()
        {

            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockOrderRepository = new Mock<IOrderRepository>();

            _mockUnitOfWork.Setup(u => u.Orders).Returns(_mockOrderRepository.Object);

            _handler = new CreateOrderCommandHandler(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_AddsOrderAndReturnsTrue()
        {
            var command = new CreateOrderCommand
            {
                UserId = 1,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { ProductId = 1, Quantity = 2 },
                    new OrderItem { ProductId = 2, Quantity = 1 }
                }
            };

            var result = await _handler.Handle(command, CancellationToken.None);

            _mockOrderRepository.Verify(r => r.Add(It.IsAny<Order>()), Times.Once);
        }

    }
}
