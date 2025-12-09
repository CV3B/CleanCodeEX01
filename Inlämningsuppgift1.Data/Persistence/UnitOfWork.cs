using Inlämningsuppgift_1.Data.Entities;
using Inlämningsuppgift_1.Data.Interfaces;
using Inlämningsuppgift_1.Data.Repositories;

namespace Inlämningsuppgift_1.Data.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly InMemoryDatabase _database;
        private DatabaseSnapshot? _snapshot;
        private bool _transactionActive = false;

        public IProductRepository Products { get; }
        public IOrderRepository Orders { get; }
        public ICartRepository Carts { get; }
        public IUserRepository Users { get; }
        public ITokenRepository Tokens { get; }


        public UnitOfWork(InMemoryDatabase database)
        {
            _database = database;

            Products = new ProductRepository(_database);
            Orders = new OrderRepository(_database);
            Carts = new CartRepository(_database);
            Users = new UserRepository(_database);
            Tokens = new TokenRepository(_database);
        }

        public void BeginTransaction()
        {
            if (_transactionActive)
                throw new InvalidOperationException("Transaction already active");

            _snapshot = new DatabaseSnapshot(_database);
            _transactionActive = true;
        }

        public void Commit()
        {
            if (!_transactionActive)
                throw new InvalidOperationException("No active transaction");

            _snapshot = null;
            _transactionActive = false;
        }

        public void Rollback()
        {
            if (!_transactionActive)
                throw new InvalidOperationException("No active transaction");

            if (_snapshot == null)
                throw new InvalidOperationException("No snapshot available");

            _snapshot.Restore(_database);

            _snapshot = null;
            _transactionActive = false;
        }

        public void Dispose()
        {
            if (_transactionActive)
            {
                Rollback();
            }
        }
    }

    internal class DatabaseSnapshot
    {
        private readonly List<Product> _products;
        private readonly List<Order> _orders;
        private readonly List<User> _users;
        private readonly Dictionary<int, List<CartItem>> _carts;
        private readonly Dictionary<string, int> _tokens;

        public DatabaseSnapshot(InMemoryDatabase db)
        {
            _products = db.Products.Select(p => new Product
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Stock = p.Stock
            }).ToList();

            _orders = db.Orders.Select(o => new Order
            {
                Id = o.Id,
                UserId = o.UserId,
                Total = o.Total,
                CreatedAt = o.CreatedAt,
                OrderItems = o.OrderItems.ToList()
            }).ToList();

            _users = db.Users.Select(u => new User
            {
                Id = u.Id,
                Username = u.Username,
                Password = u.Password,
                Email = u.Email
            }).ToList();

            _carts = db.Carts.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Select(ci => new CartItem
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity
                }).ToList()
            );

            _tokens = db.Tokens.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        public void Restore(InMemoryDatabase db)
        {
            db.Products.Clear();
            db.Products.AddRange(_products);

            db.Orders.Clear();
            db.Orders.AddRange(_orders);

            db.Users.Clear();
            db.Users.AddRange(_users);

            db.Carts.Clear();
            foreach (var cart in _carts)
            {
                db.Carts[cart.Key] = cart.Value;
            }

            db.Tokens.Clear();
            foreach (var token in _tokens)
            {
                db.Tokens[token.Key] = token.Value;
            }
        }
    }
}
