using Godot;
using System;
using System.Collections.Generic;
using System.Linq;


public static class DealerList
{
    public static Dictionary<string, DealerSlot> Database = new()
    {
       /* ["Nothing"] = new DealerSlot
        {
            name = "",
        }*/
    };

    public static void Initialize()
    {
        int index = 0;
        for(int i = 0; i < MedicineManager.Database.Count; i++)
        {
            //GD.Print(MedicineManager.Database.ElementAt(i).Key);
            if (MedicineManager.Database.ElementAt(i).Value.buyable)
            {
                DealerSlot newSlot = new DealerSlot(MedicineManager.Database.ElementAt(i).Value, index);
                Database.Add(MedicineManager.Database.ElementAt(i).Key, newSlot);
                index++;
            }
        }
    }
}
