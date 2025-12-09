namespace Inlämningsuppgift_1.Data.Interfaces
{
    public interface IUnitOfWork
    {
        ICartRepository Carts { get; }
        IOrderRepository Orders { get; }
        IProductRepository Products { get; }
        IUserRepository Users { get; }
        ITokenRepository Tokens { get; }

        void BeginTransaction();
        void Commit();
        void Dispose();
        void Rollback();
    }
}