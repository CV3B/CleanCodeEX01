using Inlämningsuppgift_1.Data.Entities;
using Inlämningsuppgift_1.Data.Repositories;
using Inlämningsuppgift1.Tests.TestHelpers.Fakes;

namespace Inlämningsuppgift1.Tests.Repositories
{

    public class ProductRepositoryTests
    {
        private readonly FakeInMemoryDatabase _fakeDatabase;
        private readonly ProductRepository _repository;

        public ProductRepositoryTests()
        {
            _fakeDatabase = new FakeInMemoryDatabase();
            _repository = new ProductRepository(_fakeDatabase);
        }

        [Fact]
        public void Add_ValidProduct_ProductIsAddedToDatabase()
        {
            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                Price = 99.99m,
                Stock = 50
            };

            _repository.Add(product);

            Assert.Single(_fakeDatabase.Products);
            Assert.Equal(product.Name, _fakeDatabase.Products[0].Name);
            Assert.Equal(product.Price, _fakeDatabase.Products[0].Price);
            Assert.Equal(product.Stock, _fakeDatabase.Products[0].Stock);
        }

        [Fact]
        public void GetById_ExistingProduct_ReturnsProduct()
        {
            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                Price = 10m,
                Stock = 5
            };
            _fakeDatabase.Products.Add(product);

            var result = _repository.GetById(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Test Product", result.Name);
        }

        [Fact]
        public void GetById_NonExistingProduct_ReturnsNull()
        {
            var result = _repository.GetById(999);

            Assert.Null(result);
        }

        [Fact]
        public void GetAll_ReturnsAllProducts()
        {
            _fakeDatabase.Products.Add(new Product { Id = 1, Name = "Product 1", Price = 10m, Stock = 5 });
            _fakeDatabase.Products.Add(new Product { Id = 2, Name = "Product 2", Price = 20m, Stock = 10 });
            _fakeDatabase.Products.Add(new Product { Id = 3, Name = "Product 3", Price = 30m, Stock = 15 });

            var result = _repository.GetAll();

            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void GetAll_EmptyDatabase_ReturnsEmptyList()
        {
            var result = _repository.GetAll();

            Assert.Empty(result);
        }

        [Fact]
        public void Update_ExistingProduct_UpdatesProductProperties()
        {
            var originalProduct = new Product
            {
                Id = 1,
                Name = "Original Name",
                Price = 10m,
                Stock = 5
            };
            _fakeDatabase.Products.Add(originalProduct);

            var updatedProduct = new Product
            {
                Id = 1,
                Name = "Updated Name",
                Price = 20m,
                Stock = 10
            };

            _repository.Update(updatedProduct);

            var result = _fakeDatabase.Products.First(p => p.Id == 1);
            Assert.Equal("Updated Name", result.Name);
            Assert.Equal(20m, result.Price);
            Assert.Equal(10, result.Stock);
        }

        [Fact]
        public void Update_NonExistingProduct_DoesNothing()
        {
            var product = new Product
            {
                Id = 999,
                Name = "Does Not Exist",
                Price = 10m,
                Stock = 5
            };

            _repository.Update(product);

            Assert.Empty(_fakeDatabase.Products);
        }

        [Fact]
        public void Remove_ExistingProduct_RemovesProductFromDatabase()
        {
            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                Price = 10m,
                Stock = 5
            };
            _fakeDatabase.Products.Add(product);

            _repository.Remove(1);

            Assert.Empty(_fakeDatabase.Products);
        }

        [Fact]
        public void Remove_NonExistingProduct_DoesNothing()
        {
            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                Price = 10m,
                Stock = 5
            };
            _fakeDatabase.Products.Add(product);

            _repository.Remove(999);

            Assert.Single(_fakeDatabase.Products);
        }

        [Fact]
        public void Remove_MultipleProducts_RemovesOnlySpecifiedProduct()
        {
            _fakeDatabase.Products.Add(new Product { Id = 1, Name = "Product 1", Price = 10m, Stock = 5 });
            _fakeDatabase.Products.Add(new Product { Id = 2, Name = "Product 2", Price = 20m, Stock = 10 });
            _fakeDatabase.Products.Add(new Product { Id = 3, Name = "Product 3", Price = 30m, Stock = 15 });

            _repository.Remove(2);

            Assert.Equal(2, _fakeDatabase.Products.Count);
            Assert.DoesNotContain(_fakeDatabase.Products, p => p.Id == 2);
            Assert.Contains(_fakeDatabase.Products, p => p.Id == 1);
            Assert.Contains(_fakeDatabase.Products, p => p.Id == 3);
        }
    }
}
