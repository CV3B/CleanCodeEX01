using Inlämningsuppgift_1.Data.Entities;

namespace Inlämningsuppgift_1.Data.Interfaces
{
    public interface IUserRepository
    {
        void Add(User user);
        User? GetById(int id);
        User? GetByUsername(string username);
    }
}