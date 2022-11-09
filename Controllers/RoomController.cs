using ChatApplicationWithSQLServer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ChatApplicationWithSQLServer.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class RoomController : Controller
    {
        IRoomRepository roomRepository;
        public RoomController(IRoomRepository _roomRepository)
        {
            roomRepository = _roomRepository;
        }

        [EnableCors]
        [HttpPost("createroom")]
        public IActionResult CreateRoom(string roomname)
        {
            var result = roomRepository.CreateRoom(roomname);
            return Ok(result);
        }

        [EnableCors]
        [HttpGet("getrooms")]
        public IActionResult GetRooms()
        {
            try
            {
                var result = roomRepository.GetRoomList();

                return Ok(result);
            }
            catch(Exception e)
            {
                return BadRequest(e.InnerException);
            }
        }

    }
}
