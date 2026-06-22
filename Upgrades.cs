using Godot;
using System;

static class Upgrades
{
	//Upgrades is used for tracking and managing
	//the upgrades of the game. All values related to
	//the progression system should be stored here.
	public static int roomCount { get; private set; } = 1;

	public static void AddNewRoom()
	{
		roomCount++;
		Economy.roomCost = (int)((float)Economy.roomCost * Economy.roomCostInflation);
    }
}
