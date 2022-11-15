namespace ChatApplicationWithSQLServer.Models
{
    public class UserModel
    {
        public int? UserId { get; set; }
        public string UserName { get; set; }
    }

	public class CreateUserModel
	{
		public string Username { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string ProjectBaseUrl { get; set; }
	}

	public class ResetPasswordUserModel
	{
		public string Token { get; set; }
		public string Password { get; set; }
	}
}
