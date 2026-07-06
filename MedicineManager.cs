using Godot;
using System;
using System.Collections.Generic;

static class MedicineManager
{
    //The medicine manager is a static class responsible for defining
    //all the different types of medicine available.
    //Imagine this script as a database for all the available types of medicine.
    //This can also easily be expanded, it is a modular system.
    //In order to access individual members, type MedicineManager.Database["<Insert medicine name here>"]
    public static Dictionary<string, Medicine> Database = new()
    {
        ["Morphine"] = new Medicine { name = "Morphine", cost = 10 },
        ["Aspirin"] = new Medicine { name = "Aspirin", cost = 15 },
        ["Ozempic"] = new Medicine { name = "Ozempic", cost = 25 }
    };
}
