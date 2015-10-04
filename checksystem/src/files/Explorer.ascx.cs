using System;
using System.Web.UI;
using main.db;

namespace main.files
{
	public partial class Explorer : UserControl
	{
		protected override void OnLoad(EventArgs e)
		{
			FilesList.DataSource = Files;
			FilesList.DataBind();
		}

		public File[] Files;
	}
}