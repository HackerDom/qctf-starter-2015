using System;
using System.Web.UI;
using main.db;

namespace main.chat
{
	public partial class Chat : UserControl
	{
		protected override void OnLoad(EventArgs e)
		{
			Dialog.DataSource = Messages;
			Dialog.DataBind();
		}

		public Msg[] Messages;
	}
}