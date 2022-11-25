using ChatApplicationWithSQLServer.DataModels;
using ChatApplicationWithSQLServer.Interfaces;
using ChatApplicationWithSQLServer.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;
using static System.Net.WebRequestMethods;

namespace ChatApplicationWithSQLServer.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly ChatApplicationDBContext _context;
		ApiResult apiResult;
		public UserRepository(ChatApplicationDBContext context)
		{
			_context = context;
			apiResult = new ApiResult()
			{
				Success = false,
				Error = "Some error has occured, please try later"
			};

		}
		public ApiResult CreateUser(string username, string email, string password, string projectBaseUrl)
		{
			try
			{
				if (_context.User.Any(x => x.UserName.ToLower().Trim() == username.ToLower().Trim()))
				{
					apiResult.Error = "This username already exists.";
					return apiResult;
				}

				else if (_context.User.Any(x => x.Email.ToLower().Trim() == email.ToLower().Trim()))
				{
					apiResult.Error = "An account already exists for this email address.";
					return apiResult;
				}

				var uniqueId = Guid.NewGuid().ToString("N");
				_context.User.Add(new User()
				{
					Email = email,
					UserName = username,
					Password = password,
					IsActive = false,
					ActivationToken = uniqueId
				}
				);
				_context.SaveChanges();

				SendEmailToVerifyEmail(username, uniqueId, email, projectBaseUrl);
				apiResult.Success = true;
				return apiResult;
			}
			catch (Exception e)
			{
				return apiResult;
			}
		}

		public ApiResult LogIn(string email, string password)
		{
			UserModel userModel = new UserModel();
			try
			{
				var loggedInUser = _context.User.Where(x => x.Email.ToLower().Trim().Equals(email.ToLower().Trim())
										&& x.Password.Equals(password)).FirstOrDefault();

				if (loggedInUser != null && loggedInUser.UserId > 0)
				{
					if (loggedInUser.IsActive != null && loggedInUser.IsActive != false)
					{
						userModel.UserId = loggedInUser.UserId;
						userModel.UserName = loggedInUser.UserName;

						apiResult = new ApiResult()
						{
							Success = true,
							Data = userModel
						};
					}
					else
					{
						apiResult = new ApiResult()
						{
							Success = false,
							Error = "Your account is not verified yet, please verify it by clicking on the link sent to you on your email."
						};
					}

				}
				else
				{
					apiResult = new ApiResult()
					{
						Success = false,
						Error = "Invalid Username/password"
					};
				}
				return apiResult;
			}
			catch (Exception e)
			{
				return apiResult;
			}
		}

		public ApiResult VerifyAndApproveToken(string token)
		{
			try
			{
				if (_context.User.Any(x => x.ActivationToken.Trim().Equals(token.Trim())))
				{
					var user = _context.User.Where(x => x.ActivationToken.Trim().Equals(token.Trim())).FirstOrDefault();

					if (user != null)
					{
						user.IsActive = true;
						_context.User.Update(user);
						_context.SaveChanges();

						apiResult.Success = true;
					}
				}
				else
				{
					apiResult.Success = false;
					apiResult.Error = "Invalid token";
				}
				return apiResult;
			}
			catch (Exception e)
			{
				return apiResult;
			}
		}

		public ApiResult ResetPassword(string token, string password)
		{
			try
			{
				if (_context.User.Any(x => x.ActivationToken.Trim().Equals(token.Trim())))
				{
					var user = _context.User.Where(x => x.ActivationToken.Trim().Equals(token.Trim())).FirstOrDefault();

					if (user != null)
					{
						user.IsActive = true;
						user.Password = password;
						_context.User.Update(user);
						_context.SaveChanges();

						apiResult.Success = true;
					}
				}
				else
				{
					apiResult.Success = false;
					apiResult.Error = "Invalid token";
				}
				return apiResult;
			}
			catch (Exception e)
			{
				return apiResult;
			}
		}

		public void SendEmailToVerifyEmail(string username, string token, string toEmailAddress, string projectBaseUrl)
		{
			try
			{
				string urlToVerifyEmail = projectBaseUrl + "verifyemail/" + token;
				string body = "<div>Hello " + username + " <br>" +
				"Please click on the below link to verify your account.<br><br>" +
				"<a href=" + urlToVerifyEmail + ">Verify your account</a>" +
				"</div>";
				MailMessage message = new MailMessage();
				SmtpClient smtp = new SmtpClient("smtp.gmail.com");

				message.From = new MailAddress("18instantdebate@gmail.com");
				message.To.Add(new MailAddress(toEmailAddress));
				message.Subject = "Verify your email for The Chat Application";
				message.IsBodyHtml = true; //to make message body as html  
				message.Body = body;
				smtp.Port = 587;
				//smtp.Host = "smtp.gmail.com"; //for gmail host  
				smtp.EnableSsl = true;
				smtp.UseDefaultCredentials = false;
				smtp.Credentials = new NetworkCredential("18instantdebate@gmail.com", "uqgizcemkxkhvoap");
				smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
				smtp.Send(message);
			}
			catch (Exception e)
			{
				var ex = e;
			}
		}

		public ApiResult SendEmailToResetPassword(string toEmailAddress, string projectBaseUrl)
		{
			try
			{
				string token = string.Empty;
				if (_context.User.Any(x => x.Email.ToLower().Trim() == toEmailAddress.ToLower().Trim()))
				{
					var user = _context.User.Where(x => x.Email.ToLower().Trim() == toEmailAddress.ToLower().Trim()).FirstOrDefault();
					token = user.ActivationToken;

					string urlToVerifyEmail = projectBaseUrl + "resetpassword/" + token;
					string body = "<div>Hello " + user.UserName + " <br>" +
					"Please click on the below link to reset the password of your account.<br><br>" +
					"<a href=" + urlToVerifyEmail + ">Reset password</a>" +
					"</div>";
					MailMessage message = new MailMessage();
					SmtpClient smtp = new SmtpClient("smtp.gmail.com");

					message.From = new MailAddress("18instantdebate@gmail.com");
					message.To.Add(new MailAddress(toEmailAddress));
					message.Subject = "Reset password for your account on The Chat Application";
					message.IsBodyHtml = true; //to make message body as html  
					message.Body = body;
					smtp.Port = 587;
					//smtp.Host = "smtp.gmail.com"; //for gmail host  
					smtp.EnableSsl = true;
					smtp.UseDefaultCredentials = false;
					smtp.Credentials = new NetworkCredential("18instantdebate@gmail.com", "uqgizcemkxkhvoap");
					smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
					smtp.Send(message);

					apiResult.Success = true;
				}
				else{
					apiResult.Success = false;
					apiResult.Error = "No account exist with email " + toEmailAddress;
				}

				
			}
			catch (Exception e)
			{
				var ex = e;
				apiResult.Success = false;
				apiResult.Error = "Invalid email";
			}

			return apiResult;
		}
	}
}
