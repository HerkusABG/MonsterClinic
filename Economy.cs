using Godot;
using System;

static class Economy
{
	//Economy is used for storing prices for everything other than
	//medicine. Variables which reference the price itself, as well
	//as any values related to it should be stored here.

	public static int roomCost = 10;
	public static float roomCostInflation = 1.5f;

	//should theoretically increase the cost by 50% everytime
	public static void IncreaseRoomCost()
	{
		roomCost = (int)(roomCost * roomCostInflation);
	}
}
