using System;
using System.IO;
using System.Linq;

namespace LinkedIn
{
	internal class CrossClimb : Game
	{
		// Words are the solution order; Hints/Guesses are what the player manipulates
		private readonly string[] Words = new string[7];
		private readonly string[] Hints = new string[7];
		private readonly string[] Guesses = new string[7];
		private bool IsReversed = false; // If true, climb is reversed

		private static readonly Random _random = new();

		public CrossClimb(string filename)
		{
			(string[] words, string[] hints) = LoadWordsAndHints(filename);
			if (words.Length != 7 || hints.Length != 7)
				throw new Exception("Puzzle must have exactly 7 words and 7 hints.");

			// Words stay in solution order
			for (int i = 0; i < 7; i++)
				Words[i] = words[i];

			// Shuffle hints (and guesses) together
			int[] indices = new int[7];
			for (int i = 0; i < 7; i++) indices[i] = i;
			_random.Shuffle(indices);

			for (int i = 0; i < 7; i++)
			{
				Hints[i] = hints[indices[i]];
				Guesses[i] = ""; // Start with empty guesses
			}
		}

		public override void Main()
		{
			int index = 0;
			while (true)
			{
				Console.Clear();
				string[] hints = GetCurrentHints();

				Console.WriteLine($"CrossClimb {(IsReversed ? "(Reversed)" : "")}\n");

				// Display all hints and guesses in a list, highlight the current index
				Console.WriteLine(" #  |  HINT".PadRight(30) + "|  GUESS");
				Console.WriteLine(new string('-', 50));
				for (int i = 0; i < 7; i++)
				{
					string pointer = (i == index) ? ">" : " ";
					string guess = Guesses[i] ?? "";
					Console.WriteLine($"{pointer}{i + 1,2}: {hints[i].PadRight(25)}| {guess}");
				}
				Console.WriteLine("\nUp/Down: Move  |  L/R: Move hint up/down  |  V: Reverse  |  Enter: Edit guess  |  C: Check  |  Esc: Exit");

				ConsoleKey key = Console.ReadKey(true).Key;
				switch (key)
				{
					case ConsoleKey.UpArrow:
						index = Math.Max(0, index - 1);
						break;
					case ConsoleKey.DownArrow:
						index = Math.Min(6, index + 1);
						break;
					case ConsoleKey.L when index > 0:
						SwapHints(index, index - 1);
						SwapGuesses(index, index - 1);
						index--;
						break;
					case ConsoleKey.R when index < 6:
						SwapHints(index, index + 1);
						SwapGuesses(index, index + 1);
						index++;
						break;
					case ConsoleKey.V:
						IsReversed = !IsReversed;
						break;
					case ConsoleKey.Enter:
						Console.SetCursorPosition(0, 10 + index); // Move cursor to the guess line
						Console.Write("Enter your guess: ");
						string guess = Console.ReadLine() ?? "";
						Guesses[index] = guess;
						break;
					case ConsoleKey.C:
						Console.WriteLine();
						(bool allCorrect, bool transitionOk) = CheckSolution();
						if (allCorrect && transitionOk)
							Console.WriteLine("Congratulations! All guesses are correct, in the right order, and transitions are valid!");
						else
						{
							if (!allCorrect)
								Console.WriteLine("Some guesses are incorrect or out of order.");
							if (!transitionOk)
								Console.WriteLine("Some transitions between words are invalid (not a 1-letter change).");
						}
						Console.WriteLine("Press any key to continue...");
						Console.ReadKey(true);
						break;
					case ConsoleKey.Escape:
						break;
					default:
						break;
				}
			}
		}

		private void SwapHints(int i, int j)
		{
			(string, string) temp = (Hints[i], Hints[j]);
			Hints[i] = temp.Item2;
			Hints[j] = temp.Item1;
		}

		private void SwapGuesses(int i, int j)
		{
			(string, string) temp = (Guesses[i], Guesses[j]);
			Guesses[i] = temp.Item2;
			Guesses[j] = temp.Item1;
		}

		private string[] GetCurrentHints()  => IsReversed ? Hints : [.. Hints.Reverse()];

		// Utility: Check if two words differ by exactly one character
		public static bool IsValidTransition(string a, string b)
		{
			if (a.Length != b.Length) return false;
			int diff = 0;
			for (int i = 0; i < a.Length; i++)
				if (a[i] != b[i]) diff++;
			return diff == 1;
		}

		public static (string[] words, string[] hints) LoadWordsAndHints(string filename)
		{
			string[] lines = File.ReadAllLines(filename);
			if (lines.Length != 7)
				throw new Exception("File must contain exactly 7 lines (7 words/hints).");

			string[] words = new string[7];
			string[] hints = new string[7];
			for (int i = 0; i < 7; i++)
			{
				string[] parts = lines[i].Split("--", 2, StringSplitOptions.TrimEntries);
				if (parts.Length != 2)
					throw new Exception($"Line {i + 1} is not in 'WORD--hint' format.");
				words[i] = parts[0];
				hints[i] = parts[1];
			}
			return (words, hints);
		}

		// Returns (allCorrect, transitionsOk)
		private (bool, bool) CheckSolution()
		{
			bool allCorrect = true;
			bool transitionsOk = true;
			for (int i = 0; i < 7; i++)
			{
				if (!string.Equals(Guesses[i], Words[i], StringComparison.OrdinalIgnoreCase))
				{
					Console.WriteLine($"Row {i + 1}: Incorrect (guess: {Guesses[i]}, answer: {Words[i]})");
					allCorrect = false;
				}
			}
			for (int i = 0; i < 6; i++)
			{
				if (!string.IsNullOrWhiteSpace(Guesses[i]) && !string.IsNullOrWhiteSpace(Guesses[i + 1]))
				{
					if (!IsValidTransition(Guesses[i], Guesses[i + 1]))
					{
						Console.WriteLine($"Transition {i + 1}→{i + 2} is invalid: {Guesses[i]} → {Guesses[i + 1]}");
						transitionsOk = false;
					}
				}
				else
				{
					transitionsOk = false;
				}
			}
			return (allCorrect, transitionsOk);
		}
	}
}
