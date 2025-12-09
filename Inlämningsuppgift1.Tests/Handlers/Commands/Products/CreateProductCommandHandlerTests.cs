using Inlämningsuppgift_1.Core.Commands.Products;
using Inlämningsuppgift_1.Core.Handlers.Commands.Products;
using Inlämningsuppgift_1.Data.Entities;
using Inlämningsuppgift_1.Data.Interfaces;
using Moq;

namespace Inlämningsuppgift1.Tests.Handlers.Commands.Products
{
    public class CreateProductCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly CreateProductCommandHandler _handler;

        public CreateProductCommandHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockProductRepository = new Mock<IProductRepository>();

            _mockUnitOfWork.Setup(uow => uow.Products).Returns(_mockProductRepository.Object);
            _handler = new CreateProductCommandHandler(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_AddsProductAndReturnsTrue()
        {
            var command = new CreateProductCommand
            {
                Name = "Test Product",
                Price = 99.99m,
                Stock = 10
            };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(command.Name, result.Value.Name);
            Assert.Equal(command.Price, result.Value.Price);
            Assert.Equal(command.Stock, result.Value.Stock);

            _mockProductRepository.Verify(repo => repo.Add(It.Is<Product>(p =>
                p.Name == command.Name &&
                p.Price == command.Price &&
                p.Stock == command.Stock)), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.Commit(), Times.Once);
        }

    }
}
