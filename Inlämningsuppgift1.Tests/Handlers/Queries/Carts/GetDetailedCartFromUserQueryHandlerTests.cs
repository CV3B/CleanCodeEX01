using Inlämningsuppgift_1.Core.Handlers.Queries.Carts;
using Inlämningsuppgift_1.Core.Queries.Carts;
using Inlämningsuppgift_1.Data.Entities;
using Inlämningsuppgift_1.Data.Interfaces;
using Moq;

namespace Inlämningsuppgift1.Tests.Handlers.Queries.Carts
{
    public class GetDetailedCartFromUserQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ICartRepository> _mockCartRepository;
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly GetDetailedCartFromUserQueryHandler _handler;

        public GetDetailedCartFromUserQueryHandlerTests()
        {

            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockCartRepository = new Mock<ICartRepository>();
            _mockProductRepository = new Mock<IProductRepository>();

            _mockUnitOfWork.Setup(u => u.Carts).Returns(_mockCartRepository.Object);
            _mockUnitOfWork.Setup(u => u.Products).Returns(_mockProductRepository.Object);

            _handler = new GetDetailedCartFromUserQueryHandler(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_ValidUserId_ReturnsDetailedCartItems()
        {
            var userId = 1;
            var cartItems = new List<CartItem>
            {
                new CartItem { ProductId = 1, Quantity = 2 },
                new CartItem { ProductId = 2, Quantity = 1 }
            };

            _mockCartRepository.Setup(c => c.GetByUserId(userId))
                .Returns(cartItems);

            _mockProductRepository.Setup(p => p.GetById(1))
                .Returns(new Product { Id = 1, Name = "Product 1", Price = 10.0m, Stock = 10 });
            _mockProductRepository.Setup(p => p.GetById(2))
                .Returns(new Product { Id = 2, Name = "Product 2", Price = 20.0m, Stock = 5 });

            var query = new GetDetailedCartFromUserQuery { UserId = userId };
            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(2, result.Value.Count);
            Assert.Equal(1, result.Value[0].ProductId);
            Assert.Equal("Product 1", result.Value[0].ProductName);
            Assert.Equal(2, result.Value[0].Quantity);
            Assert.Equal(10.0m, result.Value[0].UnitPrice);
        }

        [Fact]
        public async Task Handle_EmptyCart_ReturnsEmptyList()
        {
            var userId = 1;

            _mockCartRepository.Setup(c => c.GetByUserId(userId))
                .Returns(new List<CartItem>());

            var query = new GetDetailedCartFromUserQuery { UserId = userId };
            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Empty(result.Value);
        }

        [Fact]
        public async Task Handle_NonExistentUserId_ReturnsEmptyList()
        {
            var userId = 999;
            _mockCartRepository.Setup(c => c.GetByUserId(userId))
                .Returns(new List<CartItem>());
            var query = new GetDetailedCartFromUserQuery { UserId = userId };
            var result = await _handler.Handle(query, CancellationToken.None);
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Value);
        }

    }
}
