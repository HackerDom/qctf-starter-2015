using System;
using System.Web;
using System.Web.UI;
using main.db;
using main.utils;

namespace main.chat
{
	public partial class ChatMessage : UserControl
	{
		protected override void OnLoad(EventArgs e)
		{
			IsQuestion = Msg.Type == MsgType.Question;
			LeftTime.Visible = !IsQuestion;
			RightTime.Visible = IsQuestion;
			Text = new HtmlString(Msg.Text.HtmlEncode().ReplaceMatches(@"\r?\n", "<br/>"));
		}

		public Msg Msg;

		protected bool IsQuestion;
		protected HtmlString Text;
	}
}