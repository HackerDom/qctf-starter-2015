using System;
using System.Net;
using System.Web;
using log4net;

namespace main
{
	public class Global : HttpApplication
	{
		static Global()
		{
			Log = LogManager.GetLogger(typeof(Global));
		}

		protected void Application_Start(object sender, EventArgs e)
		{
		}

		protected void Application_BeginRequest(object sender, EventArgs e)
		{
			Context.Items["context"] = Context.GetHashCode().ToString("x8");
			Response.AddHeader("X-Frame-Options", "SAMEORIGIN");
			Response.AddHeader("X-XSS-Protection", "1; mode=block");
			Response.AddHeader("X-Content-Type-Options", "nosniff");
		}

		protected void Application_Error(object sender, EventArgs e)
		{
			try
			{
				if(Context.CurrentHandler is BaseHandler) //NOTE: Exception will be catched by Handler
					return;

				var error = Server.GetLastError();
				Log.Error(error);

				try
				{
					Response.ClearContent();
					var httpError = error as HttpException;
					Response.StatusCode = httpError == null ? (int)HttpStatusCode.InternalServerError : httpError.GetHttpCode();
					Response.Cache.SetCacheability(HttpCacheability.NoCache);
				}
				catch {}
			}
			catch(Exception exception)
			{
				Log.Error(exception);
			}
			finally
			{
				Server.ClearError();
			}
		}

		private static readonly ILog Log;
	}
}