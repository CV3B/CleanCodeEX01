using Inlämningsuppgift_1.Core.Commands.Products;
using Inlämningsuppgift_1.Core.Handlers.Commands.Products;
using Inlämningsuppgift_1.Data.Entities;
using Inlämningsuppgift_1.Data.Interfaces;
using Moq;

namespace Inlämningsuppgift1.Tests.Handlers.Commands.Products
{
    public class UpdateProductCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly UpdateProductCommandHandler _handler;

        public UpdateProductCommandHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockProductRepository = new Mock<IProductRepository>();

            _mockUnitOfWork.Setup(uow => uow.Products).Returns(_mockProductRepository.Object);

            _handler = new UpdateProductCommandHandler(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_UpdatesProduct()
        {
            var command = new UpdateProductCommand
            {
                Product = new Product
                {
                    Id = 1,
                    Name = "Updated Product",
                    Price = 20.0m,
                    Stock = 50
                }
            };
            var existingProduct = new Product
            {
                Id = 1,
                Name = "Old Product",
                Price = 15.0m,
                Stock = 30
            };

            _mockProductRepository.Setup(repo => repo.GetById(command.Product.Id)).Returns(existingProduct);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);
            _mockProductRepository.Verify(repo => repo.Update(It.Is<Product>(p =>
                p.Id == command.Product.Id &&
                p.Name == command.Product.Name &&
                p.Price == command.Product.Price &&
                p.Stock == command.Product.Stock
            )), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.Commit(), Times.Once);
        }

        [Fact]
        public async Task Handle_ProductNotFound_ReturnsFailure()
        {
            var command = new UpdateProductCommand
            {
                Product = new Product
                {
                    Id = 1,
                    Name = "Updated Product",
                    Price = 20.0m,
                    Stock = 50
                }
            };

            _mockProductRepository.Setup(repo => repo.GetById(command.Product.Id)).Returns((Product?)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            _mockProductRepository.Verify(repo => repo.Update(It.IsAny<Product>()), Times.Never);
        }
    }
}
