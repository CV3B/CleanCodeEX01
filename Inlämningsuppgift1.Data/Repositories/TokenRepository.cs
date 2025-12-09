using Inlämningsuppgift_1.Data.Interfaces;
using Inlämningsuppgift_1.Data.Persistence;

namespace Inlämningsuppgift_1.Data.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly InMemoryDatabase _db;

        public TokenRepository(InMemoryDatabase db)
        {
            _db = db;
        }

        public void Add(int userId, string token)
        {
            _db.Tokens[token] = userId;
        }

        public void Remove(string token)
        {
            _db.Tokens.Remove(token);
        }

        public int? GetUserIdByToken(string token)
        {
            if (_db.Tokens.TryGetValue(token, out var userId))
            {
                return userId;
            }
            return null;
        }
    }
}
