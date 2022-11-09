using ChatApplicationWithSQLServer.DataModels;
using ChatApplicationWithSQLServer.Interfaces;
using ChatApplicationWithSQLServer.Models;
using Microsoft.Extensions.Configuration;

namespace ChatApplicationWithSQLServer.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly ChatApplicationDBContext _context;
        public RoomRepository(ChatApplicationDBContext context)
        {
            _context = context;
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

        public List<Room> GetRoomList()
        {
            var roomList = new List<Room>();
            try
            {
                roomList = _context.Room.ToList();
                return roomList;
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }

    }
}
