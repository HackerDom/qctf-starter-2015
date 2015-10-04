namespace main.utils
{
	public static class LogUtils
	{
		public static string SafeToLog(this string value)
		{
			return value == null ? null : value.Replace('\r', ' ').Replace('\n', ' ');
		}
	}
}