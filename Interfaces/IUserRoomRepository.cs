using ChatApplicationWithSQLServer.Models;

namespace ChatApplicationWithSQLServer.Interfaces
{
    public interface IUserRoomRepository
	{
		ApiResult UpdateLastSeenDT(int userId, int roomId);
	}
}
