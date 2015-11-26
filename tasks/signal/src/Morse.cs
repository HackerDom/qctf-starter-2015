using System.Collections.Generic;
using System.Linq;

namespace TaskMorse
{
	static class Morse
	{
		public static List<bool> Encode(string source)
		{
			var result = new List<bool>();
			result = source.Split(' ').Aggregate(result, (current, word) => current.Concat(EncodeWord(word)).ToList());
			return result.Take(result.Count).ToList();
		}

		public static List<bool> EncodeWord(string source)
		{
			var result = new List<bool>();
			result = source.Aggregate(result, (current, ch) => current.Concat(EncodeLetter(ch)).ToList());
			for(var i = 0; i < 4; i++)
				result.Add(false);
			return result;
		}

		public static List<bool> EncodeLetter(char source)
		{
			var result = new List<bool>();
			foreach(var ch in Alphabet[source])
			{
				if(ch == '.')
					result.Add(true);
				else if(ch == '-')
				{
					result.Add(true);
					result.Add(true);
					result.Add(true);
				}
				result.Add(false);
			}
			result.Add(false);
			result.Add(false);
			return result;
		}

		private static readonly Dictionary<char, string> Alphabet = new Dictionary<char, string>
			{
				{'A', ".-"},
				{'a', ".-"},
				{'B', "-..."},
				{'b', "-..."},
				{'C', "-.-."},
				{'c', "-.-."},
				{'D', "-.."},
				{'d', "-.."},
				{'E', "."},
				{'e', "."},
				{'F', "..-."},
				{'f', "..-."},
				{'G', "--."},
				{'g', "--."},
				{'H', "...."},
				{'h', "...."},
				{'I', ".."},
				{'i', ".."},
				{'J', ".---"},
				{'j', ".---"},
				{'K', "-.-"},
				{'k', "-.-"},
				{'L', ".-.."},
				{'l', ".-.."},
				{'M', "--"},
				{'m', "--"},
				{'N', "-."},
				{'n', "-."},
				{'O', "---"},
				{'o', "---"},
				{'P', ".--."},
				{'p', ".--."},
				{'Q', "--.-"},
				{'q', "--.-"},
				{'R', ".-."},
				{'r', ".-."},
				{'S', "..."},
				{'s', "..."},
				{'T', "-"},
				{'t', "-"},
				{'U', "..-"},
				{'u', "..-"},
				{'V', "...-"},
				{'v', "...-"},
				{'W', ".--"},
				{'w', ".--"},
				{'X', "-..-"},
				{'x', "-..-"},
				{'Y', "-.--"},
				{'y', "-.--"},
				{'Z', "--.."},
				{'z', "--.."},
				{'0', "-----"},
				{',', "--..--"},
				{'1', ".----"},
				{'.', ".-.-.-"},
				{'2', "..---"},
				{'?', "..--.."},
				{'3', "...--"},
				{';', "-.-.-."},
				{'4', "....-"},
				{':', "---..."},
				{'5', "....."},
				{'\'', ".----."},
				{'6', "-...."},
				{'-', "-....-"},
				{'7', "--..."},
				{'/', "-..-."},
				{'8', "---.."},
				{'(', "-.--.-"},
				{'9', "----."},
				{')', "-.--.-"},
				{' ', " "},
				{'_', "..--.-"}
			};
	}
}
