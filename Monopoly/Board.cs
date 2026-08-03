using System;
using System.Collections.Generic;

namespace Monopoly
{
	internal class Board
	{
		public Board()
		{
			// Shuffle the order of the Chance and Community Chest cards at the start of the game
			Random random = new();

			Span<Card> tempChanceCards = [ ..ChanceCards];
			Span<Card> tempCommunityChestCards = [.. CommunityChestCards];

			random.Shuffle(tempChanceCards);
			random.Shuffle(tempCommunityChestCards);

			ChanceCards = [.. tempChanceCards.ToArray()];
			CommunityChestCards = [.. tempCommunityChestCards.ToArray()];
		}

		public int FreeParkingAmount { get; private set; } = 200;

		public readonly Tile[] Tiles = [
			new Event("GO", "➡️", 200, EventType.Charity),
			new Property("Mediterranean Avenue", ConsoleColor.DarkGray, 60),
			new DrawCard(false),
			new Property("Baltic Avenue", ConsoleColor.DarkGray, 60),
			new Event("Income Tax", "💰", -200, EventType.Tax),
			new Railroad("Reading Railroad"),
			new Property("Oriental Avenue", ConsoleColor.Cyan, 100),
			new DrawCard(true),
			new Property("Vermont Avenue", ConsoleColor.Cyan, 100),
			new Property("Connecticut Avenue", ConsoleColor.Cyan, 120),

			new Event("Jail", "🚓", 0, EventType.Jail),
			new Property("St. Charles Place", ConsoleColor.DarkMagenta, 140),
			new Utility(true),
			new Property("States Avenue", ConsoleColor.DarkMagenta, 140),
			new Property("Virginia Avenue", ConsoleColor.DarkMagenta, 160),
			new Railroad("Pennsylvania Railroad"),
			new Property("St. James Place", ConsoleColor.DarkYellow, 180),
			new DrawCard(false),
			new Property("Tennessee Avenue", ConsoleColor.DarkYellow, 180),
			new Property("New York Avenue", ConsoleColor.DarkYellow, 200),

			new Event("Free Parking", "🅿️", 200, EventType.Charity),
			new Property("Kentucky Avenue", ConsoleColor.Red, 220),
			new DrawCard(true),
			new Property("Indiana Avenue", ConsoleColor.Red, 220),
			new Property("Illinois Avenue", ConsoleColor.Red, 240),
			new Railroad("B&O Railroad"),
			new Property("Atlantic Avenue", ConsoleColor.Yellow, 260),
			new Property("Ventnor Avenue", ConsoleColor.Yellow, 260),
			new Utility(false),
			new Property("Marvin Gardens", ConsoleColor.Yellow, 280),

			new Event("Go To Jail", "👮", 0, EventType.Jail),
			new Property("Pacific Avenue", ConsoleColor.Green, 300),
			new Property("North Carolina Avenue", ConsoleColor.Green, 300),
			new DrawCard(false),
			new Property("Pennsylvania Avenue", ConsoleColor.Green, 320),
			new Railroad("Short Line Railroad"),
			new DrawCard(true),
			new Property("Park Place", ConsoleColor.Blue, 350),
			new Event("Luxury Tax", "💎", -100, EventType.Tax),
			new Property("Boardwalk", ConsoleColor.Blue, 400)
		];

		public readonly List<Card> ChanceCards = [
			new("Advance to Illinois Ave.", "🚶‍➡️", string.Empty, EventType.Move, 0),
			new("Advance to the nearest Railroad", "➡️🚂", "Pay the owner 2x the rent\tIf unowned, you may buy it.", EventType.Other, 0),
			new("Go Back 3 Spaces", "⬅️3x", string.Empty, EventType.Move, 0),
			new("Advance to Nearest Utility", "➡️💡/🚰", "If unowned, you may but it\tIf owned, throw dice and pay the owner 10x the roll.", EventType.Other, 0),
			new("Advance to St. Charles Place", "➡️", "If You Pass GO, Collect $200.", EventType.Move, 0),
			new("Take A Walk on the Boardwalk", "➡️🌞", "Advance token to Boardwalk", EventType.Move, 0),
			new("Bank Pays You Dividend", "🏦", string.Empty, EventType.Charity, 50),
			new("Take a Ride on the Reading Railroad", "🚂", "If you pass GO collect $200.", EventType.Move, 0),
			new("Pay Poor Tax", "☹️", string.Empty, EventType.Tax, -15),
			new("Get Out of Jail Free", "⛓️‍💥", "This card may be kept until needed or sold.", EventType.Other, 0),
			new("You Have Been Elected Chairman of the Board", "📰🗳️", "Pay each player $50.", EventType.Other, 0),
			new("Advance to GO", "➡️GO", "(Collect $200)", EventType.Move, 0),
			new("Make General Repairs on Your Properties", "🛠️🏚️", "Pay $40 for each house\t$100 for each hotel", EventType.Other, 0),
			new("Advance to Nearest Railroad", "➡️🚂", "Pay owner 2x rent\tIf unowned, you may buy it.", EventType.Move, 0),
			new("Your Building & Loan Matures", "💰", string.Empty, EventType.Charity, 150),
			new("Go To Jail", "👮", "Do not pass GO\tDo not collect $200", EventType.Other, 0),
		];

		public readonly List<Card> CommunityChestCards = [
			new("Life Insurance Matures", "🧓", string.Empty, EventType.Charity, 100),
			new("Receive for Services", "👰‍♀️🤵‍♂️", string.Empty, EventType.Charity, 25),
			new("Doctor's Fee", "🩼🤕", string.Empty, EventType.Tax, -50),
			new("Life Insurance Matures", "🧓", string.Empty, EventType.Charity, 100),
			new("You Inherit", "🤑", string.Empty, EventType.Charity, 100),
			new("Bank Error in Your Favor", "", string.Empty, EventType.Charity, 200),
			new("You Pay School Tax", "", string.Empty, EventType.Tax, -150),
			new("You Have Won 2nd Place in a Beauty Contest", "🥈💐", string.Empty, EventType.Charity, 10),
			new("Pay Hospital", "👩‍⚕️🧑‍🍼", string.Empty, EventType.Tax, -100),
			new("Christmas Fund Matures", "🎄", string.Empty, EventType.Charity, 100),
			new("Go to Jail", "👮", "Go Directly to Jail\tDo Not Pass Go\tDo Not Collect $200", EventType.Jail, 0),
			new("Get Out of Jail Free", "⛓️‍💥", "This card may be kept until needed or traded", EventType.Other, 0),
			new("Grand Opera Opening", "🧓", "Collect $50 from every player (for opening night seats)", EventType.Charity, 50),
			new("Advance to GO", "➡️", "(Collect $200)", EventType.Move, 0),
			new("Income Tax Refund", "🧾", string.Empty, EventType.Charity, 20),
			new("You Are Assessed for Street Repairs", "⛏️🪏", "Pay $40 for each house\t$115 for each hotel", EventType.Tax, -40),
		];

		public void Display(Player[] Players)
		{
			for (int i = 0; i < Tiles.Length; i++)
			{
				if (Tiles[i] is Property property)
				{
					Console.ForegroundColor = property.Color;
				}

				string tileDisplay = Tiles[i].ToString();
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

			Console.WriteLine();
		}

		public static void GetCard(Player player, Card card)
		{
			Console.WriteLine(card);
			player.ReceivePayment(card.Value);
		}
	}
}
