using Godot;
using System;
using System.Collections.Generic;


static class Upgrades
{
	//Upgrades is used for tracking and managing
	//the upgrades of the game. All values related to
	//the progression system should be stored here.
	public static int roomCount { get; private set; } = 1;

	//boolean that control whether you can buy Aspirin
	public static bool AspirinUnlock = false;

	public static IntegerUpgrade newPatientSlots = new IntegerUpgrade
	{
		incrementTarget = 3,
		cap = 999,
		price = 50
	};

    public static BooleanUpgrage remoteMedicine = new BooleanUpgrage
    {
        unlocked = false,
        price = 200
    };

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

	public static void IntegerUpgrade(IntegerUpgrade upgrade, int loops, Button upgradeButton, Action successAction, Action failAction) 
	{
		if (DoctorInventory.Money >= upgrade.price)
		{
			for (int i = 1; i <= loops; i++)
			{
				//increment the count, spend the money
				upgrade.incrementTarget++;
				DoctorInventory.Money -= upgrade.price;
				//if we reach the cap, disable the button
				if (upgrade.incrementTarget >= upgrade.cap)
				{
					upgradeButton.Disabled = true;
					break;
				}
			}
			successAction();
        } 
		else
		{
			failAction();
		}
	}

	public static void BooleanUpgrade(BooleanUpgrage upgrade, Button upgradeButton, Action successAction, Action failAction)
	{
        if (DoctorInventory.Money >= upgrade.price)
        {
            //increment the count, spend the money
            upgrade.unlocked = true;
            DoctorInventory.Money -= upgrade.price;
            upgradeButton.Disabled = true;
            successAction();
        }
        else
        {
            failAction();
        }
    }
    
	public static void ResetAllUpgrades()
	{
		newPatientSlots.incrementTarget = 3;
		remoteMedicine.unlocked = false;
    }
}
