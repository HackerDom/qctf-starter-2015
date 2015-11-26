using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using log4net.Config;

namespace TaskMorse
{
	static class Program
	{
		static void Main(string[] args)
		{
			XmlConfigurator.Configure();
			try
			{
				if(args.Length == 0)
				{
					Console.WriteLine("prog.exe <port> <path>");
					return;
				}
    			string suffix = null;
                if (args.Length == 1)
                {
                    suffix = args[1];
                }    			
				EncFlag = Morse.Encode(Flag).Select(val => Encoding.ASCII.GetBytes(val.ToString().ToLower())).ToArray();
				new AsyncListener(int.Parse(args[0]), suffix, ProcessSearchRequest).Loop();

				Thread.Sleep(Timeout.Infinite);
			}
			catch(Exception e)
			{
				Log.Fatal(e);
			}
		}

		private static async Task ProcessSearchRequest(HttpListenerContext context)
		{
			var result = EncFlag[(GetTotalMilliseconds() / 250L) % EncFlag.Length];
			context.Response.ContentLength64 = result.Length;
			await context.Response.OutputStream.WriteAsync(result, 0, result.Length);
		}

		private static long GetTotalMilliseconds()
		{
			return DateTime.UtcNow.Ticks / (10 * 1000);
		}

		private const string Flag = "mars0k9dplb730gdae";
		private static readonly ILog Log = LogManager.GetLogger(typeof(Program));
		private static byte[][] EncFlag;
	}
}