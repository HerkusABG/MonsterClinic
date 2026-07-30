using Godot;
using System;
using System.Collections.Generic;

public static class CategoryList
{
	public static Dictionary<string, MaladyCategory> Database = new()
	{
        ["Nothing"] = new MaladyCategory
        {
        },
        ["Virus"] = new MaladyCategory
        {
            Name = "Virus",
            PluralName = "Viruses"
        },
        ["Injury"] = new MaladyCategory
        {
            Name = "Injury",
            PluralName = "Physical Injuries"
        }
    };
}
