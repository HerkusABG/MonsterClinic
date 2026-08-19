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
			cost = 15,
			description = "Antibiotics, used for curing bla bla bla",
			texture = (Texture2D)ResourceLoader.Load("res://Assets/2DArt/inventory/inventory-medicine-antibiotics-normal.png"),
        },
		["Bandages"] = new Medicine
		{
			name = "Bandages",
			cost = 8,
            description = "Band together to cure a malady bla bla",
			texture = (Texture2D)ResourceLoader.Load("res://Assets/2DArt/inventory/inventory-medicine-bandages-normal.png"),

        },
		["Dewormer"] = new Medicine
		{
			name = "Dewormer",
			cost = 17,
            description = "De-worm your patient",
			texture = (Texture2D)ResourceLoader.Load("res://Assets/2DArt/inventory/inventory-medicine-dewormer.png"),
        },
		["PrussianBlue"] = new Medicine
		{
			name = "Prussian Blue",
            description = "Konigsberger Klopse",
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
            description = "Foot fungus number 15 bla bla",
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
            description = "Lead syringe description",
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
            description = "Chemical drip. Drippy aye aye",
			texture = (Texture2D)ResourceLoader.Load("res://Assets/2DArt/inventory/inventory-medicine-chemdrip.png"),
        },
		["Curitol"] = new Medicine
		{
			name = "Curitol",
			cost = 100,
            description = "Cure EVERYTHING!",
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
