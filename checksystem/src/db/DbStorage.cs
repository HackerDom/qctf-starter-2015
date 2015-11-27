using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using log4net;
using main.utils;

namespace main.db
{
	public static class DbStorage
	{
		public static void AddUser(User user)
		{
			Settings.ConnectionString.UsingConnection(conn => conn.UsingCommand("insert into users ([login], [pass], [avatar]) values (@login, @pass, @avatar)",
				cmd =>
				{
					cmd.AddParam("login", user.Login, DbType.String);
					cmd.AddParam("pass", user.Pass, DbType.String);
					cmd.AddParam("avatar", user.Avatar, DbType.String);
					if(cmd.ExecuteNonQuery() > 0)
						Log.DebugFormat("Add user '{0}'", user.Login);
				}));
		}

		public static User FindUserByPass(string pass)
		{
			return Settings.ConnectionString.UsingConnection(conn => conn.UsingCommand("select [login], [pass], [avatar], [startat] from users where pass = @pass",
				cmd =>
				{
					cmd.AddParam("pass", pass, DbType.String);
					var reader = cmd.ExecuteReader();
					if(reader.IsClosed || !reader.Read())
						return null;
					var user = new User
					{
						Login = reader.GetString(0),
						Pass = reader.GetString(1),
						Avatar = reader.TryGetString(2),
						StartTime = reader.TryGetDateTime(3)
					};
					Log.DebugFormat("Found user '{0}'", user.Login);
					return user;
				}));
		}

		public static long FindBroadcast(string login)
		{
			return Settings.ConnectionString.UsingConnection(conn => conn.UsingCommand("select max([revision]) from broadcast where login = @login",
				cmd =>
				{
					cmd.AddParam("login", login, DbType.String);
					var result = cmd.ExecuteScalar() as long?;
					if(result != null)
						Log.DebugFormat("Found broadcast id {0}", result);
					return result ?? 0L;
				}));
		}

		public static Score[] FindScores(string login = null)
		{
			return Settings.ConnectionString.UsingConnection(conn => conn.UsingCommand(string.Format("select tmp1.[login], tmp1.[avatar], tmp2.[count] from (select [login], [avatar] from users {0}) tmp1 left join (select [login], count(1) as [count], max([dt]) as [last] from flags group by [login]) tmp2 on tmp1.[login] = tmp2.[login] order by tmp2.[count] desc, tmp2.[last] asc", login == null ? null : "where login = @login"),
				cmd =>
				{
					if(login != null)
						cmd.AddParam("login", login, DbType.String);
					var reader = cmd.ExecuteReader();
					var result = IterateScores(reader).ToArray();
					Log.DebugFormat("Found {0} scores", result.Length);
					return result;
				}));
		}

		public static void AddDialog(string login, Msg question, Msg[] answers, Flag flag, File[] files, long? broadcast = null)
		{
			Settings.ConnectionString.UsingConnection(conn => conn.UsingTransaction(tran =>
			{
				AddMessage(tran, question, login);
				Array.ForEach(answers, answer => AddMessage(tran, answer, login));
				AddFlag(tran, flag, login);
				AddFiles(tran, files, login);
				SetBroadcast(tran, broadcast, login);
			}));
		}

		public static Msg[] FindMessages(string login)
		{
			return Settings.ConnectionString.UsingConnection(conn => conn.UsingCommand("select [dt], [type], [text] from chat where login = @login order by [id]",
				cmd =>
				{
					cmd.AddParam("login", login, DbType.String);
					var reader = cmd.ExecuteReader();
					var result = IterateMessages(reader).ToArray();
					Log.DebugFormat("Found {0} messages", result.Length);
					return result;
				}));
		}

		public static Dictionary<string, Flag> FindFlags(string login)
		{
			return Settings.ConnectionString.UsingConnection(conn => conn.UsingCommand("select [flag], [type] from flags where login = @login",
				cmd =>
				{
					cmd.AddParam("login", login, DbType.String);
					var reader = cmd.ExecuteReader();
					var result = IterateFlags(reader).ToDictionary(flag => flag.Value, StringComparer.InvariantCultureIgnoreCase);
					Log.DebugFormat("Found {0} flags", result.Count);
					return result;
				}));
		}

		public static File[] FindFiles(string login)
		{
			return Settings.ConnectionString.UsingConnection(conn => conn.UsingCommand("select [name], [ext], [url] from files where login = @login",
				cmd =>
				{
					cmd.AddParam("login", login, DbType.String);
					var reader = cmd.ExecuteReader();
					var result = IterateFiles(reader).ToArray();
					Log.DebugFormat("Found {0} files", result.Length);
					return result;
				}));
		}

