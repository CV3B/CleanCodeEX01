using Inlämningsuppgift_1.Core.Commands.Products;
using Inlämningsuppgift_1.Core.Handlers.Commands.Products;
using Inlämningsuppgift_1.Data.Entities;
using Inlämningsuppgift_1.Data.Interfaces;
using Moq;

namespace Inlämningsuppgift1.Tests.Handlers.Commands.Products
{
    public class ChangeStockOfProductCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly ChangeStockOfProductCommandHandler _handler;

        public ChangeStockOfProductCommandHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockProductRepository = new Mock<IProductRepository>();

            _mockUnitOfWork.Setup(uow => uow.Products).Returns(_mockProductRepository.Object);
            _handler = new ChangeStockOfProductCommandHandler(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_UpdatesProductStock()
        {
            var command = new ChangeStockOfProductCommand
            {
                Id = 1,
                Amount = 50
            };
            var existingProduct = new Product
            {
                Id = 1,
                Name = "Test Product",
                Price = 100,
                Stock = 20
            };

            _mockProductRepository.Setup(repo => repo.GetById(command.Id)).Returns(existingProduct);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);
            _mockProductRepository.Verify(repo => repo.Update(It.Is<Product>(p => p.Stock == 70)), Times.Once);
        }

        [Fact]
        public async Task Handle_ProductNotFound_ReturnsFailure()
        {
            var command = new ChangeStockOfProductCommand
            {
                Id = 1,
                Amount = 50
            };

            _mockProductRepository.Setup(repo => repo.GetById(command.Id)).Returns((Product?)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task Handle_DecreaseStock_UpdatesProductStock()
        {
            var command = new ChangeStockOfProductCommand { Id = 1, Amount = -10 };
            var existingProduct = new Product { Id = 1, Name = "Test Product", Price = 100, Stock = 20 };

            _mockProductRepository.Setup(repo => repo.GetById(command.Id)).Returns(existingProduct);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);
            _mockProductRepository.Verify(repo => repo.Update(It.Is<Product>(p => p.Stock == 10)), Times.Once);
        }

        [Fact]
        public async Task Handle_DecreaseStock_BelowZero_ReturnsFailure()
        {
            var command = new ChangeStockOfProductCommand { Id = 1, Amount = -30 };
            var existingProduct = new Product { Id = 1, Name = "Test Product", Price = 100, Stock = 20 };

            _mockProductRepository.Setup(repo => repo.GetById(command.Id)).Returns(existingProduct);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
        }
    }
}
