namespace Monopoly
{
	internal class Event(string name, string icon, int cost, EventType eventType) : TileWithIcon(name, icon)
	{
		public readonly int Cost = cost;

		public readonly EventType eventType = eventType;
	}
}