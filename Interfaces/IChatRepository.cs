using ChatApplicationWithSQLServer.Models;

namespace ChatApplicationWithSQLServer.Interfaces
{
	public interface IChatRepository
	{
		bool SendMessage(ReceivedMessageModel receivedMessageModel);
		List<ChatModel> GetMessages(int roomId);
	}
}
