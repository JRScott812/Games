using System;
using System.ComponentModel;
using System.Linq;

namespace Monopoly
{

	internal class Game(int numberOfPlayers)
	{
		public static readonly string[] PlayerIcons = ["🎩", "🪣", "🏎️", "🚢", "🥾", "🔫", "🪙", "👛"];

		public static readonly Random random = new();

		public readonly Player[] Players = new Player[numberOfPlayers];

		public Board Board { get; private set; } = new();

		public int Turns { get; private set; } = 0;
		public int CurrentPlayerTurn => Turns / 4;
		public Player CurrentPlayer => Players[CurrentPlayerTurn];

		public static int RollDie() => random.Next(1, 7);
		public static (int, int) RollDice() => (RollDie(), RollDie());

		public void SetupGame()
		{
			for (int i = 0; i < Players.Length; i++)
			{
				bool repeatedName = false;
				string name;
				do
				{
					name = PromptForInput<string>($"Enter name for Player #{i + 1}");
					if (Array.Exists(Players, p => p != null && p.Name == name))
					{
						Console.WriteLine("Name already taken. Please choose a different name.");
						repeatedName = true;
					}
					else
					{
						repeatedName = false;
						break;
					}
				} while (repeatedName == true);

				bool repeatedIcon = false;
				string icon;
				do
				{
					for (int j = 0; j < PlayerIcons.Length; j++)
					{
						Console.WriteLine($"{j + 1}: {PlayerIcons[j]}");
					}

					int iconIndex = PromptForInput<int>($"Select icon for Player #{i + 1}: {name}");
					icon = PlayerIcons[iconIndex - 1];

					if (Array.Exists(Players, p => p != null && p.Icon == icon))
					{
						Console.WriteLine("Icon already taken. Please choose a different icon.");
						repeatedIcon = true;
					}
					else
					{
						repeatedIcon = false;
						break;
					}
				} while (repeatedIcon == true);

				Players[i] = new Player(name, icon);
				Console.Clear();
			}
		}

		#region Display
		public void DisplayPlayers()
		{
			Player[] rankings = [.. Players.OrderByDescending(p => p.Money)];

			for (int i = 0; i < rankings.Length; i++)
			{
				Player player = rankings[i];
				Console.WriteLine($"{i + 1}: {player.Name} {player.Icon} --> ${player.Money}");
			}
		}

		public void Display()
		{
			DisplayPlayers();
			Console.WriteLine();
			Board.Display(Players);
		}
		#endregion

		public void TakeTurn(Player player)
		{
			int speedingCount = 0;
			while (speedingCount < 3)
			{
				(int die1, int die2) = RollDice();
				int totalValue = die1 + die2;

				Console.WriteLine($"{player.Name} rolled a {die1} and a {die2} (Total: {totalValue})\t(Pairs in a row: {speedingCount})");

				if (die1 == die2)
				{
					speedingCount++;
				}

				if (speedingCount >= 3)
				{
					player.PutInJail();
					return;
				}

				player.Advance(totalValue);
				Tile currentTile = Board.Tiles[player.Position];

				if (currentTile is Property property)
				{
					// Determine who owns the Tile
					Player? owner = null;
					foreach (Player p in Players)
					{
						if (p.OwnedProperties.Contains(currentTile))
						{
							owner = p;
							break;
						}
					}

					if (owner != null && owner != player)
					{
						PayRent(player, property, owner);
					}
					else if (owner == null)
					{
						bool wantsToBuy = PromptForInput<bool>($"Do you want to buy {property.Name} for ${property.Cost}? (true/false)");
						if (wantsToBuy && player.Money >= property.Cost)
						{
							_ = player.ReceivePayment(-property.Cost);
							player.OwnedProperties.Add(property);
							Console.WriteLine($"{player.Name} bought {property.Name}!");
						}
					}
				}
			}

			Turns++;
		}

		public void Main()
		{
			while (true)
			{
				Console.WriteLine($"It's {CurrentPlayer.Name}'s turn! {CurrentPlayer.Icon}");
				TakeTurn(CurrentPlayer);
				Display();
			}
		}

		public static T PromptForInput<T>(string prompt)
		{
			while (true)
			{
				Console.Write(prompt + ": ");
				string? input = Console.ReadLine();

				if (string.IsNullOrWhiteSpace(input))
				{
					Console.WriteLine("Input cannot be empty. Try again.");
					continue;
				}

				try
				{
					// Use TypeDescriptor to convert string to type T
					TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
					if (converter != null && converter.IsValid(input))
					{
						return (T)converter.ConvertFromString(input)!;
					}
					else
					{
						Console.WriteLine($"Cannot convert input to {typeof(T).Name}. Try again.");
					}
				}
				catch
				{
					Console.WriteLine($"Invalid {typeof(T).Name}. Please try again.");
				}
			}
		}

		public static void PayRent(Player player, Property property, Player owner)
		{
			if (!player.OwnedProperties.Contains(property))
			{
				_ = player.PayRent(property, owner);
			}
		}
	}
}
