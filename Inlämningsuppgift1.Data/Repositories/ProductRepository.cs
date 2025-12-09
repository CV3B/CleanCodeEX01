using Inlämningsuppgift_1.Data.Entities;
using Inlämningsuppgift_1.Data.Interfaces;
using Inlämningsuppgift_1.Data.Persistence;

namespace Inlämningsuppgift_1.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly InMemoryDatabase _db;

        public ProductRepository(InMemoryDatabase db)
        {
            _db = db;
        }

        public List<Product> GetAll() => _db.Products;

        public Product? GetById(int id) => _db.Products.FirstOrDefault(p => p.Id == id);

        public void Add(Product product)
        {
            product.Id = _db.Products.Count > 0 ? _db.Products.Max(p => p.Id) + 1 : 1;
            _db.Products.Add(product);
        }

        public void Update(Product product)
        {
            var existing = GetById(product.Id);
            if (existing == null) return;
            existing.Name = product.Name;
            existing.Price = product.Price;
            existing.Stock = product.Stock;

        }

        public void Remove(int id)
        {
            _db.Products.RemoveAll(p => p.Id == id);
        }
    }
}
