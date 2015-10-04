using System;
using System.Web;
using log4net;
using main.utils;

namespace main
{
	public abstract class BaseHandler : IHttpHandler
	{
		public void ProcessRequest(HttpContext context)
		{
			try
			{
				context.Response.TrySkipIisCustomErrors = true;
				context.Response.ContentType = "text/plain; charset=utf-8";
				context.Response.AppendHeader("Cache-Control", "no-cache");

				var result = ProcessRequestInternal(context);
				context.Response.Write(result.ToJsonString());
			}
			catch(Exception e)
			{
				Log.Error(string.Format("Failed to process request '{0}'", context.Request.RawUrl), e);

				string message = null;
				var httpError = e as HttpException;
				if(httpError == null)
					context.Response.StatusCode = 500;
				else
				{
					context.Response.StatusCode = httpError.GetHttpCode();
					message = e.Message;
				}

				var result = new AjaxResult {Error = message ?? "Unknown server error"};
				context.Response.Write(result.ToJsonString());
			}
		}

		public bool IsReusable { get { return true; } }

		protected abstract AjaxResult ProcessRequestInternal(HttpContext context);

		private static readonly ILog Log = LogManager.GetLogger(typeof(BaseHandler));
	}
}