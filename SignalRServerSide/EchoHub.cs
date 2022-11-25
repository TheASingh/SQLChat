using ChatApplicationWithSQLServer.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace ChatApplicationWithSQLServer.Repositories
{
	public class EchoHub : Hub
	{
		//private ChatApplicationDBContext _chatApplicationDBContext;
		private IChatRepository chatRepository;
		private IRoomRepository roomRepository;
		public EchoHub(IChatRepository _chatRepository, IRoomRepository _roomRepository)
		{
			chatRepository = _chatRepository;
			roomRepository = _roomRepository;
		}

		public void Echo(int roomId)
		{
			var chatDataListToReturn = chatRepository.GetMessages(roomId);
			Clients.All.SendAsync("SendToHomeController", chatDataListToReturn);


			// updating room-badges
			// RoomList method requires userId to fetch data for a particular user.
			// so just triggering the method in front-end
			Clients.All.SendAsync("SendToRoomListComponent", true);
		}
	}
}
