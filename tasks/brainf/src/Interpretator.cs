using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainFuckTask
{
	public static class Interpretator
	{
		public const int MaxProgramLength = 100; 

		private static Dictionary<char, Action<int, InterpretatorState>> Rules = new Dictionary<char, Action<int, InterpretatorState>>
		{
			{'+', (posTo, state) => state.AddToValue(1)},
			{'-', (posTo, state) => state.AddToValue(-1)},
			{'>', (posTo, state) => state.AddToX(1)},
			{'<', (posTo, state) => state.AddToX(-1)},
			{'A', (posTo, state) => state.AddToY(-1)},
			{'V', (posTo, state) => state.AddToY(1)},
			{'[', (posTo, state) => { if(state.Values[state.ValueIdx] == 0 && posTo > 0) state.MoveTo(posTo); else state.DoNothing();}},
			{']', (posTo, state) => { if(state.Values[state.ValueIdx] > 0) state.MoveTo(posTo); else state.DoNothing();}},
			{'.', (posTo, state) => state.SetMatrix()},
			{'^', (posTo, state) => state.AddToValueIdx(1)},
			{'_', (posTo, state) => state.AddToValueIdx(-1)},
		};

		public static InterpretatorEndState Iterpretate(string pseudobfProgram, out bool[,] resultMatrix)
		{
			resultMatrix = null;
			if (pseudobfProgram.Length > MaxProgramLength)
				return InterpretatorEndState.TooBigProgramFail;

			var compiled = CompileSource(pseudobfProgram);
			var state = new InterpretatorState();

			while (state.IterationsCount < 10000)
			{
				if (state.ProgramPosition >= compiled.Count)
				{
					resultMatrix = state.Matrix;
					return InterpretatorEndState.Success;
				}

				if (compiled[state.ProgramPosition] == null)
				{
					if (state.Values[state.ValueIdx] == 0)
					{
						resultMatrix = state.Matrix;
						return InterpretatorEndState.Success;
					}

					state.DoNothing();
					continue;
				}

				compiled[state.ProgramPosition](state);
			}

			return InterpretatorEndState.TooMuchTimeFail;
		}

		private static List<Action<InterpretatorState>> CompileSource(string program)
		{
			var cleanedCode = program.Where(c => c == 'X' || Rules.ContainsKey(c)).ToList();

			var gotos = GenerateGotos(cleanedCode);

			var compiledSource = new List<Action<InterpretatorState>>();

			for (int i = 0; i < cleanedCode.Count; i++)
			{
				if (cleanedCode[i] == 'X')
				{
					compiledSource.Add(null);
					continue;
				}
				int j = i;
				compiledSource.Add(state => Rules[cleanedCode[j]](gotos[j], state));
			}

			return compiledSource;
		}

		private static int[] GenerateGotos(List<char> bfProgram)
		{
			int[] gotos = new int[bfProgram.Count];
			Stack<int> startsOfLoops = new Stack<int>();

			for (int i = 0; i < bfProgram.Count; i++)
			{
				if (bfProgram[i] == '[')
				{
					startsOfLoops.Push(i);
				}
				else if (bfProgram[i] == ']')
				{
					if (startsOfLoops.Count == 0)
						gotos[i] = 0;
					else
					{
						var loopStart = startsOfLoops.Pop();
						gotos[i] = loopStart;
						gotos[loopStart] = i;

					}
				}
			}
			return gotos;
		}

		public static string GetRulesHtml()
		{
			return "<div>&lt+[V_.X^A-]&gt;</div>";
		}
	}

	class InterpretatorState
	{
		private const int XSize = 10;
		private const int YSize = 10;

		public int IterationsCount = 0;
		public int ProgramPosition = 0;
		public int[] Values = new int[256];
		public int ValueIdx;
		public bool[,] Matrix = new bool[XSize, YSize];
		public int X;
		public int Y;

		private void OnIterationStart()
		{
			IterationsCount++;
			ProgramPosition++;
		}

		public void DoNothing()
		{
			OnIterationStart();
		}

		public void AddToX(int count)
		{
			OnIterationStart();
			X = (X + XSize + count) % XSize;
		}

		public void AddToY(int count)
		{
			OnIterationStart();
			Y = (Y + YSize + count) % YSize;
		}

		public void AddToValueIdx(int count)
		{
			OnIterationStart();
			ValueIdx = (ValueIdx + count + 256) % 256;
		}

		public void AddToValue(int count)
		{
			OnIterationStart();
			Values[ValueIdx] = (Values[ValueIdx] + count + 256) % 256;
		}

		public void MoveTo(int x)
		{
			OnIterationStart();
			ProgramPosition = x;
		}

		public void SetMatrix()
		{
			OnIterationStart();
			Matrix[X, Y] ^= true;
		}
	}

	public enum InterpretatorEndState
	{
		Success,
		TooBigProgramFail,
		TooMuchTimeFail
	}
}
