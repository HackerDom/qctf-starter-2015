using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml;
using System.Xml.Serialization;

namespace MartianDbProxy
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// Web API configuration and services

			// Web API routes
			config.MapHttpAttributeRoutes();
			config.Formatters.Clear();
			var xmlFormatter = new NoXmlnsXmlMediaTypeFormatter();
			config.Formatters.Add(xmlFormatter);
			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
				);
		}
	}
	
	public class NoXmlnsXmlMediaTypeFormatter : XmlMediaTypeFormatter
	{
		public NoXmlnsXmlMediaTypeFormatter()
		{
			UseXmlSerializer = true;
		}

		public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext, CancellationToken cancellationToken)
		{
			try
			{
				WriteToStream(type, value, writeStream, content);
				return Task.FromResult(0);
			}
			catch (Exception ex)
			{
				TaskCompletionSource<int> completionSource = new TaskCompletionSource<int>();
				completionSource.SetException(ex);
				return completionSource.Task;
			}
		}

		private void WriteToStream(Type type, object value, Stream writeStream, HttpContent content)
		{
			object serializer = GetSerializer(type, value, content);
			using (XmlWriter xmlWriter = CreateXmlWriter(writeStream, content))
			{
				//Remove namespaces!
				var ns = new XmlSerializerNamespaces();
				ns.Add("", "");
				((XmlSerializer)serializer).Serialize(xmlWriter, value, ns);
			}
		}
	}
}