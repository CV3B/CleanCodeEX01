using Inlämningsuppgift_1.Data.Entities;

namespace Inlämningsuppgift_1.Data.Interfaces
{
    public interface IProductRepository
    {
        void Add(Product product);
        List<Product> GetAll();
        Product? GetById(int id);
        void Remove(int id);
        void Update(Product product);
    }
}