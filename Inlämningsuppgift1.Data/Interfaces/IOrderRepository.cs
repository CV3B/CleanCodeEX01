using Inlämningsuppgift_1.Data.Entities;

namespace Inlämningsuppgift_1.Data.Interfaces
{
    public interface IOrderRepository
    {
        void Add(Order order);
        Order? GetByOrderId(int orderId);
        List<Order>? GetByUserId(int userId);
    }
}