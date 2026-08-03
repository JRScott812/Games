namespace Monopoly
{
	internal abstract class TileWithIcon(string name, string icon) : Tile(name)
	{
		public readonly string Icon = icon;
		public override string ToString() => $"{Name} {Icon}";
	}
}
