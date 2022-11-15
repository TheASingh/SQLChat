using ChatApplicationWithSQLServer.Models;

namespace ChatApplicationWithSQLServer.Interfaces
{
    public interface IUserRepository
    {
		ApiResult CreateUser(string username, string email, string password, string projectBaseUrl);

		ApiResult LogIn(string email, string password);

		ApiResult VerifyAndApproveToken(string token);

		ApiResult SendEmailToResetPassword(string toEmailAddress, string projectBaseUrl);

		ApiResult ResetPassword(string token, string password);

	}
}
