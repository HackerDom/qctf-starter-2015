using System;
using System.Linq;
using NUnit.Framework;

namespace Language
{
    [TestFixture]
    public class Translator_should
    {
        [Test]
        public void translate()
        {
            var text = "martian method knowledge error bad good ";
            Console.WriteLine(text.ToMars());
            Console.WriteLine(text.ToMars().ToEarth());
            Assert.AreEqual(text.ToLower(), text.ToMars().ToEarth());
            Assert.AreEqual(text.ToLower(), text.ToMars().ToEarth().ToMars().ToEarth());
        }

        [Test]
        public void create_table()
        {
            var pairs = TranslateExtensions.table.Select(t => string.Format("{0}{1}", t.Item1, t.Item2)).Shuffle();
            var s = string.Join("", pairs);
			
            Console.WriteLine(s);
        }

        [Test]
        public void translate_multiline_text()
        {
            var text = @"hello,
world!

";
            Assert.AreEqual(text, text.ToMars().ToEarth());
            Assert.AreEqual("○▭▱◜▴◙\r\n▥▻△◉◂▤\r\n\r\n", text.ToMars());
        }
        [Test]
        public void show_alphabet()
        {
            Console.WriteLine(Translator.MartianAlphabet);
        }
    }
}