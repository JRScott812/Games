using System;

namespace Monopoly
{
	internal class Property(string name, ConsoleColor color, int cost) : Tile(name)
	{
		public readonly ConsoleColor Color = color;
		public readonly int Cost = cost;

		public HouseStatus Houses { get; private set; } = HouseStatus.NoHouses;

		public int Rent => Cost / 10 * (int)Houses;

		public bool AddHouse()
		{
			if ((int)Houses < 4)
			{
				Houses++;
				return true;
			}
			else
			{
				Console.Error.WriteLine($"Property: {this} cannot have more than 4 houses!");
				return false;
			}
		}

		public bool UpgradeToHotel()
		{
			if ((int)Houses == 4)
			{
				Houses++;
				return true;
			}
			else
			{
				Console.Error.WriteLine($"Property: {this} cannot be upgraded to a hotel!");
				return false;
			}
		}

		public override string ToString() => base.ToString() + $"\t${Cost}";

		public enum HouseStatus
		{
			NoHouses = 0,
			OneHouse = 1,
			TwoHouses = 2,
			ThreeHouses = 3,
			FourHouses = 4,
			Hotel = 5
		}
	}
}