namespace Monopoly
{
	public class Player(string name, string icon)
	{
		public readonly string Name = name;
		public readonly string Icon = icon;

		public int Position => TotalDistance % 40;
		public int TotalDistance { get; private set; } = 0;

		public List<Tile> OwnedProperties { get; private set; } = [];

		public bool IsInJail { get; private set; } = false;
		public bool HasGetOutOfJailCard { get; private set; } = false;

		public int Money { get; private set; } = 1500;

		/// <summary>
		/// Put the player in jail.
		/// </summary>
		public void PutInJail()
		{
			throw new System.NotImplementedException();
		}

		public void Advance(int moveDistance)
		{
			TotalDistance += moveDistance;
		}

		public bool PayRent(Property property, Player owner)
		{
			if (Money >= property.Rent)
			{
				Money -= property.Rent;
				owner.ReceivePayment(property.Rent);
				return true;
			}
			else
			{
				// Handle insufficient funds (e.g., bankruptcy)
				Console.Error.WriteLine($"Player: {Name} cannot afford rent of {property.Rent:$}");
				return false;
			}
		}

		public bool ReceivePayment(int amount)
		{
			if (Money >= 0)
			{
				Money += amount;
				return true;
			}
			else
			{
				Console.Error.WriteLine($"Player: {Name} cannot receive negative payment of ${amount:$}!");
				return false;
			}
		}

		public bool BuyProperty(Property property)
		{
			if (Money >= property.Cost)
			{
				Money -= property.Cost;
				OwnedProperties.Add(property);
				Console.WriteLine($"Player: {Name} bought the property: {property}");
				return true;
			}
			else
			{
				Console.Error.WriteLine($"Player: {Name} cannot afford the property: {property}");
				return false;
			}
		}
	}
}