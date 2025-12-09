using Inlämningsuppgift_1.Data.Entities;
using Inlämningsuppgift_1.Data.Interfaces;
using Inlämningsuppgift_1.Data.Persistence;

namespace Inlämningsuppgift_1.Data.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly InMemoryDatabase _db;
        public CartRepository(InMemoryDatabase db)
        {
            _db = db;
        }

        public void Add(int userId, List<CartItem> cartItems)
        {
            _db.Carts[userId] = cartItems;
        }

        public List<CartItem> GetByUserId(int userId)
        {
            _db.Carts.TryGetValue(userId, out var cartItems);
            return cartItems ?? [];
        }

        public void Remove(int userId)
        {
            _db.Carts.Remove(userId, out _);
        }

        public void Clear(int userId)
        {
            _db.Carts[userId].Clear();
        }
    }
}
