using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using main.auth;
using main.chat;
using main.db;

namespace main
{
	public partial class Default : Page
	{
		protected override void OnLoad(EventArgs e)
		{
			Login = AuthModule.GetAuthLogin();

			var msgs = DbStorage.FindMessages(Login);
			var files = DbStorage.FindFiles(Login);

			if(msgs.Length == 0)
			{
				var answer = new Msg {Text = ElCapitan.StartMessage, Time = DateTime.UtcNow, Type = MsgType.Answer};
				DbStorage.AddDialog(Login, null, new[] {answer}, null, files = ElCapitan.StartFiles);
				msgs = new[] {answer};
			}

			Chat.Messages = msgs;
			Explorer.Files = files;

			var score = DbStorage.FindScores(Login).FirstOrDefault();
			if(score != null)
			{
				Avatar = score.Avatar;
				Stars = score.Stars;
			}

			var flags = DbStorage.FindFlags(Login);
			HasBombTimer = ElCapitan.HasBombTimer(flags);

			var user = DbStorage.FindUserByLogin(Login);
			if(user == null)
				throw new HttpException(403, "Access denied");

			EndTime = user.EndTime != DateTime.MinValue ? user.EndTime : Settings.BombTimerEnd;

			StartBombTimer.Visible = HasBombTimer;
		}

		protected HtmlString GetStars()
		{
			return new HtmlString(string.Join(string.Empty, Enumerable.Range(0, Stars).Select(i => "<span class='star'></span>")));
		}

		protected string Login;
		protected string Avatar;
		protected int Stars;

		protected bool HasBombTimer;
		protected DateTime EndTime;
	}
}