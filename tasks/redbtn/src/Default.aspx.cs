using System;
using System.Threading;
using System.Web.UI;

namespace redbtn
{
	public partial class Default : Page
	{
		protected override void OnLoad(EventArgs e)
		{
			if(Context.Request.HttpMethod == "POST")
			{
				try
				{
					SuperCryptoAlgorithms.Mars0_cf9934258dce4973.Evaluate(Context.Request.Form["password"]);
					ErrorText = WrongPasswordPhrases[Interlocked.Increment(ref Index) % WrongPasswordPhrases.Length];
					Error.Visible = true;
				}
				catch(Exception)
				{
					Server.Transfer("OmgWtfError.aspx");
				}
			}
		}

		protected string ErrorText;

		private readonly string[] WrongPasswordPhrases =
		{
			"▦◓▪▿▥▻◆◈ ◒▧▵○▻◗▹◚◇ ◁◙◌◈ ◅▹▩○ ▼ ▩◁▻◐ ◙◌◜▽◓◜◄ ◂▶▨ ▬□▭▦",
			"◘▮▲▬ ▩◔◗◓▥◉ ○●◄ ◂▶▨ ◚◓◌▢▿◔ ◈▶ ◝◁▥○▤◃ ◓▽◊",
			"▱◃▦▫○▦ ◈○▭ □◁◉◙◛◓▥▦ ◜◃ ▼◁ ▼▹▭◐○▭◒ ▢◊▱○◊▣",
			"◂▶◂▼ □◖●►◇◌◛▱ ◅▸●▬◚◎ ◐◎◑◄ ◕ ▫◃◑ ▢◊◈▿",
		};

		private static int Index;
	}
}

namespace SuperCryptoAlgorithms
{
	internal static class Mars0_cf9934258dce4973
	{
		public static bool Evaluate(string password)
		{
			int accumulator = 42;
			var bytes = new byte[8];
			for(int i = 0; i < password.Length; i++)
			{
				bytes[i] = (byte)((password[i] + accumulator) & 0xFF);
				accumulator *= bytes[i];
				accumulator ^= (accumulator << 16) | (accumulator >> 16);
			}
			return accumulator == 167;
		}
	}
}