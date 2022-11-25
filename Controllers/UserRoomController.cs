using ChatApplicationWithSQLServer.Interfaces;
using ChatApplicationWithSQLServer.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ChatApplicationWithSQLServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserRoomController : Controller
    {
        IUserRoomRepository userRoomRepository;
        public UserRoomController(IUserRoomRepository _userRoomRepository)
        {
			userRoomRepository = _userRoomRepository;
        }

        [EnableCors]
        [HttpPost("updatelastseendt")]
        public IActionResult UpdateLastSeenDT([FromBody] UsersRoomUpdateModel usersRoomUpdateModel)
        {
            var result = userRoomRepository.UpdateLastSeenDT(usersRoomUpdateModel.UserId, usersRoomUpdateModel.RoomId);
            return Ok(result);
        }

    }
}
