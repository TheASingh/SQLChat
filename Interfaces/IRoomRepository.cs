using ChatApplicationWithSQLServer.DataModels;
using ChatApplicationWithSQLServer.Models;

namespace ChatApplicationWithSQLServer.Interfaces
{
    public interface IRoomRepository
    {
        bool CreateRoom(string roomName);
        List<RoomModel> GetRoomList();
        ApiResult GetUserRoomDetail(int userId);

	}
}
