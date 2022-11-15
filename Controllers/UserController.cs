using ChatApplicationWithSQLServer.Interfaces;
using ChatApplicationWithSQLServer.Models;
using ChatApplicationWithSQLServer.Repositories;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ChatApplicationWithSQLServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        IUserRepository userRepository;
        public UserController(IUserRepository _userRepository)
        {
            userRepository = _userRepository;
        }

        [EnableCors]
        [HttpPost("createuser")]
        public IActionResult CreateUser([FromBody] CreateUserModel createUserModel)
        {
            var result = userRepository.CreateUser(createUserModel.Username, createUserModel.Email, createUserModel.Password, createUserModel.ProjectBaseUrl);
            return Ok(result);
        }

        [EnableCors]
        [HttpGet("login")]
        public IActionResult LogIn(string email, string password)
        {
            var result = userRepository.LogIn(email, password);
            return Ok(result);
        }

		[EnableCors]
		[HttpGet("verifyandapprovetoken")]
		public IActionResult LogIn(string token)
		{
			var result = userRepository.VerifyAndApproveToken(token);
			return Ok(result);
		}

		[EnableCors]
		[HttpGet("sendresetpasswordemail")]
		public IActionResult SendResetPasswordEmail(string email, string projectBaseUrl)
		{
			var result = userRepository.SendEmailToResetPassword(email, projectBaseUrl);
			return Ok(result);
		}

		[EnableCors]
		[HttpPost("resetpassword")]
		public IActionResult ResetPassword([FromBody] ResetPasswordUserModel resetPasswordUserModel)
		{
			var result = userRepository.ResetPassword(resetPasswordUserModel.Token, resetPasswordUserModel.Password);
			return Ok(result);
		}

		[EnableCors]
        [HttpGet("test")]
        public IActionResult Test()
        {
            string value = "tested";
            return Ok(value);
        }
    }
}
