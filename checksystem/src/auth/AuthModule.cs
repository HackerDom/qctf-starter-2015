using System;
using System.Web;
using log4net;
using main.utils;

namespace main.auth
{
	public class AuthModule : IHttpModule
	{
		public void Dispose()
		{
		}

		public void Init(HttpApplication context)
		{
			context.PreRequestHandlerExecute += OnPreRequest;
		}

		private static void OnPreRequest(object sender, EventArgs e)
		{
			var app = (HttpApplication)sender;
			var context = app.Context;

			if(context.CurrentHandler == null)
				return;

			var tokenCookie = context.Request.Cookies[TokenParamName];
			var token = tokenCookie?.Value;

			string login = null;
			if(token != null)
			{
				login = Token.TryDeserialize(token, Settings.HmacKey);
				context.Items.Add(LoginParamName, login);
			}

			if(token == null && !(context.CurrentHandler is Login || context.CurrentHandler is Register || context.CurrentHandler is Scores || context.CurrentHandler is BaseHandler))
				context.Response.Redirect("/login", true);

			Log.InfoFormat("{0,-4} '{1}', form '{2}', ua '{3}'", context.Request.HttpMethod.SafeToLog(), context.Request.Unvalidated.RawUrl.SafeToLog(), context.Request.Unvalidated.Form.ToString().SafeToLog(), context.Request.UserAgent.SafeToLog());

			if(context.CurrentHandler is System.Web.UI.Page)
				AntiFlood.CheckFlood($"{context.Request.CurrentExecutionFilePath}:{login ?? context.Request.UserHostAddress}", login != null ? 10 : 50);
		}

		public static void SetAuthLoginCookie(string login)
		{
			var token = Token.Serialize(login, Settings.HmacKey);
			var cookie = new HttpCookie(TokenParamName, token) {HttpOnly = true, Expires = DateTime.UtcNow.AddDays(7)};
			HttpContext.Current.Response.SetCookie(cookie);
		}

		public static string GetAuthLogin()
		{
			var login = HttpContext.Current.Items[LoginParamName] as string;
			if(login == null)
				throw new HttpException(403, "Access denied");
			return login;
		}

		private static readonly ILog Log = LogManager.GetLogger(typeof(AuthModule));
		private const string LoginParamName = "login";
		private const string TokenParamName = "token";
	}
}