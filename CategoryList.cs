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
		["Mundane"] = new MaladyCategory
		{
			Name = "Mundane",
			PluralName = "Mundane"
		},
		["Supernatural"] = new MaladyCategory
		{
			Name = "Supernatural",
			PluralName = "Supernatural"
		},
		["Zaza"] = new MaladyCategory
		{
			Name = "Zaza",
			PluralName = "Zaza"
		}
	};
}
