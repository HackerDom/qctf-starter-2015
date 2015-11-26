using System.Net.Http.Formatting;
using System.Web.Http;
using Newtonsoft.Json;

namespace MartianDbHacking
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// Web API configuration and services

			// Web API routes
			config.MapHttpAttributeRoutes();
			config.Formatters.Clear();
			config.Formatters.Add(new JsonMediaTypeFormatter(){Indent = true, SerializerSettings = new JsonSerializerSettings(){NullValueHandling = NullValueHandling.Ignore}});

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);
		}
	}
}
