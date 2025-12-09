using Inlämningsuppgift_1.Core.Commands.Orders;
using Inlämningsuppgift_1.Core.Handlers.Commands.Orders;
using Inlämningsuppgift_1.Data.Entities;
using Inlämningsuppgift_1.Data.Interfaces;
using Moq;

namespace Inlämningsuppgift1.Tests.Handlers.Commands.Orders
{
    public class CreateOrderFromCartCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly Mock<ICartRepository> _mockCartRepository;
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly CreateOrderFromCartCommandHandler _handler;

        public CreateOrderFromCartCommandHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockOrderRepository = new Mock<IOrderRepository>();
            _mockCartRepository = new Mock<ICartRepository>();
            _mockProductRepository = new Mock<IProductRepository>();

            _mockUnitOfWork.Setup(uow => uow.Orders).Returns(_mockOrderRepository.Object);
            _mockUnitOfWork.Setup(uow => uow.Carts).Returns(_mockCartRepository.Object);
            _mockUnitOfWork.Setup(uow => uow.Products).Returns(_mockProductRepository.Object);

            _handler = new CreateOrderFromCartCommandHandler(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_CreatesOrderSuccessfully()
        {
            var cartItems = new List<CartItem>
            {
                new CartItem { ProductId = 1, Quantity = 2 },
                new CartItem { ProductId = 2, Quantity = 1 }
            };

            _mockCartRepository.Setup(c => c.GetByUserId(1)).Returns(cartItems);
            _mockProductRepository.Setup(p => p.GetById(1)).Returns(new Product { Id = 1, Name = "Product 1", Price = 10.0m, Stock = 10 });
            _mockProductRepository.Setup(p => p.GetById(2)).Returns(new Product { Id = 2, Name = "Product 2", Price = 20.0m, Stock = 5 });

            var result = await _handler.Handle(new CreateOrderFromCartCommand { UserId = 1 }, CancellationToken.None);

            Assert.True(result.IsSuccess);
            _mockOrderRepository.Verify(repo => repo.Add(It.IsAny<Order>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.Commit(), Times.Once);
            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task Handle_EmptyCart_ReturnsFailure()
        {
            _mockCartRepository.Setup(c => c.GetByUserId(2)).Returns(new List<CartItem>());

            var result = await _handler.Handle(new CreateOrderFromCartCommand { UserId = 2 }, CancellationToken.None);

            Assert.False(result.IsSuccess);
        }
    }
}
