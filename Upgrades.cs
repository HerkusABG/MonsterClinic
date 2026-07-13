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
	public static int newPatientSlots { get; private set; } = 3;

	public static bool RemoteMedicineUnlock { get; private set; } = false;

	//resets room count to one, should be used when starting a new game, else there's some issues
	public static void NewGameRoomReset()
	{
		roomCount = 1;
	}

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

	//unlock remote medicine use, pay for it
	public static void UnlockRemoteMedicine()
	{
		RemoteMedicineUnlock = true;
		DoctorInventory.Money -= 200;
	}

}
