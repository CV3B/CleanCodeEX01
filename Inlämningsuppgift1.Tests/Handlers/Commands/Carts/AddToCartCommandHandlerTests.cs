using Inlämningsuppgift_1.Core.Commands.Carts;
using Inlämningsuppgift_1.Core.Handlers.Commands.Carts;
using Inlämningsuppgift_1.Data.Entities;
using Inlämningsuppgift_1.Data.Interfaces;
using Moq;

namespace Inlämningsuppgift1.Tests.Handlers.Commands.Carts
{
    public class AddToCartCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ICartRepository> _mockCartRepository;
        private readonly AddToCartCommandHandler _handler;

        public AddToCartCommandHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockCartRepository = new Mock<ICartRepository>();
            _mockUnitOfWork.Setup(u => u.Carts).Returns(_mockCartRepository.Object);
            _handler = new AddToCartCommandHandler(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_AddsItemToCart()
        {
            var command = new AddToCartCommand
            {
                UserId = 1,
                ProductId = 1,
                Quantity = 2
            };

            _mockUnitOfWork.Setup(u => u.Products.GetById(command.ProductId)).Returns(new Product { Id = command.ProductId, Stock = 10 });
            _mockCartRepository.Setup(c => c.GetByUserId(command.UserId)).Returns(new List<CartItem>());

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);
            _mockCartRepository.Verify(c => c.Add(command.UserId, It.IsAny<List<CartItem>>()), Times.Once);
        }

        [Fact]
        public async Task Handle_InsufficientStock_ReturnsFailure()
        {
            var command = new AddToCartCommand 
            { 
                UserId = 1, 
                ProductId = 1, 
                Quantity = 5 
            };

            _mockUnitOfWork.Setup(u => u.Products.GetById(1)).Returns(new Product { Id = 1, Stock = 3 });
            _mockCartRepository.Setup(c => c.GetByUserId(1)).Returns(new List<CartItem>());

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task Handle_NonExistentProduct_ReturnsFailure()
        {
            var command = new AddToCartCommand 
            { 
                UserId = 1, 
                ProductId = 999, 
                Quantity = 1 
            };

            _mockUnitOfWork.Setup(u => u.Products.GetById(999)).Returns((Product?)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task Handle_NegativeQuantity_ReturnsFailure()
        {
            var command = new AddToCartCommand 
            { 
                UserId = 1, 
                ProductId = 1, 
                Quantity = -1 
            };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
        }
    }
}
