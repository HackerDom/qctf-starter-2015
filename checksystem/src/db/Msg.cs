using System;
using System.Runtime.Serialization;
using main.utils;

namespace main.db
{
	[DataContract]
	public class Msg
	{
		[IgnoreDataMember] public MsgType Type;
		[IgnoreDataMember] public DateTime Time;
		[DataMember(Name = "time", EmitDefaultValue = false)] private string time;
		[DataMember(Name = "text", EmitDefaultValue = false)] public string Text;

		[OnSerializing]
		private void OnSerializing(StreamingContext context)
		{
			time = Time.ToMinutesTime();
		}
	}

	public enum MsgType
	{
		Unknown,
		Question,
		Answer
	}
}