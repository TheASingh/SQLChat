using ChatApplicationWithSQLServer.Models;

namespace ChatApplicationWithSQLServer.Interfaces
{
    public interface IUserRepository
    {
        bool CreateUser(string username, string email, string password);
        UserModel LogIn(string email, string password);
    }
}
