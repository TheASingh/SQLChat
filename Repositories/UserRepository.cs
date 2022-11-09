using ChatApplicationWithSQLServer.DataModels;
using ChatApplicationWithSQLServer.Interfaces;
using ChatApplicationWithSQLServer.Models;
using Microsoft.Extensions.Configuration;

namespace ChatApplicationWithSQLServer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ChatApplicationDBContext _context;
        public UserRepository(ChatApplicationDBContext context)
        {
            _context = context;
        }
        public bool CreateUser(string username, string email, string password)
        {
            try
            {
                _context.User.Add(new User()
                {
                    Email = email,
                    UserName = username,
                    Password = password
                }
                );
                _context.SaveChanges();
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public UserModel LogIn(string email, string password)
        {
            UserModel userModel = new UserModel();
            try
            {
                var loggedInUser = _context.User.Where(x => x.Email.ToLower().Trim().Equals(email.ToLower().Trim())
                                        && x.Password.Equals(password)).FirstOrDefault();

                if(loggedInUser != null)
                {
                    userModel.UserId = loggedInUser.UserId;
                    userModel.UserName = loggedInUser.UserName;
                }

                return userModel;
            }
            catch (Exception e)
            {
                return userModel;
            }
        }
    }
}
