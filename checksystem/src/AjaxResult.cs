using System;
using System.Runtime.Serialization;
using main.db;
using main.utils;

namespace main
{
	[DataContract]
	public class AjaxResult
	{
		[DataMember(Name = "text", EmitDefaultValue = false)] public string Text;
		[DataMember(Name = "error", EmitDefaultValue = false)] public string Error;
		[DataMember(Name = "msgs", EmitDefaultValue = false)] public Msg[] Messages;
		[DataMember(Name = "files", EmitDefaultValue = false)] public File[] Files;
		[DataMember(Name = "score", EmitDefaultValue = false)] public int Score;
		[DataMember(Name = "timer", EmitDefaultValue = false)] private string timer;
		[IgnoreDataMember] public DateTime Timer;

		[OnSerializing]
		private void OnSerializing(StreamingContext context)
		{
			timer = Timer == DateTime.MinValue ? null : Timer.ToJsDate();
		}
	}
}