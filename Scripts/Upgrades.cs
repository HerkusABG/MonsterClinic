using Godot;
using System;
using System.Collections.Generic;
using System.Linq;


static class Upgrades
{
	//Upgrades is used for tracking and managing
	//the upgrades of the game. All values related to
	//the progression system should be stored here.
	public static int roomCount { get; private set; } = 1;

	//boolean that control whether you can buy Aspirin
	public static bool AspirinUnlock = false;

	public static Dictionary<string, IncrementalUpgrade> AllUpgrades = new()
    {
     
    };

    public static Dictionary<string, IntegerUpgrade> IntUpgradeDatabase = new()
	{
		["PatientSlots"] = new IntegerUpgrade
		{
			name = "More patient slots",
			incrementTarget = 3,
			cap = 6,
			price = 50
		},
        ["Rooms"] = new IntegerUpgrade
        {
            name = "More rooms to treat patients",
            incrementTarget = 1,
            cap = 6,
            price = 100
        }
    };

    public static Dictionary<string, BooleanUpgrade> BoolUpgradeDatabase = new()
    {
        ["RemoteMedicine"] = new BooleanUpgrade
        {
            name = "Remotely treat patients",
            unlocked = false,
            price = 200
        },
		["Aspirin"] = new BooleanUpgrade
        {
            name = "Be able to buy aspirin",
            unlocked = false,
            price = 200,
            medicine = MedicineManager.Database["Aspirin"]
        },
        ["Placeholder1"] = new BooleanUpgrade
        {
            name = "placeholder upgrade1",
            unlocked = false,
            price = 200
        },
        ["Placeholder2"] = new BooleanUpgrade
        {
            name = "placeholder upgrade2",
            unlocked = false,
            price = 200
        }
    };

	public static void Initialize()
	{
		for(int i = 0; i < IntUpgradeDatabase.Count; i++)
		{
			AllUpgrades.Add(IntUpgradeDatabase.ElementAt(i).Key, IntUpgradeDatabase.ElementAt(i).Value);
        }
        for (int i = 0; i < BoolUpgradeDatabase.Count; i++)
        {
            AllUpgrades.Add(BoolUpgradeDatabase.ElementAt(i).Key, BoolUpgradeDatabase.ElementAt(i).Value);
        }
        ResetAllUpgrades();
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

	public static void IntegerUpgrade(IntegerUpgrade upgrade, int loops, Button upgradeButton, Action successAction, Action failAction) 
	{
		if (DoctorInventory.Money >= upgrade.price)
		{
			for (int i = 1; i <= loops; i++)
			{
				//increment the count, spend the money
				upgrade.incrementTarget++;
				DoctorInventory.Money -= upgrade.price;

                if(upgrade.OnUpgradePressed != null)
                {
                    upgrade.OnUpgradePressed();
                }

				//if we reach the cap, disable the button
				if (upgrade.incrementTarget >= upgrade.cap)
				{
                    upgrade.fullyUnlocked = true;
                    //upgradeButton.Disabled = true;
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

	public static void BooleanUpgrade(BooleanUpgrade upgrade, Button upgradeButton, Action successAction, Action failAction)
	{
        if (DoctorInventory.Money >= upgrade.price)
        {
            //increment the count, spend the money
            upgrade.medicine.unlocked = true;
            upgrade.unlocked = true;
            upgrade.fullyUnlocked = true;
            DoctorInventory.Money -= upgrade.price;
            //upgradeButton.Disabled = true;
            successAction();
        }
        else
        {
            failAction();
        }
    }
    
	public static void ResetAllUpgrades()
	{
		IntegerUpgrade patientUpgrade = IntUpgradeDatabase["PatientSlots"];
        patientUpgrade.incrementTarget = 3;


        for(int i = 0; i < BoolUpgradeDatabase.Count; i++)
        {
            BooleanUpgrade boolUpgrade = BoolUpgradeDatabase.ElementAt(i).Value;
            boolUpgrade.unlocked = false;
            if(boolUpgrade.medicine != null)
            {
                boolUpgrade.medicine.unlocked = false;
            }
        }
		//BooleanUpgrade remoteUpgrade = BoolUpgradeDatabase["RemoteMedicine"];
		//remoteUpgrade.unlocked = false;
    }
}
