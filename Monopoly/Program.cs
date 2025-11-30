using System.Text;

namespace Monopoly
{
	internal class Program
	{
		static void Main()
		{
			Console.OutputEncoding = Encoding.UTF8;
			Console.WriteLine($"Welcome to Monopoly!{Environment.NewLine}🎩{Environment.NewLine}🤑");

			int numberOfPlayers = Game.PromptForInput<int>("Enter number of Players");

			Game game = new(numberOfPlayers);
			game.SetupGame();
			game.DisplayBoard();
		}
	}
}
