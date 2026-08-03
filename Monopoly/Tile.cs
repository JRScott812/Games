namespace Monopoly
{
	public abstract class Tile(string name)
	{
		public readonly string Name = name;

		public override string ToString() => Name;
	}
}