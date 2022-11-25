using ChatApplicationWithSQLServer.DataModels;
using ChatApplicationWithSQLServer.Interfaces;
using ChatApplicationWithSQLServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace ChatApplicationWithSQLServer.Repositories
{
	public class RoomRepository : IRoomRepository
	{
		private readonly ChatApplicationDBContext _context;
		ApiResult apiResult;
		public RoomRepository(ChatApplicationDBContext context)
		{
			_context = context;
			apiResult = new ApiResult()
			{
				Success = false,
				Error = "Some error has occured, please try later"
			};
		}

		public bool CreateRoom(string roomName)
		{
			try
			{
				_context.Room.Add(new Room()
				{
					RoomName = roomName
				});
				_context.SaveChanges();
				return true;
			}
			catch (Exception e)
			{
				return false;
			}
		}

		public List<RoomModel> GetRoomList()
		{
			var roomList = new List<RoomModel>();
			try
			{
				roomList = _context.Room.Select(x => new RoomModel()
				{
					RoomId = x.RoomId,
					RoomName = x.RoomName
				}).ToList();
				return roomList;
			}
			catch (Exception e)
			{
				throw e;
			}

		}

		public ApiResult GetUserRoomDetail(int userId)
		{
			try
			{
				var roomDetail = _context.Room.Select(z => z).ToList();
				var chatDetail = _context.Chat.Select(x => x).ToList();
				var userDetailData = _context.UsersRoom.Where(x => x.UserId == userId).Select(x => x).ToList();

				List<UsersRoomModel> listUsersRoomModels = new List<UsersRoomModel>(); ;
				UsersRoomModel usersRoomModel;

				DateTime lastSeenDT = new DateTime(DateTime.Now.Year, 1, 1);
				DateTime lastMessageSentDT = new DateTime(DateTime.Now.Year, 1, 1);
				DateTime biggerDT = new DateTime(DateTime.Now.Year, 1, 1);

				foreach (var room in roomDetail)
				{
					var userDetailByRoomId = userDetailData.Where(x => x.RoomId == room.RoomId);
					if (userDetailByRoomId != null && userDetailByRoomId.Count() > 0)
					{
						lastSeenDT = userDetailByRoomId.Max(x => x.LastSeenDT);
					}
					if (_context.Chat.Any(x => x.UserId == userId))
					{
						lastMessageSentDT = _context.Chat.Where(x => x.UserId == userId).Max(y => y.Timestamp);
					}
					biggerDT = lastSeenDT > lastMessageSentDT ? lastSeenDT : lastMessageSentDT;

					
					usersRoomModel = new UsersRoomModel()
					{
						RoomId = room.RoomId,
						RoomName = room.RoomName,
						MessageCount = chatDetail.Any(x => x.RoomId == room.RoomId && x.Timestamp > biggerDT) ? chatDetail.Where(x => x.RoomId == room.RoomId && x.Timestamp > biggerDT).Count() : 0,
						LastSeenDT = lastSeenDT
					};
					listUsersRoomModels.Add(usersRoomModel);
				}

				//var data = userDetailData.Where()

				//var userDetailData = _context.UsersRoom
				//.GroupJoin(_context.Chat,
				//UR => UR.RoomId,
				//C => C.RoomId,
				//(UR, C) => new
				//{
				//	UserDetail = UR,
				//	ChatDetail = C
				//}).
				//Select(z => new UsersRoomModel()
				//{
				//	RoomId = z.UserDetail.RoomId,
				//	LastSeenDT = z.UserDetail.LastSeenDT,
				//	MessageCount = 0

				//}).ToList();


				//var userData = _context.UsersRoom.Where(x => x.UserId == userId)
				//			.Join(_context.User.Where(x => x.UserId == userId),
				//			UR => UR.UserId,
				//			U => U.UserId,
				//			(UR, U) => new
				//			{
				//				UserRoomDetail = UR
				//			})
				//			.Join(_context.Chat.Where(x => x.UserId == userId),
				//			UR => UR.UserRoomDetail.RoomId,
				//			C => C.RoomId,
				//			(UR, C) => new
				//			{
				//				UserRoomDetail = UR.UserRoomDetail,
				//				ChatDetail = C
				//			})
				//			.Select(y => new UsersRoomModel()
				//			{
				//				RoomId = y.UserRoomDetail.RoomId,
				//				UserId = y.UserRoomDetail.UserId,
				//				LastSeenDT = y.UserRoomDetail.LastSeenDT,
				//				MessageCount = 0
				//			}).ToList();


				apiResult.Success = true;
				apiResult.Error = string.Empty;
				apiResult.Data = listUsersRoomModels;
				return apiResult;
			}
			catch (Exception e)
			{
				apiResult.Success = false;
				apiResult.Error = e.Message;
				return apiResult;
			}
		}

	}
}
