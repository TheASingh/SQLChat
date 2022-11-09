using ChatApplicationWithSQLServer.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace ChatApplicationWithSQLServer.Repositories
{
	public class EchoHub : Hub
	{
		//private ChatApplicationDBContext _chatApplicationDBContext;
		private IChatRepository chatRepository;
		public EchoHub(IChatRepository _chatRepository)
		{
			chatRepository = _chatRepository;
		}

		public void Echo(int roomId)
		{
			var chatDataListToReturn = chatRepository.GetMessages(roomId);
			Clients.All.SendAsync("SendToHomeController", chatDataListToReturn);
		}
	}
}
