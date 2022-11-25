using ChatApplicationWithSQLServer.Interfaces;
using ChatApplicationWithSQLServer.Models;
using ChatApplicationWithSQLServer.Repositories;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ChatApplicationWithSQLServer.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ChatController : Controller
	{
		IChatRepository chatRepository;
		public ChatController(IChatRepository _chatRepository)
		{
			chatRepository = _chatRepository;
		}

		[EnableCors]
		[HttpPost("sendmessage")]
		public IActionResult SendMessage([FromBody] ReceivedMessageModel receivedMessageModel)
		{
			var result = chatRepository.SendMessage(receivedMessageModel);
			return Ok(result);
		}


		[EnableCors]
		[HttpGet("getmessages")]
		public IActionResult GetMessages(int roomId, int userId)
		{
			var result = chatRepository.GetMessages(roomId, userId);
			return Ok(result);
		}



	}
}
