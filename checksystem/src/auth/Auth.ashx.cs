using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using main.db;
using main.utils;

namespace main.auth
{
	public class Auth : BaseHandler
	{
		protected override AjaxResult ProcessRequestInternal(HttpContext context)
		{
			User user;
			if(context.Request.QueryString["signup"] != null)
			{
				var login = context.Request.Form["login"].TrimToNull();
				if(login == null)
					throw new HttpException(400, "Login is empty");
				if(login.Length < 4)
					throw new HttpException(400, "Login too short");
				if(login.Length > Settings.MaxLoginLength)
					throw new HttpException(400, "Login too long");

				try
				{
					user = new User {Login = login, Pass = RandomPass(), Avatar = RandomAvatar()};
					DbStorage.AddUser(user);
				}
				catch(Exception)
				{
					throw new HttpException(400, "User already exists? Try another login");
				}
			}
			else
			{
				var pass = context.Request.Form["pass"].TrimToNull();
				if(pass == null)
					throw new HttpException(403, "Access denied");

				user = DbStorage.FindUserByPass(pass);
				if(user == null)
					throw new HttpException(403, "Access denied");

				if(user.StartTime < DateTime.UtcNow)
					throw new HttpException(403, $"Start at '{user.StartTime.ToReadable()}'");
			}

			AuthModule.SetAuthLoginCookie(user.Login.Trim());

			return new AjaxResult {Text = user.Pass};
		}

		private string RandomPass()
		{
			PasswordBuffer = PasswordBuffer ?? new byte[PasswordLength];
			Rng = Rng ?? new RNGCryptoServiceProvider();
			Rng.GetBytes(PasswordBuffer);
			for(int i = 0; i < PasswordBuffer.Length; i++)
				PasswordBuffer[i] = Alphabet[PasswordBuffer[i] % Alphabet.Length];
			return Encoding.ASCII.GetString(PasswordBuffer);
		}

		private string RandomAvatar()
		{
			AvatarBuffer = AvatarBuffer ?? new byte[sizeof(int)];
			Rng = Rng ?? new RNGCryptoServiceProvider();
			Rng.GetBytes(AvatarBuffer);
			return Avatars[Math.Abs(BitConverter.ToInt32(AvatarBuffer, 0)) % Avatars.Length];
		}

		private const int PasswordLength = 10;
		private static readonly byte[] Alphabet = Encoding.ASCII.GetBytes("0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ");
		private static readonly string[] Avatars = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "./static/img/avatars")).Select(Path.GetFileName).ToArray();
		[ThreadStatic] private static RandomNumberGenerator Rng;
		[ThreadStatic] private static byte[] PasswordBuffer;
		[ThreadStatic] private static byte[] AvatarBuffer;
	}
}