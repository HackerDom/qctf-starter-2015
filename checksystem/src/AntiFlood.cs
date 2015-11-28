using System;
using System.Collections.Concurrent;
using System.Web;
using main.utils;

namespace main
{
	public static class AntiFlood
	{
		public static void CheckFlood(string key, int count = MaxCountInInterval)
		{
			var checkTime = DateTime.UtcNow.TotalSeconds() / CheckIntervalSec;
			Dict.AddOrUpdate(key, new FloodCheck {Time = checkTime, Count = 1}, (l, check) =>
			{
				lock(check)
				{
					if(check.Time == checkTime)
					{
						if(++check.Count > count)
							throw new HttpException(403, $"Too fast, wait {CheckIntervalSec} sec pls");
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
		private const int MaxCountInInterval = 10;

		private static readonly ConcurrentDictionary<string, FloodCheck> Dict = new ConcurrentDictionary<string, FloodCheck>(StringComparer.InvariantCultureIgnoreCase);
	}
}