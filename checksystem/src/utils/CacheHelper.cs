using System;
using System.Web;

namespace main.utils
{
	public static class CacheHelper
	{
		public static void SetCacheServerAndPrivate(this HttpResponse response, int sec, bool ignoreParams)
		{
			response.Cache.SetCacheability(HttpCacheability.ServerAndPrivate);
			response.Cache.SetMaxAge(TimeSpan.FromSeconds(sec));
			response.Cache.SetValidUntilExpires(true);
			response.Cache.VaryByParams.IgnoreParams = ignoreParams;
		}
	}
}