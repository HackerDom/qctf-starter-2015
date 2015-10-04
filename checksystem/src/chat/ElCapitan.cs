using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using main.db;
using main.utils;

namespace main.chat
{
	public static class ElCapitan
	{
		static ElCapitan()
		{
		}

		public static string GetAnswer(string question, Dictionary<string, Flag> state, out Flag flag, out File[] files, out DateTime timer)
		{
			flag = null;
			files = null;

			timer = HasBombTimer(state) ? Settings.BombTimerEnd : DateTime.MinValue;

			question = question.TrimToNull();
			if(question == null)
				return GetRandomAnswer(DefaultAnswers, string.Empty);

			var possibleFlag = question.SanitizeFlag();

			if(state.ContainsKey(possibleFlag))
				return GetRandomAnswer(OldFlagAnswers, possibleFlag);

			FlagType flagType;
			if(Flags.TryGetValue(possibleFlag, out flagType))
				flag = new Flag {Value = possibleFlag, Type = flagType};

			FlagFiles.TryGetValue(possibleFlag, out files);

			string answer;
			if(FlagAnswers.TryGetValue(possibleFlag, out answer))
				return answer + GetSuffix(flag, state, ref timer);

			if(possibleFlag.StartsWith(Settings.FlagPrefix, StringComparison.InvariantCultureIgnoreCase))
				return GetRandomAnswer(InvalidFlagAnswers, question);

			if(question.IndexOfAny(Settings.MartianChars) >= 0)
				return GetRandomAnswer(MartianAnswers, question);

			if(HintAnswers.TryGetValue(question, out answer))
				return answer;

			return GetRandomAnswer(DefaultAnswers, question);
		}

		public static bool GameEnded(Dictionary<string, Flag> state)
		{
			return DateTime.UtcNow > Settings.BombTimerEnd || Flags.Keys.All(state.ContainsKey);
		}

		public static int GetBombFlags(Dictionary<string, Flag> state)
		{
			return state == null ? 0 : state.Count(pair => pair.Value.Type == FlagType.Bomb);
		}

		public static bool HasBombTimer(Dictionary<string, Flag> state)
		{
			return state != null && state.ContainsKey(Settings.MainBombFlag) && GetBombFlags(state) < Settings.BombShutDownFlagsCount;
		}

		public static string[] GetBroadcastMsgs(ref long revision)
		{
			if(Broadcasts.Count == 0)
				return new string[0];
			var rev = revision;
			var result = Broadcasts.Where(pair => pair.Key > rev).ToArray();
			revision = result.LastOrDefault().Key;
			return result.Select(pair => pair.Value).ToArray();
		}

		public static int FlagsCount { get { return Flags.Count; } }
		public static readonly string StartMessage = FileReader.ReadString("../settings/chat/start").TrimToNull();
		public static readonly File[] StartFiles = FileReader.ReadStrings("../settings/chat/startFiles").Select(line => line.TrimToNull()).Select(JsonHelper.ParseJson<File>).ToArray();

		private static string GetRandomAnswer(string[] answers, string question)
		{
			return answers[Math.Abs((question.GetHashCode() ^ Environment.TickCount) % answers.Length)];
		}

		private static bool IsMainBombFlag(string flag)
		{
			return string.Equals(flag, Settings.MainBombFlag, StringComparison.InvariantCultureIgnoreCase);
		}

		private static string GetSuffix(Flag flag, Dictionary<string, Flag> state, ref DateTime timer)
		{
			if(flag == null)
				return null;
			if(state != null && Flags.Keys.All(f => state.ContainsKey(f) || string.Equals(flag.Value, f, StringComparison.InvariantCultureIgnoreCase)))
			{
				timer = DateTime.MinValue;
				return Settings.EndGameSuffix;
			}
			var bombs = GetBombFlags(state);
			if(IsMainBombFlag(flag.Value))
			{
				timer = Settings.BombTimerEnd;
				if(state == null || bombs == 0)
					return null;
				if(bombs < Settings.BombShutDownFlagsCount)
					return string.Format(Settings.MainBombTaskSuffix, bombs);
				timer = DateTime.MinValue;
				return Settings.BombShuttedDownSuffix;
			}
			if(flag.Type == FlagType.Bomb)
			{
				bombs++;
				var hasBombTimer = HasBombTimer(state);
				if(hasBombTimer && bombs < Settings.BombShutDownFlagsCount)
				{
					timer =  Settings.BombTimerEnd;
					return Settings.BombTasksSuffix;
				}
				if(hasBombTimer && bombs == Settings.BombShutDownFlagsCount)
				{
					timer = DateTime.MinValue;
					return Settings.BombShuttedDownSuffix;
				}
				timer = DateTime.MinValue;
				return null;
			}
			return null;
		}

		public static string SanitizeFlag(this string value)
		{
			return value.ReplaceMatches(@"[^\d\p{L}]+", string.Empty);
		}

		private static readonly SortedList<long, string> Broadcasts = new SortedList<long, string>(FileReader.ReadDict("../settings/chat/broadcast", long.Parse, s => s.Replace("\\n", "\n")));
		private static readonly ConcurrentDictionary<string, FlagType> Flags = FileReader.ReadDict("../settings/chat/flags", s => s.SanitizeFlag(), s => s.TryParseOrDefault(FlagType.Unknown), StringComparer.InvariantCultureIgnoreCase);
		private static readonly ConcurrentDictionary<string, string> FlagAnswers = FileReader.ReadDict("../settings/chat/flagAnswers", s => s.SanitizeFlag(), s => s.Replace("\\n", "\n"), StringComparer.InvariantCultureIgnoreCase);
		private static readonly ConcurrentDictionary<string, string> HintAnswers = FileReader.ReadDict("../settings/chat/hintAnswers", s => s.SanitizeFlag(), s => s.Replace("\\n", "\n"), StringComparer.InvariantCultureIgnoreCase);
		private static readonly ConcurrentDictionary<string, File[]> FlagFiles = FileReader.ReadDict("../settings/chat/files", s => s.SanitizeFlag(), JsonHelper.ParseJson<File[]>, StringComparer.InvariantCultureIgnoreCase);
		private static readonly string[] InvalidFlagAnswers = FileReader.ReadStrings("../settings/chat/invalidFlags");
		private static readonly string[] OldFlagAnswers = FileReader.ReadStrings("../settings/chat/oldFlags");
		private static readonly string[] MartianAnswers = FileReader.ReadStrings("../settings/chat/martian");
		private static readonly string[] DefaultAnswers = FileReader.ReadStrings("../settings/chat/default");
	}
}