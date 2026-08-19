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
		["Antibiotics"] = new Medicine
		{
			name = "Antibiotics",
			cost = 15,
			texture = (Texture2D)ResourceLoader.Load("res://Assets/2DArt/inventory/inventory-medicine-antibiotics-normal.png"),
        },
		["Bandages"] = new Medicine
		{
			name = "Bandages",
			cost = 8,
			texture = (Texture2D)ResourceLoader.Load("res://Assets/2DArt/inventory/inventory-medicine-bandages-normal.png"),

        },
		["Dewormer"] = new Medicine
		{
			name = "Dewormer",
			cost = 17,
			texture = (Texture2D)ResourceLoader.Load("res://Assets/2DArt/inventory/inventory-medicine-dewormer.png"),
        },
		["PrussianBlue"] = new Medicine
		{
			name = "Prussian Blue",
			cost = 20,
			texture = (Texture2D)ResourceLoader.Load("res://Assets/2DArt/inventory/inventory-medicine-prussianblue.png"),
        },
//		["SilverDrops"] = new Medicine
//		{
//			name = "Silver Drops",
//			cost = 10,
//		},
		["FungalPowder"] = new Medicine
		{
			name = "Fungal Powder",
			cost = 22,
			texture = (Texture2D)ResourceLoader.Load("res://Assets/2DArt/inventory/inventory-medicine-antifungalpowder.png"),

        },
//		["OsmiumCapsules"] = new Medicine
//		{
//			name = "Osmium Capsules",
//			cost = 10,
//		},
//		["Incense"] = new Medicine
//		{
//			name = "Incense",
//			cost = 10,
//		},
		["LeadSyringe"] = new Medicine
		{
			name = "Lead Syringe",
			cost = 12,
			texture = (Texture2D)ResourceLoader.Load("res://Assets/2DArt/inventory/inventory-medicine-leadsyringe.png"),
        },
//		["MeatGlue"] = new Medicine
//		{
//			name = "Meat Glue",
//			cost = 10,
//		},
		["ChemDrip"] = new Medicine
		{
			name = "Chem Drip",
			cost = 16,
			texture = (Texture2D)ResourceLoader.Load("res://Assets/2DArt/inventory/inventory-medicine-chemdrip.png"),
        },
		["Curitol"] = new Medicine
		{
			name = "Curitol",
			cost = 100,
			texture = (Texture2D)ResourceLoader.Load("res://Assets/2DArt/inventory/inventory-medicine-curitol.png"),
        }
	};
}
