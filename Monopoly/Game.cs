using System;
using System.ComponentModel;

namespace Monopoly
{

	internal class Game(int numberofPlayers)
	{
		public static readonly string[] PlayerIcons = ["🎩", "🪣", "🏎️", "🚢", "🥾", "🔫", "🪙", "👛"];

		public static readonly Random random = new();

		public readonly Tile[] Board = [
			new Event("GO", "➡️", 200, Event.EventType.Charity),
			new Property("Mediterranean Avenue", ConsoleColor.DarkGray),
			new Event("Community Chest", "🎁", 0, Event.EventType.CommunityChest),
			new Property("Baltic Avenue", ConsoleColor.DarkGray),
			new Event("Income Tax", "💰", -200, Event.EventType.Tax),
			new Utility("Reading Railroad", "🚂"),
			new Property("Oriental Avenue", ConsoleColor.Cyan),
			new Event("Chance", "❓", 0, Event.EventType.Chance),
			new Property("Vermont Avenue", ConsoleColor.Cyan),
			new Property("Connecticut Avenue", ConsoleColor.Cyan),

			new Event("Jail", "🚓", 0, Event.EventType.Jail),
			new Property("St. Charles Place", ConsoleColor.DarkMagenta),
			new Utility("Electric Company", "💡"),
			new Property("States Avenue", ConsoleColor.DarkMagenta),
			new Property("Virginia Avenue", ConsoleColor.DarkMagenta),
			new Utility("Pennsylvania Railroad", "🚂"),
			new Property("St. James Place", ConsoleColor.DarkYellow),
			new Event("Community Chest", "🎁", 0, Event.EventType.CommunityChest),
			new Property("Tennessee Avenue", ConsoleColor.DarkYellow),
			new Property("New York Avenue", ConsoleColor.DarkYellow),

			new Event("Free Parking", "🅿️", 200, Event.EventType.Charity),
			new Property("Kentucky Avenue", ConsoleColor.Red),
			new Event("Chance", "❓", 0, Event.EventType.Chance),
			new Property("Indiana Avenue", ConsoleColor.Red),
			new Property("Illinois Avenue", ConsoleColor.Red),
			new Utility("B&O Railroad", "🚂"),
			new Property("Atlantic Avenue", ConsoleColor.Yellow),
			new Property("Ventnor Avenue", ConsoleColor.Yellow),
			new Utility("Water Works", "🚰"),
			new Property("Marvin Gardens", ConsoleColor.Yellow),

			new Event("Go To Jail", "👮", 0, Event.EventType.Jail),
			new Property("Pacific Avenue", ConsoleColor.Green),
			new Property("North Carolina Avenue", ConsoleColor.Green),
			new Event("Community Chest", "🎁", 0, Event.EventType.CommunityChest),
			new Property("Pennsylvania Avenue", ConsoleColor.Green),
			new Utility("Short Line Railroad", "🚂"),
			new Event("Chance", "❓", 0, Event.EventType.Chance),
			new Property("Park Place", ConsoleColor.Blue),
			new Event("Luxury Tax", "💎", -100, Event.EventType.Tax),
			new Property("Boardwalk", ConsoleColor.Blue)
		];

		public List<Card> ChanceCards = [
			
		];
		public List<Card> CommunityChestCards = [

		];

		public readonly Player[] Players = new Player[numberofPlayers];

		public int Turns { get; private set; } = 0;
		public int CurrentPlayerTurn => Turns / 4;

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
				Tile currentTile = Board[player.Position];

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
							player.ReceivePayment(-property.Cost);
							player.OwnedProperties.Add(property);
							Console.WriteLine($"{player.Name} bought {property.Name}!");
						}
					}
				}
			}
			Turns++;
		}

		public void DisplayBoard()
		{
			for (int i = 0; i < Board.Length; i++)
			{
				if (Board[i] is Property property)
				{
					Console.ForegroundColor = property.Color;
				}
				string tileDisplay = Board[i].ToString();
				foreach (Player player in Players)
				{
					if (player.Position == i)
					{
						tileDisplay += ' ' + player.Icon;
					}
				}
				Console.WriteLine(tileDisplay);
				Console.ResetColor();
			}

			string playersKey = "Players Key: ";
			foreach (Player player in Players)
			{
				playersKey += $"{player.Name} = {player.Icon}";
			}
			Console.WriteLine();
			Console.WriteLine(playersKey);
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

		public void PayRent(Player player, Property property, Player owner)
		{
			if (!player.OwnedProperties.Contains(property))
			{
				player.PayRent(property, owner);
			}
		}
	}
}
