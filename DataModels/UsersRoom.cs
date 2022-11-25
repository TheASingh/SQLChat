namespace ChatApplicationWithSQLServer.DataModels
{
    public class UsersRoom
	{
        public int IndexId { get; set; }
        public int RoomId { get; set; }
        public int UserId { get; set; }
        public DateTime LastSeenDT { get; set; }
    }
}
