using Inlämningsuppgift_1.Core.Handlers.Queries.Products;
using Inlämningsuppgift_1.Core.Queries.Products;
using Inlämningsuppgift_1.Data.Entities;
using Inlämningsuppgift_1.Data.Interfaces;
using Moq;

namespace Inlämningsuppgift1.Tests.Handlers.Queries.Products
{
    public class GetProductByIdQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly GetProductByIdQueryHandler _handler;

        public GetProductByIdQueryHandlerTests()
        {

            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockProductRepository = new Mock<IProductRepository>();

            _mockUnitOfWork.Setup(u => u.Products).Returns(_mockProductRepository.Object);

            _handler = new GetProductByIdQueryHandler(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_ValidId_ReturnsProduct()
        {
            var productId = 1;
            var product = new Product
            {
                Id = productId,
                Name = "Test Product",
                Price = 100.0m,
                Stock = 10
            };

            _mockProductRepository.Setup(r => r.GetById(productId)).Returns(product);

            var query = new GetProductByIdQuery { Id = productId };
            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(productId, result.Value.Id);
            Assert.Equal("Test Product", result.Value.Name);
        }

        [Fact]
        public async Task Handle_InvalidId_ReturnsFailure()
        {
            var productId = 999;

            _mockProductRepository.Setup(r => r.GetById(productId)).Returns((Product?)null);

            var query = new GetProductByIdQuery { Id = productId };
            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.False(result.IsSuccess);
        }

    }
}
