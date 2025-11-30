namespace Monopoly
{
	public class Event(string name, string icon, int cost, Event.EventType eventType) : Tile(name)
	{
		public readonly string Icon = icon;
		public readonly int Cost = cost;

		public readonly EventType eventType = eventType;

		public override string ToString() => $"{Name}: {Icon}";

		public enum EventType
		{
			Tax,
			Charity,
			Bonus,
			Fine,
			CommunityChest,
			Chance,
			Jail,
		}
	}
}