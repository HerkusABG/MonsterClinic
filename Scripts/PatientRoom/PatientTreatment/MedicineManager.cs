using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

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
			cost = 8,
			description = "Antibiotics, used for curing bla bla bla",
			texture = (Texture2D)ResourceLoader.Load("res://Assets/2DArt/inventory/inventory-medicine-antibiotics-normal.png"),
		},
		["Bandages"] = new Medicine
		{
			name = "Bandages",
			cost = 6,
			description = "Band together to cure a malady bla bla",
			texture = (Texture2D)ResourceLoader.Load("res://Assets/2DArt/inventory/inventory-medicine-bandages-normal.png"),

		},
		["Dewormer"] = new Medicine
		{
			name = "Dewormer",
			cost = 13,
			description = "Highly toxic to any worms. Harmless to humans, mostly.\n Reduces Severity of Bone Crawler by 1.",
			texture = (Texture2D)ResourceLoader.Load("res://Assets/2DArt/inventory/inventory-medicine-dewormer.png"),
		},
		["PrussianBlue"] = new Medicine
		{
			name = "Prussian Blue",
			description = "Binds to and flushes out various toxic and radioactive compounds. \n Reduces Severity of Radiation Sickness by 1.",
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
			cost = 15,
			description = "Common garden chemical used for killing wild mushrooms. \n Reduces Severity of Flesh Fungus by 1.",
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
			description = "Syringe with a radioactively shielded reservoir \n Reduces Severity of The Glow by 1.",
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
			description = "An IV drip filled with toxic chemicals. \n Reduces Severity of Tumours by 1.",
			texture = (Texture2D)ResourceLoader.Load("res://Assets/2DArt/inventory/inventory-medicine-chemdrip.png"),
		},
		["Curitol"] = new Medicine
		{
			name = "Curitol",
			cost = 30,
			description = "A highly experimental wonder drug. \n Reduces Severity of Every malady by 1.",
			texture = (Texture2D)ResourceLoader.Load("res://Assets/2DArt/inventory/inventory-medicine-curitol.png"),
		}
	};

	public static void Initialize()
	{
		SaveCatalogueInfo();
	}
	public static void SaveCatalogueInfo()
	{
		for (int i = 0; i < Database.Count; i++)
		{
			CatalogueInfoPackage package = new CatalogueInfoPackage(Database.ElementAt(i).Value);
			InfoList.Medicines.Add(package);
		}
	}
}
