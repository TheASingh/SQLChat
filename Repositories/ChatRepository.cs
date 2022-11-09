using ChatApplicationWithSQLServer.DataModels;
using ChatApplicationWithSQLServer.Interfaces;
using ChatApplicationWithSQLServer.Models;

namespace ChatApplicationWithSQLServer.Repositories
{
	public class ChatRepository : IChatRepository
	{
		private readonly ChatApplicationDBContext _context;
		public ChatRepository(ChatApplicationDBContext context)
		{
			_context = context;
		}

		public bool SendMessage(ReceivedMessageModel receivedMessageModel)
		{
			try
			{
				_context.Chat.Add(new Chat()
				{
					RoomId = receivedMessageModel.RoomId,
					UserId = receivedMessageModel.UserId,
					Message = receivedMessageModel.Message,
					Timestamp = DateTime.Now
				}); ;
				_context.SaveChanges();
				return true;
			}
			catch (Exception e)
			{
				return false;
			}
		}

		public List<ChatModel> GetMessages(int roomId)
		{
			var chatMessages = new List<ChatModel>();
			try
			{

				chatMessages = _context.Chat.Where(y => y.RoomId == roomId)
										.Join(_context.User,
												C => C.UserId,
												U => U.UserId,
												(C, U) => new
												{
													Chat = C,
													U.UserName
												}
										)
										.Select(x => new ChatModel()
										{
											Id = x.Chat.Id,
											RoomId = x.Chat.RoomId,
											UserName = x.UserName,
											Timestamp = x.Chat.Timestamp,
											Message = x.Chat.Message
										}).ToList();
			}
			catch (Exception e)
			{

			}
			return chatMessages;
		}



	}
}
