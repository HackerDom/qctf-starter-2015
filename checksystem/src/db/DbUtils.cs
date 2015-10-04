using System;
using System.Configuration;
using System.Data;
using System.Data.Common;

namespace main.db
{
	public static class DbHelper
	{
		public static void AddParam(this DbCommand command, string name, object value, DbType type = DbType.Object)
		{
			var param = command.CreateParameter();
			param.ParameterName = name;
			param.DbType = type;
			param.Value = value ?? DBNull.Value;
			command.Parameters.Add(param);
		}

		public static void UsingCommand(this DbConnection conn, string text, Action<DbCommand> func)
		{
			UsingCommand(conn, text, cmd =>
			{
				func(cmd);
				return true;
			});
		}

		public static T UsingCommand<T>(this DbConnection conn, string text, Func<DbCommand, T> func)
		{
			using(var cmd = conn.CreateCommand())
			{
				cmd.CommandText = text;
				cmd.Connection = conn;
				return func(cmd);
			}
		}

		public static void UsingTransaction(this DbConnection conn, Action<DbTransaction> func)
		{
			UsingTransaction(conn, tran =>
			{
				func(tran);
				return true;
			});
		}

		public static T UsingTransaction<T>(this DbConnection conn, Func<DbTransaction, T> func)
		{
			using(var tran = conn.BeginTransaction())
			{
				var result = func(tran);
				tran.Commit();
				return result;
			}
		}

		public static void UsingConnection(this ConnectionStringSettings connString, Action<DbConnection> func)
		{
			UsingConnection(connString, conn =>
			{
				func(conn);
				return true;
			});
		}

		public static T UsingConnection<T>(this ConnectionStringSettings connString, Func<DbConnection, T> func)
		{
			using(var conn = DbProviderFactories.GetFactory(connString.ProviderName).CreateConnection())
			{
				conn.ConnectionString = connString.ConnectionString;
				conn.Open();
				return func(conn);
			}
		}

		public static string TryGetString(this DbDataReader reader, int idx)
		{
			return reader.IsDBNull(idx) ? null : reader.GetString(idx);
		}

		public static int TryGetInt32(this DbDataReader reader, int idx)
		{
			return reader.IsDBNull(idx) ? 0 : reader.GetInt32(idx);
		}
	}
}