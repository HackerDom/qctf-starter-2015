using System;
using System.Configuration;
using System.Linq;
using System.Threading;
using log4net;
using main.chat;
using main.utils;

namespace main
{
	public static class Settings
	{
		static Settings()
		{
			TryUpdate();
			UpdateThread = new Thread(() =>
			{
				TryUpdate();
				Thread.Sleep(30000);
			}) {IsBackground = true};
			UpdateThread.Start();
		}

		public static ConnectionStringSettings ConnectionString { get; private set; }

		public static byte[] HmacKey { get; private set; }

		public static int MaxLoginLength { get; private set; }
		public static int MaxPassLength { get; private set; }
		public static int MaxMsgLength { get; private set; }

		public static string FlagPrefix { get; private set; }
		public static string MainBombFlag { get; private set; }
		public static DateTime BombTimerEnd { get; private set; }
		public static char[] MartianChars { get; private set; }
		public static string BombTasksSuffix { get; private set; }
		public static string MainBombTaskSuffix { get; private set; }
		public static string BombShuttedDownSuffix { get; private set; }
		public static int BombShutDownFlagsCount { get; private set; }
		public static string EndGameSuffix { get; private set; }

		private static void TryUpdate()
		{
			try
			{
				ConnectionString = ConfigurationManager.ConnectionStrings["main"];

				HmacKey = Convert.FromBase64String(ConfigurationManager.AppSettings["HmacKey"]);

				MaxLoginLength = int.Parse(ConfigurationManager.AppSettings["MaxLoginLength"]);
				MaxPassLength = int.Parse(ConfigurationManager.AppSettings["MaxPassLength"]);
				MaxMsgLength = int.Parse(ConfigurationManager.AppSettings["MaxMsgLength"]);

				FlagPrefix = ConfigurationManager.AppSettings["FlagPrefix"].TrimToNull().SanitizeFlag();
				MainBombFlag = ConfigurationManager.AppSettings["MainBombFlag"].TrimToNull().SanitizeFlag();
				BombTimerEnd = DateTimeUtils.ParseSortable(ConfigurationManager.AppSettings["BombTimerEnd"]);
				MartianChars = ConfigurationManager.AppSettings["MartianChars"].Distinct().OrderBy(c => c).ToArray();
				BombTasksSuffix = " " + ConfigurationManager.AppSettings["BombTasksSuffix"].Trim().Replace("\\n", "\n");
				MainBombTaskSuffix = " " + ConfigurationManager.AppSettings["MainBombTaskSuffix"].Trim().Replace("\\n", "\n");
				BombShuttedDownSuffix = " " + ConfigurationManager.AppSettings["BombShuttedDownSuffix"].Trim().Replace("\\n", "\n");
				BombShutDownFlagsCount = int.Parse(ConfigurationManager.AppSettings["BombShutDownFlagsCount"].TrimToNull());
				EndGameSuffix = " " + ConfigurationManager.AppSettings["EndGameSuffix"].Trim().Replace("\\n", "\n");
			}
			catch(Exception e)
			{
				Log.Error("Failed to update settings", e);
			}
		}

		private static readonly ILog Log = LogManager.GetLogger(typeof(Settings));
		private static readonly Thread UpdateThread;
	}
}