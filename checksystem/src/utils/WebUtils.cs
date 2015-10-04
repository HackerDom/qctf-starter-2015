using System.Web;

namespace main.utils
{
	public static class WebUtils
	{
		public static string HtmlEncode(this string value)
		{
			return value == null ? null : HttpUtility.HtmlEncode(value);
		}
	}
}