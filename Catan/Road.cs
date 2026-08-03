namespace Catan
{
	public class Road(Player owner)
	{
		public readonly Player Owner = owner;

		public override string ToString() => "🛣️";
	}
}