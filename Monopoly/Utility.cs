namespace Monopoly
{
	internal class Utility(bool isElectric) : TileWithIcon(isElectric ? "Electric Company" : "Water Works", isElectric ? "💡" : "🚰")
	{

	}
}