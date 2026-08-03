using System;

namespace Monopoly
{
	internal class Card(string name, string icon, string description, EventType type, int value) : TileWithIcon(name, icon)
	{
		public readonly string Description = description;

		public readonly EventType Type = type;

		public readonly int Value = value;

		public override string ToString() => base.ToString() + $"{Environment.NewLine}{Description}";
	}
}