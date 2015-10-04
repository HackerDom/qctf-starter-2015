using System;
using System.Linq;
using System.Web;
using main.auth;
using main.db;

namespace main.chat
{
	public class Check : BaseHandler
	{
		protected override AjaxResult ProcessRequestInternal(HttpContext context)
		{
			var login = AuthModule.GetAuthLogin();

			/*if(DateTime.UtcNow > Settings.BombTimerEnd)
				throw new HttpException(403, "Connection lost...");*/

			var revision = DbStorage.FindBroadcast(login);
			var flags = DbStorage.FindFlags(login);

			var timer = ElCapitan.HasBombTimer(flags) ? Settings.BombTimerEnd : DateTime.MinValue;

			var answers = ElCapitan.GetBroadcastMsgs(ref revision);
			if(answers.Length == 0)
				return new AjaxResult {Messages = null, Files = null, Score = 0, Timer = timer};

			var msgs = answers.Select(msg => new Msg {Text = msg, Time = DateTime.UtcNow, Type = MsgType.Answer}).ToArray();

			DbStorage.AddDialog(login, null, msgs, null, null, revision);
			return new AjaxResult {Messages = msgs, Files = null, Score = 0, Timer = timer};
		}
	}
}