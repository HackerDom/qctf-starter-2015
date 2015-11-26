using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using log4net;

namespace BrainFuckTask
{
	public class AsyncListener
	{
		public AsyncListener(int port, string suffix, Func<HttpListenerContext, Task> callback)
		{
			this.callback = callback;
			listener = new HttpListener();
			listener.Prefixes.Add($"http://+:{port}/{(suffix != null ? suffix.Trim('/') + '/' : null)}");
		}

		public async void Loop()
		{
			listener.Start();
			Log.InfoFormat("Listen '{0}'", string.Join(";", listener.Prefixes));
			while(true)
			{
				try
				{
					var context = await listener.GetContextAsync();
					var hash = '#' + context.GetHashCode().ToString("x8");
					Log.InfoFormat("{0} [{1}] {2}", hash, context.Request.RemoteEndPoint, context.Request.Url);
					Task.Run(() => TryProcessRequest(context, Stopwatch.StartNew(), hash));
				}
				catch(Exception e)
				{
					Log.Error(e);
				}
			}
		}

		private async Task TryProcessRequest(HttpListenerContext context, Stopwatch stopwatch, string hash)
		{
			try
			{
				using(var response = context.Response)
				{
					try
					{
						await callback(context).ConfigureAwait(false);
						Log.InfoFormat("{0} {1}ms", hash, stopwatch.ElapsedMilliseconds);
					}
					catch(Exception e)
					{
						var httpListenerException = e as HttpListenerException;
						if(httpListenerException != null && httpListenerException.ErrorCode == ERROR_NETNAME_DELETED)
							return;
						Log.Error(hash, e);
						response.StatusCode = 500;
					}
				}
			}
			catch(Exception e)
			{
				Log.Error(e);
			}
		}

		private const int ERROR_NETNAME_DELETED = 64; //NOTE: The specified network name is no longer available
		private static readonly ILog Log = LogManager.GetLogger(typeof(AsyncListener));
		private readonly Func<HttpListenerContext, Task> callback;
		private readonly HttpListener listener;
	}
}