namespace Monopoly
{
	public class Card(string name, string icon, string description)
	{
		public readonly string Name = name;
		public readonly string Icon = icon;
		public readonly string Description = description;

		public override string ToString() => $"{Name}: {Icon}{Environment.NewLine}{Description}";
	}
}