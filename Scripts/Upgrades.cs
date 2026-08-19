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
			name = "Waiting Room Space",
			incrementTarget = 3,
			cap = 6,
			price = 40,
			description = "Increase the amount of patients that can appear at the beginning of each day."
		},
		["NewRooms"] = new IntegerUpgrade
		{
			name = "Repair a room for more patients treatment",
			incrementTarget = 1,
			cap = 6,
			price = 30,
			description = "Purchase an additional patient room so that more patients can be admitted at a time."
		}
	};

	public static Dictionary<string, BooleanUpgrade> BoolUpgradeDatabase = new()
	{
		["Phranax"] = new BooleanUpgrade
		{
			name = "Unlock Curitol",
			unlocked = false,
			price = 100,
			medicine = MedicineManager.Database["Curitol"],
			description = "Unlock CURITOL for purchase, will can cure every malady."
		},
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
   
	//unlock aspirin, pay for it
	public static void UnlockAspirin()
	{
		Upgrades.AspirinUnlock = true;
		DoctorInventory.Money -= 50;
	}

	public static void IntegerUpgrade(IntegerUpgrade upgrade, int loops, TextureButton upgradeButton, Action successAction, Action failAction) 
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

	public static void BooleanUpgrade(BooleanUpgrade upgrade, TextureButton upgradeButton, Action successAction, Action failAction)
	{
		if (DoctorInventory.Money >= upgrade.price)
		{
			//increment the count, spend the money
			if(upgrade.medicine != null)
			{
				upgrade.medicine.unlocked = true;
			}
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