		private static void SetBroadcast(DbTransaction tran, long? broadcast, string login)
		{
			if(broadcast == null)
				return;
			tran.Connection.UsingCommand("insert into broadcast ([login], [revision], [dt]) values (@login, @revision, @dt)",
				cmd =>
				{
					cmd.Transaction = tran;
					cmd.AddParam("login", login, DbType.String);
					cmd.AddParam("revision", broadcast.Value, DbType.Int64);
					cmd.AddParam("dt", DateTime.UtcNow, DbType.DateTime2);
					if(cmd.ExecuteNonQuery() > 0)
						Log.DebugFormat("Set broadcast to {0}", broadcast);
				});
		}

		private static void AddMessage(DbTransaction tran, Msg msg, string login)
		{
			if(msg == null)
				return;
			tran.Connection.UsingCommand("insert into chat ([dt], [type], [login], [text]) values (@dt, @type, @login, @text)",
				cmd =>
				{
					cmd.Transaction = tran;
					cmd.AddParam("dt", msg.Time, DbType.DateTime2);
					cmd.AddParam("type", msg.Type.ToString(), DbType.String);
					cmd.AddParam("login", login, DbType.String);
					cmd.AddParam("text", msg.Text, DbType.String);
					if(cmd.ExecuteNonQuery() > 0)
						Log.DebugFormat("Add {0} msg '{1}'", msg.Type, msg.Text.SafeToLog());
				});
		}

		private static void AddFlag(DbTransaction tran, Flag flag, string login)
		{
			if(flag == null)
				return;
			tran.Connection.UsingCommand("insert into flags ([login], [flag], [type], [dt]) values (@login, @flag, @type, @dt)",
				cmd =>
				{
					cmd.Transaction = tran;
					cmd.AddParam("login", login, DbType.String);
					cmd.AddParam("flag", flag.Value, DbType.String);
					cmd.AddParam("type", flag.Type.ToString(), DbType.String);
					cmd.AddParam("dt", DateTime.UtcNow, DbType.DateTime2);
					if(cmd.ExecuteNonQuery() > 0)
						Log.DebugFormat("Add {0} flag '{1}'", flag.Type, flag.Value);
				});
		}

		private static void AddFiles(DbTransaction tran, File[] files, string login)
		{
			if(files == null || files.Length == 0)
				return;
			Array.ForEach(files, file =>
			{
				tran.Connection.UsingCommand("insert into files ([login], [name], [ext], [url], [dt]) values (@login, @name, @ext, @url, @dt)", cmd =>
				{
					cmd.Transaction = tran;
					cmd.AddParam("login", login, DbType.String);
					cmd.AddParam("name", file.Name, DbType.String);
					cmd.AddParam("ext", file.Ext, DbType.String);
					cmd.AddParam("url", file.Url, DbType.String);
					cmd.AddParam("dt", DateTime.UtcNow, DbType.DateTime2);
					if(cmd.ExecuteNonQuery() > 0)
						Log.DebugFormat("Add file '{0}'", file.Name);
				});
			});
		}

		private static IEnumerable<Score> IterateScores(DbDataReader reader)
		{
			while(!reader.IsClosed && reader.Read())
			{
				yield return
					new Score
					{
						Name = reader.GetString(0),
						Avatar = reader.TryGetString(1),
						Stars = reader.TryGetInt32(2)
					};
			}
		}

		private static IEnumerable<Msg> IterateMessages(DbDataReader reader)
		{
			while(!reader.IsClosed && reader.Read())
			{
				yield return
					new Msg
					{
						Time = reader.GetDateTime(0),
						Type = reader.TryGetString(1).TryParseOrDefault(MsgType.Unknown),
						Text = reader.TryGetString(2)
					};
			}
		}

		private static IEnumerable<Flag> IterateFlags(DbDataReader reader)
		{
			while(!reader.IsClosed && reader.Read())
			{
				yield return
					new Flag
					{
						Value = reader.GetString(0),
						Type = reader.TryGetString(1).TryParseOrDefault(FlagType.Unknown)
					};
			}
		}

		private static IEnumerable<File> IterateFiles(DbDataReader reader)
		{
			while(!reader.IsClosed && reader.Read())
			{
				yield return
					new File
					{
						Name = reader.GetString(0),
						Ext = reader.GetString(1),
						Url = reader.TryGetString(2)
					};
			}
		}

		private static readonly ILog Log = LogManager.GetLogger(typeof(DbStorage));
	}
}