using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;

namespace main.utils
{
	public static class JsonHelper
	{
		public static string ToJsonString<T>(this T obj)
		{
			return Encoding.UTF8.GetString(obj.ToJson());
		}

		public static byte[] ToJson<T>(this T obj)
		{
			using(var stream = new MemoryStream())
			{
				obj.ToJson(stream);
				return stream.ToArray();
			}
		}

		public static void ToJson<T>(this T obj, Stream stream)
		{
			using(var writer = JsonReaderWriterFactory.CreateJsonWriter(stream, Encoding.UTF8, false))
				new DataContractJsonSerializer(Equals(obj, null) ? typeof(T) : obj.GetType()).WriteObject(writer, obj);
		}

		public static T ParseJson<T>(string record)
		{
			var reader = JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(record), XmlDictionaryReaderQuotas.Max);
			return (T)new DataContractJsonSerializer(typeof(T), Settings).ReadObject(reader);
		}

		private static readonly DataContractJsonSerializerSettings Settings = new DataContractJsonSerializerSettings {UseSimpleDictionaryFormat = true};
	}
}