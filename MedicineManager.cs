using Godot;
using System;
using System.Collections.Generic;

static class MedicineManager
{
    public static Dictionary<string, Medicine> Database = new()
    {
        ["Morphine"] = new Medicine { name = "Morphine", cost = 11 },
        ["Aspirin"] = new Medicine { name = "Aspirin", cost = 25 },
        ["Ozempic"] = new Medicine { name = "Ozempic", cost = 36 }
    };
}
