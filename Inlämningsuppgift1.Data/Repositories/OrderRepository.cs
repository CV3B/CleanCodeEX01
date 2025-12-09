using Inlämningsuppgift_1.Data.Entities;
using Inlämningsuppgift_1.Data.Interfaces;
using Inlämningsuppgift_1.Data.Persistence;

namespace Inlämningsuppgift_1.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly InMemoryDatabase _db;

        public OrderRepository(InMemoryDatabase db)
        {
            _db = db;
        }

        public void Add(Order order)
        {
            order.Id = _db.Orders.Count > 0 ? _db.Orders.Max(o => o.Id) + 1 : 1;
            _db.Orders.Add(order);
        }

        public Order? GetByOrderId(int orderId)
        {
            return _db.Orders.FirstOrDefault(o => o.Id == orderId);
        }

        public List<Order>? GetByUserId(int userId)
        {
            return _db.Orders.Where(o => o.UserId == userId).ToList();
        }


    }
}
