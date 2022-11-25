namespace ChatApplicationWithSQLServer.Models
{
	public class ChatModel
	{
		public int RoomId { get; set; }
		public int? LoggedInUserId { get; set; }
		public DateTime? LoggedInUserLastSeen { get; set; }
		public List<MessageDetail> Messages { get; set; }
	}

	public class MessageDetail
	{
		public int Id { get; set; }
		public string UserName { get; set; }
		public string? Message { get; set; }
		public DateTime Timestamp { get; set; }
	}

	public class ReceivedMessageModel
	{
		public int RoomId { get; set; }
		public int UserId { get; set; }
		public string? Message { get; set; }
	}
}
