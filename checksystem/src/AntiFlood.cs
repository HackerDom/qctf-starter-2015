using System;
using System.Collections.Concurrent;
using System.Web;
using main.utils;

namespace main
{
	public static class AntiFlood
	{
		public static void CheckFlood(string key)
		{
			var checkTime = DateTime.UtcNow.TotalSeconds() / CheckIntervalSec;
			Dict.AddOrUpdate(key, new FloodCheck {Time = checkTime, Count = 1}, (l, check) =>
			{
				lock(check)
				{
					if(check.Time == checkTime)
					{
						if(++check.Count > MaxCountInInterval)
							throw new HttpException(403, string.Format("Too fast, wait {0} sec pls", CheckIntervalSec));
					}
					else
					{
						check.Time = checkTime;
						check.Count = 1;
					}
					return check;
				}
			});
		}

		private class FloodCheck
		{
			public long Time;
			public int Count;
		}

		private const int CheckIntervalSec = 10;
		private const int MaxCountInInterval = 5;

		private static readonly ConcurrentDictionary<string, FloodCheck> Dict = new ConcurrentDictionary<string, FloodCheck>(StringComparer.InvariantCultureIgnoreCase);

	}
}