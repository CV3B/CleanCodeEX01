using Inlämningsuppgift_1.Data.Entities;
using Inlämningsuppgift_1.Data.Interfaces;
using Inlämningsuppgift_1.Data.Persistence;

namespace Inlämningsuppgift_1.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly InMemoryDatabase _db;

        public UserRepository(InMemoryDatabase db)
        {
            _db = db;
        }

        public void Add(User user)
        {
            user.Id = _db.GetNextUserId();
            _db.Users.Add(user);
        }

        public User? GetById(int id)
        {
            return _db.Users.FirstOrDefault(u => u.Id == id);
        }

        public User? GetByUsername(string username)
        {
            return _db.Users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

    }
}
