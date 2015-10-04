using System.Runtime.Serialization;

namespace main.db
{
	[DataContract]
	public class File
	{
		[DataMember(Name = "name")] public string Name;
		[DataMember(Name = "ext")] public string Ext;
		[DataMember(Name = "url")] public string Url;
	}
}