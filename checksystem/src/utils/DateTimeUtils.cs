using System;
using System.Globalization;

namespace main.utils
{
	public static class DateTimeUtils
	{
		public static string ToMinutesTime(this DateTime dt)
		{
			return dt.ToString("HH:mm");
		}

		public static long TotalSeconds(this DateTime dt)
		{
			return dt.Ticks / 10000000;
		}

		public static string ToJsDate(this DateTime dt)
		{
			return dt.ToString("yyyy-MM-ddTHH:mm:ssZ");
		}

		public static DateTime ParseSortable(string value)
		{
			return DateTime.ParseExact(value, "s", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
		}

		public static string ToReadable(this DateTime dt)
		{
			return dt.ToString("yyyy-MM-dd HH:mm:ss zz");
		}
	}
}