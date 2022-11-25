namespace ChatApplicationWithSQLServer.Models
{
    public class UsersRoomModel
	{
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        //public int UserId { get; set; }
        public DateTime LastSeenDT { get; set; }
        public int MessageCount { get; set; }
    }

	public class UsersRoomUpdateModel
	{
		public int RoomId { get; set; }
		public int UserId { get; set; }
	}
}
