using System;

namespace main.utils
{
	public static class PrimitiveTypesExtension
	{
		public static T TryParseOrDefault<T>(this string value, T defaultValue = default(T)) where T : struct
		{
			T result;
			return Enum.TryParse(value, true, out result) ? result : defaultValue;
		}
	}
}