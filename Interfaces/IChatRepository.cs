using ChatApplicationWithSQLServer.Models;

namespace ChatApplicationWithSQLServer.Interfaces
{
	public interface IChatRepository
	{
		bool SendMessage(ReceivedMessageModel receivedMessageModel);
		ChatModel GetMessages(int roomId, int? userId = null);
	}
}
