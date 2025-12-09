using Inlämningsuppgift_1.Core.Handlers.Queries.Products;
using Inlämningsuppgift_1.Core.Queries.Products;
using Inlämningsuppgift_1.Data.Entities;
using Inlämningsuppgift_1.Data.Interfaces;
using Moq;

namespace Inlämningsuppgift1.Tests.Handlers.Queries.Products
{
    public class SearchForProductQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly SearchForProductQueryHandler _handler;

        public SearchForProductQueryHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockProductRepository = new Mock<IProductRepository>();

            _mockUnitOfWork.Setup(u => u.Products).Returns(_mockProductRepository.Object);

            _handler = new SearchForProductQueryHandler(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_ValidSearchTerm_ReturnsMatchingProducts()
        {
            var searchTerm = "Test";
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Test Product 1", Price = 10.0m, Stock = 5 },
                new Product { Id = 2, Name = "Another Product", Price = 20.0m, Stock = 3 },
                new Product { Id = 3, Name = "Test Product 2", Price = 15.0m, Stock = 8 }
            };
            _mockProductRepository.Setup(r => r.GetAll()).Returns(products);

            var result = await _handler.Handle(new SearchForProductQuery { Query = searchTerm }, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(2, result.Value.Count);
            Assert.All(result.Value, p => Assert.Contains(searchTerm, p.Name));
        }
    }
}
