namespace ChatApplicationWithSQLServer.DataModels
{
    public class Chat
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int UserId { get; set; }
        public string? Message { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
