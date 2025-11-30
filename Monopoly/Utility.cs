namespace Monopoly
{
	public class Utility(string name, string icon) : Tile(name)
	{
		public readonly string Icon = icon;

		public override string ToString() => $"{Name}: {Icon}";
	}
}