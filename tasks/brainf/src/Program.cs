using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using log4net;
using log4net.Config;

namespace BrainFuckTask
{
	static class Program
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Program));
		static readonly List<Tuple<bool[,], string>> ExpectedMatricesToText = new List<Tuple<bool[,], string>>();

		static void Main(string[] args)
		{
			XmlConfigurator.Configure();
			try
			{
				foreach(var fileName in Directory.EnumerateFiles("data"))
				{
					var lines = File.ReadAllLines(fileName);
					ExpectedMatricesToText.Add(Tuple.Create(GetMatrixFromString(lines.Take(10).ToList()), string.Join("\r\n", lines.Skip(10))));
				}

				new AsyncListener("brainfuck.contest.qctf.ru", int.Parse(args[0]), Interpretate).Loop(); //"q5391Lan"

				Thread.Sleep(-1);
			}
			catch(Exception e)
			{
				Log.Fatal(e);
			}
		}

		private static async Task Interpretate(HttpListenerContext context)
		{
			using (var sr = new StreamReader(context.Request.InputStream, Encoding.GetEncoding(1251)))
			{
				var program = sr.ReadToEnd();
				if (program.StartsWith("aor="))
					program = HttpUtility.UrlDecode(program.Substring(4));

				Log.Info(program);

				bool[,] matrix;
				var data = Encoding.UTF8.GetBytes(GeneratePage(Interpretator2.Iterpretate(program, out matrix), matrix, program));
				await context.Response.OutputStream.WriteAsync(data, 0, data.Length);
			}
		}

		private static string GeneratePage(InterpretatorEndState state, bool[,] matrix, string program)
		{
			var page = @"
<!DOCTYPE html>
<html lang=""en"">
<head>
	<meta charset=""utf-8""/>
	<title style=""line-height: 0px;"">◃◝◜◃◊▱◅▹▭ ◊▱◚◓◊□ ◓▼▹▤▦◜▯◊◁◌◛</title>
</head>
<body>
	<h1>◃◝◜◃◊▱◅▹▭ ◊▱◚◓◊□ ◓▼▹▤▦◜▯◊◁◌◛</h1>
	<div>◔▲▴◆▼▹◃○ ▴◌◛ ◓▼▹▤▦◜▯◊◁◌◛</div>
	{4}

	<form method=""POST"" style=""margin-top: 12px;"">
		<div>
			<textarea id=""input"" name=""aor"" rows=""10"" cols=""80"">{3}</textarea>
		</div>

		<button id=""e2m"" type=""submit"" style=""margin-top: 12px;"">▱○○●◈ ▥▻◒◎</button>
	<form>
	<div>
		{0}
	</div>
	<pre>
		{1}
	</pre>
	<div>
		{2}
	</div>
</body>
</html>";

			return string.Format(page, ConvertInterpretatorEndState(state), ConvertMatrixToString(matrix), GetAnswer(matrix), HttpUtility.HtmlEncode(program), Interpretator2.GetRulesHtml());
		}

		static string ConvertInterpretatorEndState(InterpretatorEndState state)
		{
			return state == InterpretatorEndState.Success ? "◓◌◈ ◈▿◃ " 
				: state == InterpretatorEndState.TooBigProgramFail ? "▩○▲◒►◎ ▶▢◁◛►▵▽▩ "
				: state == InterpretatorEndState.TooMuchTimeFail ? "◅◗▶ ▵▽▿◔ ▶▱▮▣▧◅▹◗◌" 
				: "";
		}

		static string GetAnswer(bool[,] resultMatrix)
		{
			var foundPair = ExpectedMatricesToText.FirstOrDefault(pair => MatrixEquals(resultMatrix, pair.Item1));
			return foundPair == null ? "" : foundPair.Item2;
		}

		static string ConvertMatrixToString(bool[,] resultMatrix)
		{
			if (resultMatrix == null)
				return "";

			string result = "";

			for (int y = 0; y < resultMatrix.GetLength(1); y++)
			{
				result += "<div>";
				for (int x = 0; x < resultMatrix.GetLength(0); x++)
				{
					result += resultMatrix[x, y] ? "O" : ".";
				}
				result += "</div>";
			}

			return result;
		}

		static bool[,] GetMatrixFromString(List<string> lines)
		{
			var matrix = new bool[lines[0].Length, lines.Count];

			for (int y = 0; y < matrix.GetLength(1); y++)
			{
				for (int x = 0; x < matrix.GetLength(0); x++)
				{
					matrix[x, y] = lines[y][x] == 'O';
				}
			}

			return matrix;
		}

		static bool MatrixEquals(bool[,] result, bool[,] expected)
		{
			if (result == null)
				return false;

			for (int y = 0; y < result.GetLength(1); y++)
			{
				for (int x = 0; x < result.GetLength(0); x++)
				{
					if (result[x, y] != expected[x, y])
						return false;
				}
			}

			return true;
		}
	}
}
