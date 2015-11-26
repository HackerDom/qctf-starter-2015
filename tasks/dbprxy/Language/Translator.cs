using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Language
{
    public class Translator
    {
        private readonly Dictionary<string, string> table;

        public Translator(IEnumerable<Tuple<string, string>> table)
        {
            this.table = table.ToDictionary(t => t.Item1, t => t.Item2);
        }

        public string Translate(string text)
        {
            return string.Join("\r\n", Regex.Split(text.ToLower(), @"\r?\n").Select(TranslateLine));
        }

        private string TranslateLine(string text)
        {
            if (text.Length % 2 == 1)
                return Translate(text + " ");
            var res = new StringBuilder();
            for (int i = 0; i < text.Length; i += 2)
            {
                var key = text.Substring(i, 2);
                res.Append(table.ContainsKey(key) ? table[key] : key);
            }
            return res.ToString();
        }

        public static IEnumerable<Tuple<string, string>> BuildTable(string sourceAbc, string destAbc, Random rnd)
        {
            if (sourceAbc.Length > destAbc.Length)
                throw new ArgumentException();
            var fromKeys = sourceAbc.SelectMany(a => sourceAbc.Select(b => "" + a + b)).Concat(sourceAbc.SelectMany(a => new[] { a + " ", " " + a }));
            var toKeys = destAbc.SelectMany(a => destAbc.Select(b => "" + a + b)).Shuffle(rnd).Concat(destAbc.Shuffle(rnd).SelectMany(a => new[] { a + " ", " " + a }));
            return fromKeys.Zip(toKeys, Tuple.Create);
        }

        public static IEnumerable<Tuple<string, string>> ReverseTable(IEnumerable<Tuple<string, string>> table)
        {
            return table.Select(t => Tuple.Create(t.Item2, t.Item1));
        }

        public static string BuildAlphabet(char start, int count)
        {
            return new string(Enumerable.Range(start, count).Select(i => (char)i).ToArray());
        }

        public static readonly string EarthAlphabet = BuildAlphabet('a', 26) + BuildAlphabet('0', 10) + "!@#$%^&*()_+=-/:{}[]?<>~,.";
        public static readonly string MartianAlphabet = BuildAlphabet('■', 62);
    }

    public static class TranslateExtensions
    {
        public static List<Tuple<string, string>> table = Translator.BuildTable(Translator.EarthAlphabet, Translator.MartianAlphabet, new Random(1234567)).ToList();
        public static Translator toMars = new Translator(table);
        public static Translator toEarth = new Translator(Translator.ReverseTable(table));

        public static string ToMars(this string earthText)
        {
            return toMars.Translate(earthText);
        }
        public static string ToEarth(this string marsText)
        {
            return toEarth.Translate(marsText);
        }

        public static bool IsMartian(this string text)
        {
            return text.All(c => Translator.MartianAlphabet.Contains(c.ToString()));
        }
    }
}
