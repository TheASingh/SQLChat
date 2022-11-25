using ChatApplicationWithSQLServer.DataModels;
using ChatApplicationWithSQLServer.Interfaces;
using ChatApplicationWithSQLServer.Models;
using System.Runtime.CompilerServices;

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

		public ChatModel GetMessages(int roomId, int? userId = null)
		{
			var chatModel = new ChatModel();
			var listMessageDetail = new List<MessageDetail>();
			try
			{
				if (userId != null)
				{
					chatModel.LoggedInUserId = userId;
					if (_context.UsersRoom.Any(x => x.UserId == userId && x.RoomId == roomId))
					{
						chatModel.LoggedInUserLastSeen = _context.UsersRoom
														.Where(x => x.UserId == userId && x.RoomId == roomId)
														.Max(z => z.LastSeenDT);
					}
				}
				listMessageDetail = _context.Chat.Where(y => y.RoomId == roomId)
										.Join(_context.User,
												C => C.UserId,
												U => U.UserId,
												(C, U) => new
												{
													Chat = C,
													U.UserName
												}
										)
										.Select(x => new MessageDetail()
										{
											Id = x.Chat.Id,
											UserName = x.UserName,
											Timestamp = x.Chat.Timestamp,
											Message = x.Chat.Message
										}).ToList();

				chatModel.RoomId = roomId;
				chatModel.Messages = listMessageDetail;
			}
			catch (Exception e)
			{

			}
			return chatModel;
		}



	}
}
