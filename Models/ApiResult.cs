namespace ChatApplicationWithSQLServer.Models
{
	public class ApiResult
	{
		public bool Success { get; set; }
		public string? Error { get; set; }
		public object? Data { get; set; }
	}
}
