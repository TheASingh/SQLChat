﻿namespace ChatApplicationWithSQLServer.DataModels
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool? IsActive { get; set; }
        public string? ActivationToken { get; set; }
    }
}
