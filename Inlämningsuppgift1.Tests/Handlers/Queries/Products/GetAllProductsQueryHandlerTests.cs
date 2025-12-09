using Inlämningsuppgift_1.Core.Handlers.Queries.Products;
using Inlämningsuppgift_1.Core.Queries.Products;
using Inlämningsuppgift_1.Data.Entities;
using Inlämningsuppgift_1.Data.Interfaces;
using Moq;

namespace Inlämningsuppgift1.Tests.Handlers.Queries.Products
{
    public class GetAllProductsQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly GetAllProductsQueryHandler _handler;

        public GetAllProductsQueryHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockProductRepository = new Mock<IProductRepository>();
            _mockUnitOfWork.Setup(u => u.Products).Returns(_mockProductRepository.Object);
            _handler = new GetAllProductsQueryHandler(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_ReturnsAllProducts()
        {
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Price = 10.0m, Stock = 5 },
                new Product { Id = 2, Name = "Product 2", Price = 20.0m, Stock = 3 }
            };
            _mockProductRepository.Setup(r => r.GetAll()).Returns(products);

            var query = new GetAllProductsQuery();
            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(2, result.Value.Count);
            Assert.Equal("Product 1", result.Value[0].Name);
            Assert.Equal("Product 2", result.Value[1].Name);
        }
    }
}
