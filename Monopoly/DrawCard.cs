namespace Monopoly
{
	internal class DrawCard(bool isChance) : TileWithIcon(isChance ? "Chance" : "Community Chest", isChance ? "❓" : "🧰")
	{

	}
}
