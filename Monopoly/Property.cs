namespace Monopoly
{
	public class Property(string name, ConsoleColor color) : Tile(name)
	{
		public readonly ConsoleColor Color = color;
		public int Cost;

		public int HousesCount { get; private set; } = 0;

		public bool HasHotel { get; private set; } = false;

		public int Rent { get; private set; }

		public bool AddHouse()
		{
			if (HousesCount < 4)
			{
				HousesCount++;
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
			if (HousesCount == 4)
			{
				HousesCount = 0;
				HasHotel = true;
				return true;
			}
			else
			{
				Console.Error.WriteLine($"Property: {this} cannot be upgraded to a hotel!");
				return false;
			}
		}

		public override string ToString() => Name;
	}
}