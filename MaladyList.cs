using System;
using System.Collections.Generic;
using System.Linq;

public static class MaladyList

	//all the Maladies that we have
	//The first title is used in code. For the actual name reference you should use the "name"
	//field instead. Refer to the class references for further information on implementation.
{
	public static Dictionary<string, Malady> Database = new()
	{
		["Nothing"] = new Malady
		{
			name = "",
			description = "",
			category = CategoryList.Database["Nothing"],
			payout = 0,
            passiveIncome = 10,
            dialogueSymptoms =
			{
				SymptomList.Database["Nothing"],
			},
			pulseSymptoms =
			{
				SymptomList.Database["Nothing"]
			},
			temperatureSymptoms =
			{
				SymptomList.Database["Nothing"]
			},
			allSymptoms =
			{
				SymptomList.Database["Nothing"].name,
			}
		},
		["Accident"] = new Malady {
			name = "Accident",
			description = "If you are physically injured, your wounds will heal over time given time to rest after being stabilised, and should be dressed with fresh, clean antiseptic bandages to prevent further complications. \n - @IDC_Official",
			category = CategoryList.Database["Mundane"],
            payout = 50,
            passiveIncome = 10,
            dialogueSymptoms =
			{
				SymptomList.Database["BodyPain"],
				SymptomList.Database["Headache"]
			},
			pulseSymptoms =
			{
				SymptomList.Database["HeartProblems"]
			},
			allSymptoms =
			{
				SymptomList.Database["HeartProblems"].name,
				SymptomList.Database["BodyPain"].name,
				SymptomList.Database["Headache"].name
			},
			tags =
			{
				TagList.Database["Deadly"].Clone(),
				TagList.Database["Healing"].Clone(),
				TagList.Database["Unstable"].Clone()
			},
			cures =
			{
				MedicineManager.Database["Bandages"],
				MedicineManager.Database["Curitol"]
			},
			admittedDialogue =
			{
				"These bandages are so itchy",
                "Ouch."
			},
			layerType = TextureType.Normal,
			visualKey = "BrokenBones",
		},
		["BluePox"] = new Malady
		{
			name = "Blue Pox",
			description = "`A highly infectious and irritating mundane bacterial infection. Presents with flu-like symptoms and patches of blue spots. Treated with responsibly administered antibiotics. \n - @IDC_Official",
			category = CategoryList.Database["Mundane"],
            payout = 75,
            passiveIncome = 80,
            dialogueSymptoms =
			{
				SymptomList.Database["Sneezing"],
				SymptomList.Database["Headache"]
			},
			temperatureSymptoms =
			{
				SymptomList.Database["Fever"]
			},
			allSymptoms =
			{
				SymptomList.Database["Sneezing"].name,
				SymptomList.Database["Headache"].name,
				SymptomList.Database["Fever"].name
			},
			tags =
			{
				TagList.Database["Worsening"].Clone(),
				TagList.Database["Deadly"].Clone()
			},
			cures =
			{
				MedicineManager.Database["Antibiotics"],
				MedicineManager.Database["Curitol"]
			},
			admittedDialogue =
			{
				"I wish I'd gotten my blu shot this year",
                "Cough cough I hate blue pox"
			},
			layerType = TextureType.Normal,
			visualKey = "BluePox",
		},
		["TheGlow"] = new Malady
		{
			name = "The Glow",
			description = "The Glow causes patient bodies to start producing radioactive slush beneath their skin. This slush can be safely lanced and disposed of in lead syringes until the symptoms abate.\n - @IDC_Official",
			category = CategoryList.Database["Supernatural"],
            payout = 100,
            passiveIncome = 90,
            dialogueSymptoms =
			{
				SymptomList.Database["SkinPeel"],
				SymptomList.Database["Headache"]
			},
			temperatureSymptoms =
			{
				SymptomList.Database["Fever"]
			},
			allSymptoms =
			{
				SymptomList.Database["Delirious"].name,
				SymptomList.Database["SkinPeel"].name,
				SymptomList.Database["Fever"].name
			},
			tags =
			{
				TagList.Database["Deadly"].Clone(),
				TagList.Database["Worsening"].Clone()
			},
			cures =
			{
				MedicineManager.Database["LeadSyringe"],
				MedicineManager.Database["Curitol"]
			},
			admittedDialogue =
			{
				"Why does everything look so green?",
                "I feel like I swallowed a microwave."
			},
			layerType = TextureType.Normal,
			visualKey = "Radiation",
		},
		["RadiationSickness"] = new Malady
		{
			name = "Radiation Sickness",
			description = "Avoid exposure to or ingestion of radioactive materials. If exposed, Prussian Blue may be used to flush some radioactive compounds from the body. \n - @IDC_Official",
			category = CategoryList.Database["Mundane"],
            payout = 150,
            passiveIncome = 100,
            dialogueSymptoms =
			{
				SymptomList.Database["SkinPeel"],
				SymptomList.Database["Vomitting"]
			},
			pulseSymptoms =
			{
				SymptomList.Database["HeartProblems"]
			},
			allSymptoms =
			{
				SymptomList.Database["SkinPeel"].name,
				SymptomList.Database["Vomitting"].name,
				SymptomList.Database["HeartProblems"].name
			},
			tags =
			{
				TagList.Database["Resistant"].Clone(),
				TagList.Database["Deadly"].Clone(),
				TagList.Database["Healing"].Clone()
			},
			cures =
			{
				MedicineManager.Database["PrussianBlue"],
				MedicineManager.Database["Curitol"]
			},
			admittedDialogue =
			{
				"I swear I didn't mean to drop that screwdriver",
                "Worst. Sunburn. Ever."
			},
			layerType = TextureType.Normal,
			visualKey = "Necrosis",
		},
		["BoneCrawler"] = new Malady
		{
			name = "BoneCrawler",
			description = "HELPMEGETITOUTGETITOUTGETITOUTGETITOUTGE \n - @UnknownResearcher420",
			category = CategoryList.Database["Supernatural"],
            payout = 100,
            passiveIncome = 50,
            dialogueSymptoms =
			{
				SymptomList.Database["Vertigo"],
				SymptomList.Database["Vomitting"]
			},
			pulseSymptoms =
			{
				SymptomList.Database["HeartProblems"]
			},
			allSymptoms =
			{
				SymptomList.Database["Vertigo"].name,
				SymptomList.Database["Vomitting"].name,
				SymptomList.Database["HeartProblems"].name
			},
			tags =
			{
				TagList.Database["Unstable"].Clone(),
				TagList.Database["Deadly"].Clone(),
				TagList.Database["WeakHealing"].Clone()
			},
			cures =
			{
				MedicineManager.Database["Dewormer"],
				MedicineManager.Database["Curitol"]
			},
			admittedDialogue =
			{
				"I can feel it crawling up my spine...!",
                "This is a very unpleasant sensation."
			},
			layerType = TextureType.Normal,
			visualKey = "Centipede",
		},
		["Tumours"] = new Malady
		{
			name = "Tumours",
			description = "Malignant growths of flesh, swelling up across the body. Can be eliminated through drips of concentrated toxic chemicals. \n - @IDC_Official",
			category = CategoryList.Database["Mundane"],
            payout = 120,
            passiveIncome = 70,
            dialogueSymptoms =
			{
				SymptomList.Database["BodyPain"],
				SymptomList.Database["Paralysis"]
			},
			temperatureSymptoms =
			{
				SymptomList.Database["Fever"]
			},
			allSymptoms =
			{
				SymptomList.Database["BodyPain"].name,
				SymptomList.Database["Paralysis"].name,
				SymptomList.Database["Fever"].name
			},
			tags =
			{
				TagList.Database["Deadly"].Clone(),
				TagList.Database["Resistant"].Clone(),
				TagList.Database["Unstable"].Clone()
			},
			cures =
			{
				MedicineManager.Database["ChemDrip"],
				MedicineManager.Database["Curitol"]
			},
			admittedDialogue =
			{
				"There are so many lumps under my skin.",
                "I'm gonna call the biggest one Jimothy."
			},
			layerType = TextureType.Normal,
			visualKey = "Tumors",
		},
		["PolyporusAnthropophilum"] = new Malady
		{
			name = "Polyporus Anthropophilum",
			description = "A beautiful fungus species that grows on otherwise boring human bodies. If infected, avoid antifungal medicines and make sure you listen to the voices telling you to climb up to high places! <3 - @Fun_Gal",
			category = CategoryList.Database["Mundane"],
            payout = 90,
            passiveIncome = 80,
            dialogueSymptoms =
			{
				SymptomList.Database["Delirious"],
				SymptomList.Database["Sneezing"]
			},
			pulseSymptoms =
			{
				SymptomList.Database["HeartProblems"]
			},
			allSymptoms =
			{
				SymptomList.Database["Delirious"].name,
				SymptomList.Database["Sneezing"].name,
				SymptomList.Database["HeartProblems"].name
			},
			tags =
			{
				TagList.Database["Deadly"].Clone(),
				TagList.Database["WeakWorsening"].Clone(),
				TagList.Database["Resistant"].Clone()
			},
			cures =
			{
				MedicineManager.Database["FungalPowder"],
				MedicineManager.Database["Curitol"]
			},
			layerType = TextureType.Top,
			visualKey = "Mushrooms",
		}
	};

	public static List<Malady> GetAllMaladiesOfType(MaladyCategory category)
	{
		List<Malady> outputList = new List<Malady>();
		foreach(KeyValuePair<string, Malady> malady in Database)
		{
			if(malady.Value.HasThisCategory(category))
			{
				outputList.Add(malady.Value);
			}
		}
		return outputList;
	}
	public static void Initialize()
	{
		SaveCatalogueInfo();
    }
	public static void SaveCatalogueInfo()
	{
		for (int i = 0; i < Database.Count; i++)
		{
			CatalogueInfoPackage package = new CatalogueInfoPackage(Database.ElementAt(i).Value);
            InfoList.Maladies.Add(package);
        }
	}


}
