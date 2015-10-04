using System;
using System.Text.RegularExpressions;

namespace main.utils
{
	public static class StringUtils
	{
		public static string TrimToNull(this string value)
		{
			if(value == null)
				return null;
			value = value.Trim();
			if(value == string.Empty)
				return null;
			return value;
		}

		public static string ReplaceMatches(this string value, string pattern, string replacement)
		{
			return value == null ? null : Regex.Replace(value, pattern, replacement);
		}

		public static string CutEnd(this string value, string delim, bool ignoreCase = true)
		{
			if(value == null || delim == null)
				return value;
			var index = value.IndexOf(delim, ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture);
			return index < 0 ? value : value.Substring(0, index);
		}
	}
}