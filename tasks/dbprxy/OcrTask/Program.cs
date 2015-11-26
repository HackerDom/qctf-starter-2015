using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Language;

namespace OcrTask
{
    class Program
    {
        private static Random rnd;
        private static List<string> chunks;
        private static string md5Hash;

        static void Main(string[] args)
        {
            rnd = new Random(12445324);
            chunks = RandomSequence(rnd, 6).Take(1000).ToList();
            var flag = "mars0_" + chunks[42] + chunks[13] + chunks[210];
            var md5 = new MD5Cng();
            md5Hash = md5.ComputeHash(Encoding.ASCII.GetBytes(flag)).Select(b => b.ToString("x2")).Aggregate("", (s, c) => s + c);
            Console.WriteLine(flag);
            MakeImage(s => s).Save(@"..\..\english.png", ImageFormat.Png);
            MakeImage(TranslateExtensions.ToMars).Save(@"..\..\mars.png", ImageFormat.Png);
        }

        private static Bitmap MakeImage(Func<string, string> m)
        {
            var bmp = new Bitmap(1024, 2048, PixelFormat.Format32bppRgb);
            var i = 0;
            using (var g = Graphics.FromImage(bmp))
            {
                var bigFont = new Font(new FontFamily("Courier New"), 20);
                g.FillRectangle(Brushes.White, new Rectangle(0, 0, bmp.Width, bmp.Height));
                g.DrawString(m("key='mars0_'+a_i+a_j+a_k"), bigFont, Brushes.Black, 60, 10);
                g.DrawString(m("md5(key) = " + md5Hash), bigFont, Brushes.Black, 60, 50);
                g.DrawString(m("a:"), bigFont, Brushes.Black, 60, 130);
                var y = 170;
                const int lineHeight = 32;
                while (y < bmp.Height - lineHeight)
                {
                    for (var x = 60; x < 1024 - 100; x += 200)
                        g.DrawString(chunks[i++], bigFont, Brushes.Black, x, y);
                    y += lineHeight;
                }
                Console.WriteLine(i);
            }
            return bmp;
        }

        private static IEnumerable<string> RandomSequence(Random r, int itemLen)
        {
            return Enumerable.Range(0, int.MaxValue).Select(i => r.Next().ToString().PadLeft(itemLen, '0').Substring(0, itemLen));
        }
    }
}
