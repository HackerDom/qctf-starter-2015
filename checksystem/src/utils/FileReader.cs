using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace main.utils
{
	public static class FileReader
	{
		public static string ReadString(string relativeFilePath, Encoding encoding = null)
		{
			var filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativeFilePath);
			return File.ReadAllText(filepath, encoding ?? Encoding.UTF8);
		}

		public static string[] ReadStrings(string relativeFilePath, Encoding encoding = null)
		{
			var filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativeFilePath);
			return File.ReadAllLines(filepath, encoding ?? Encoding.UTF8);
		}

		public static ConcurrentDictionary<TKey, TValue> ReadDict<TKey, TValue>(string relativeFilePath, Func<string, TKey> readKey, Func<string, TValue> readValue, IEqualityComparer<TKey> comparer = null, Encoding encoding = null, string delim = "\t")
		{
			var filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativeFilePath);
			var lines = File.ReadLines(filepath, encoding ?? Encoding.UTF8);
			var dict = new ConcurrentDictionary<TKey, TValue>(comparer ?? EqualityComparer<TKey>.Default);
			foreach(var ln in lines)
			{
				var line = ln.CutEnd("#").Trim();
				if(line == string.Empty)
					continue;
				var delimOffset = line.IndexOf(delim, StringComparison.Ordinal);
				if(delimOffset <= 0)
					dict[readKey(line)] = default(TValue);
				else
				{
					var key = line.Substring(0, delimOffset).TrimEnd();
					var value = line.Substring(delimOffset + 1).TrimStart();
					dict[readKey(key)] = readValue(value);
				}
			}
			return dict;
		}
	}
}