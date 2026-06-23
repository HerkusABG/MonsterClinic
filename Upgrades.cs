using Godot;
using System;

static class Upgrades
{
	//Upgrades is used for tracking and managing
	//the upgrades of the game. All values related to
	//the progression system should be stored here.
	public static int roomCount { get; private set; } = 1;

	//boolean that control whether you can buy Aspirin
	public static bool AspirinUnlock = false;

	public static void AddNewRoom()
	{
		if (roomCount <= 6)
		{
			roomCount++;
			Economy.roomCost = (int)((float)Economy.roomCost * Economy.roomCostInflation);
		}
	}
	//unlock aspirin, pay for it
    public static void UnlockAspirin()
    {
            Upgrades.AspirinUnlock = true;
            DoctorInventory.Money -= 50;
    }

}
