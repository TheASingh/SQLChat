using ChatApplicationWithSQLServer.Interfaces;
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
        public IActionResult CreateUser(string username, string email, string password)
        {
            var result = userRepository.CreateUser(username, email, password);
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
        [HttpGet("test")]
        public IActionResult Test()
        {
            string value = "tested";
            return Ok(value);
        }
    }
}
