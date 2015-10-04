using System.Collections.Generic;
using System.Linq;
using main.utils;

namespace main.db
{
	public static class DbUsers
	{
		public static User FindUser(string login)
		{
			User user;
			return Users.TryGetValue(login, out user) ? user : null;
		}

		private static readonly Dictionary<string, User> Users = FileReader.ReadObjects<User>("../settings/users").ToDictionary(user => user.Login);
	}
}