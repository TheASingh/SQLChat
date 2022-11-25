using ChatApplicationWithSQLServer.DataModels;
using ChatApplicationWithSQLServer.Interfaces;
using ChatApplicationWithSQLServer.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;
using static System.Net.WebRequestMethods;
using Microsoft.Net.Http.Headers;

namespace ChatApplicationWithSQLServer.Repositories
{
	public class UserRoomRepository : IUserRoomRepository
	{
		private readonly ChatApplicationDBContext _context;
		ApiResult apiResult;
		public UserRoomRepository(ChatApplicationDBContext context)
		{
			_context = context;
			apiResult = new ApiResult()
			{
				Success = false,
				Error = "Some error has occured, please try later"
			};

		}
		public ApiResult UpdateLastSeenDT(int userId, int roomId)
		{
			try
			{
				if (_context.UsersRoom.Any(x => x.UserId == userId && x.RoomId == roomId))
				{
					var recordToUpdate = _context.UsersRoom.Where(x => x.UserId == userId && x.RoomId == roomId).FirstOrDefault();
					recordToUpdate.LastSeenDT = DateTime.Now;

					_context.UsersRoom.Update(recordToUpdate);
				}
				else
				{
					var recordToAdd = new UsersRoom()
					{
						UserId = userId,
						RoomId = roomId,
						LastSeenDT = DateTime.Now
					};
					_context.UsersRoom.Add(recordToAdd);
				}

				_context.SaveChanges();
				apiResult.Success = true;
				apiResult.Error = string.Empty;
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
