using System;
using System.Web;
using main.auth;
using main.db;
using main.utils;

namespace main.chat
{
	public class Send : BaseHandler
	{
		protected override AjaxResult ProcessRequestInternal(HttpContext context)
		{
			var login = AuthModule.GetAuthLogin();
			var flags = DbStorage.FindFlags(login);

			if(ElCapitan.GameEnded(flags))
				throw new HttpException(403, "The End");

			var user = DbStorage.FindUserByLogin(login);
			if(user == null)
				throw new HttpException(403, "Access denied");

			if(user.EndTime != DateTime.MinValue && user.EndTime < DateTime.UtcNow)
				throw new HttpException(403, "The End");

			var question = context.Request.Form["question"].TrimToNull();
			if(question == null)
				throw new HttpException(400, "Message is empty");

			if(question.Length > Settings.MaxMsgLength)
				throw new HttpException(400, "Message too large");

			Flag flag;
			File[] files;
			DateTime timer;

			var answer = ElCapitan.GetAnswer(question, flags, out flag, out files, out timer);
			var msg = new Msg {Text = answer, Time = DateTime.UtcNow, Type = MsgType.Answer};

			DbStorage.AddDialog(login, new Msg {Text = question, Time = DateTime.UtcNow, Type = MsgType.Question}, new[] {msg}, flag, files);

			return new AjaxResult {Messages = new[] {msg}, Files = files, Score = flag != null ? 1 : 0, Timer = timer};
		}
	}
}