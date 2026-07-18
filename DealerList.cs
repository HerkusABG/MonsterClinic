using Godot;
using System;
using System.Collections.Generic;
using System.Linq;


public static class DealerList
{
    public static Dictionary<string, DealerSlot> MedicineDatabase = new()
    {
      
    };

    public static Dictionary<string, DealerSlot> UpgradeDatabase = new()
    {

    };

    public static void Initialize()
    {
        int index = 0;
        for(int i = 0; i < MedicineManager.Database.Count; i++)
        {
            if (MedicineManager.Database.ElementAt(i).Value.buyable)
            {
                DealerSlot newSlot = new DealerSlot(MedicineManager.Database.ElementAt(i).Value, index);
                MedicineDatabase.Add(MedicineManager.Database.ElementAt(i).Key, newSlot);
                index++;
            }
        }

        index = 0;
        for (int i = 0; i < Upgrades.AllUpgrades.Count; i++)
        {
            DealerSlot newSlot = new DealerSlot(Upgrades.AllUpgrades.ElementAt(i).Value, index);
            UpgradeDatabase.Add(Upgrades.AllUpgrades.ElementAt(i).Key, newSlot);
            index++;
        }
    }
}
