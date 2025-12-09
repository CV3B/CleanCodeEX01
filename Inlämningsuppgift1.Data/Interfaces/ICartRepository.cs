using Inlämningsuppgift_1.Data.Entities;

namespace Inlämningsuppgift_1.Data.Interfaces
{
    public interface ICartRepository
    {
        void Add(int userId, List<CartItem> cartItems);
        void Clear(int userId);
        List<CartItem> GetByUserId(int userId);
        void Remove(int userId);
    }
}