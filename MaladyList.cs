using System;
using System.Collections.Generic;

public static class MaladyList

    //all the Maladies that we have (placeholders ofc)
{
    public static Dictionary<string, Malady> Database = new()
    {
        ["Accident"] = new Malady { name = "Accident" },
        ["BluePox"] = new Malady { name = "Blue Pox"},
    };
}
