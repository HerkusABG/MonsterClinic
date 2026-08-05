using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public static class TextureList
{
	public static Dictionary<string, TextureSet> BodyTextures = new()
	{
		/*["Body1"] = new TextureSet()
		{
			sitting = GD.Load<Texture2D>("res://Assets/2DArt/Patient/Sitting/Body/patient-sitting-body1.png"),
			standing = GD.Load<Texture2D>("res://Assets/2DArt/Patient/Standing/patient-standing-body1.png")
		},
		["Body2"] = new TextureSet()
		{
			sitting = GD.Load<Texture2D>("res://Assets/2DArt/Patient/Sitting/Body/patient-sitting-body2.png"),
			standing = GD.Load<Texture2D>("res://Assets/2DArt/Patient/Standing/patient-standing-body2.png")
		},
		["Body3"] = new TextureSet()
		{
			sitting = GD.Load<Texture2D>("res://Assets/2DArt/Patient/Sitting/Body/patient-sitting-body3.png"),
			standing = GD.Load<Texture2D>("res://Assets/2DArt/Patient/Standing/patient-standing-body3.png")
		}*/
	};

	public static Dictionary<string, TextureSet> HeadTextures = new()
	{
	};

	public static Dictionary<string, TextureSet> MaladyTextures = new()
	{
		["Lycanthropy"] = new TextureSet()
		{
			sitting = GD.Load<Texture2D>("res://Assets/2DArt/Patient/Sitting/NormalSymptoms/patient-sitting-symptom-animaltransform.png"),
			standing = GD.Load<Texture2D>("res://Assets/2DArt/Patient/Standing/NormalSymptoms/patient-standing-symptom-animaltransform.png")
		},
		["BluePox"] = new TextureSet()
		{
			sitting = GD.Load<Texture2D>("res://Assets/2DArt/Patient/Sitting/NormalSymptoms/patient-sitting-symptom-bluepox.png"),
			standing = GD.Load<Texture2D>("res://Assets/2DArt/Patient/Standing/NormalSymptoms/patient-standing-symptom-bluepox.png")
		},
		 ["BrokenBones"] = new TextureSet()
		{
			sitting = GD.Load<Texture2D>("res://Assets/2DArt/Patient/Sitting/NormalSymptoms/patient-sitting-symptom-brokenbones.png"),
			 standing = GD.Load<Texture2D>("res://Assets/2DArt/Patient/Standing/NormalSymptoms/patient-standing-symptom-brokenbones.png")
		 },
		["Centipede"] = new TextureSet()
		{
			sitting = GD.Load<Texture2D>("res://Assets/2DArt/Patient/Sitting/NormalSymptoms/patient-sitting-symptom-centipede.png"),
			standing = GD.Load<Texture2D>("res://Assets/2DArt/Patient/Standing/NormalSymptoms/patient-standing-symptom-centipede.png")
		},
		["CultMarkings"] = new TextureSet()
		{
			sitting = GD.Load<Texture2D>("res://Assets/2DArt/Patient/Sitting/NormalSymptoms/patient-sitting-symptom-cultmarkings.png"),
			standing = GD.Load<Texture2D>("res://Assets/2DArt/Patient/Standing/NormalSymptoms/patient-standing-symptom-cultmarkings.png")
		},
		["Leeches"] = new TextureSet()
		{
			sitting = GD.Load<Texture2D>("res://Assets/2DArt/Patient/Sitting/NormalSymptoms/patient-sitting-symptom-leeches.png"),
			standing = GD.Load<Texture2D>("res://Assets/2DArt/Patient/Standing/NormalSymptoms/patient-standing-symptom-leeches.png")
		},
		["Necrosis"] = new TextureSet()
		{
			sitting = GD.Load<Texture2D>("res://Assets/2DArt/Patient/Sitting/NormalSymptoms/patient-sitting-symptom-necrosis.png"),
			standing = GD.Load<Texture2D>("res://Assets/2DArt/Patient/Standing/NormalSymptoms/patient-standing-symptom-necrosis.png")
		},
		["Radiation"] = new TextureSet()
		{
			sitting = GD.Load<Texture2D>("res://Assets/2DArt/Patient/Sitting/NormalSymptoms/patient-sitting-symptom-radiation.png"),
			standing = GD.Load<Texture2D>("res://Assets/2DArt/Patient/Standing/NormalSymptoms/patient-standing-symptom-radiation.png")
		},
		["Shattered"] = new TextureSet()
		{
			sitting = GD.Load<Texture2D>("res://Assets/2DArt/Patient/Sitting/NormalSymptoms/patient-sitting-symptom-shattered.png"),
			standing = GD.Load<Texture2D>("res://Assets/2DArt/Patient/Standing/NormalSymptoms/patient-standing-symptom-shattered.png")
		},
		["Tumors"] = new TextureSet()
		{
			sitting = GD.Load<Texture2D>("res://Assets/2DArt/Patient/Sitting/NormalSymptoms/patient-sitting-symptom-tumors.png"),
			standing = GD.Load<Texture2D>("res://Assets/2DArt/Patient/Standing/NormalSymptoms/patient-standing-symptom-tumors.png")
		}
	};

	public static Dictionary<string, TextureSet> TopMaladyTextures = new()
	{
		["Mushrooms"] = new TextureSet()
		{
			sitting = GD.Load<Texture2D>("res://Assets/2DArt/Patient/Sitting/TopSymptoms/patient-sitting-symptom-mushrooms.png"),
			standing = GD.Load<Texture2D>("res://Assets/2DArt/Patient/Standing/TopSymptoms/patient-standing-symptom-mushrooms.png")
		},
		["Portal"] = new TextureSet()
		{
			sitting = GD.Load<Texture2D>("res://Assets/2DArt/Patient/Sitting/TopSymptoms/patient-sitting-symptom-portal.png"),
			standing = GD.Load<Texture2D>("res://Assets/2DArt/Patient/Standing/TopSymptoms/patient-standing-symptom-portal.png")
		}
	};

	public static Dictionary<string, Texture2D> CivilianOutfits = new()
	{
		["Clothing1"] = GD.Load<Texture2D>("res://Assets/2DArt/Patient/Standing/Outfits/patient-standing-clothes1.png"),
		["Clothing2"] = GD.Load<Texture2D>("res://Assets/2DArt/Patient/Standing/Outfits/patient-standing-clothes2.png"),
		["Clothing3"] = GD.Load<Texture2D>("res://Assets/2DArt/Patient/Standing/Outfits/patient-standing-clothes3.png"),
		["Clothing4"] = GD.Load<Texture2D>("res://Assets/2DArt/Patient/Standing/Outfits/patient-standing-clothes4.png"),
		["Clothing5"] = GD.Load<Texture2D>("res://Assets/2DArt/Patient/Standing/Outfits/patient-standing-clothes5.png"),
		["Clothing6"] = GD.Load<Texture2D>("res://Assets/2DArt/Patient/Standing/Outfits/patient-standing-clothes6.png"),
		["Clothing7"] = GD.Load<Texture2D>("res://Assets/2DArt/Patient/Standing/Outfits/patient-standing-clothes7.png"),
		["Clothing8"] = GD.Load<Texture2D>("res://Assets/2DArt/Patient/Standing/Outfits/patient-standing-clothes8.png")
	};

	public static void Initialize()
	{
		//LoadTextures();
		LoadDesiredTexture("res://Assets/2DArt/Patient/Standing/Body/patient-standing-body",
			"res://Assets/2DArt/Patient/Sitting/Body/patient-sitting-body",
			"Body",
			3,
			BodyTextures);
		LoadDesiredTexture("res://Assets/2DArt/Patient/Standing/Head/patient-standing-head",
			"res://Assets/2DArt/Patient/Sitting/Head/patient-sitting-head",
			"Head",
			48,
			HeadTextures);
	}

	private static void LoadDesiredTexture(string standingFilepath, string sittingFilepath, string keyName, int count, Dictionary<string, TextureSet> dictionary)
	{
		//string path = filepath;
		for (int i = 0; i < count; i++)
		{
			int index = i + 1;
			string key = "keyName" + (index);

			TextureSet set = new TextureSet()
			{
				standing = GD.Load<Texture2D>(standingFilepath + index + ".png"),
				sitting = GD.Load<Texture2D>(sittingFilepath + index + ".png")
			};
			dictionary.Add(key, set);
		}
	}

	public static TextureSet GetRandomSet(Dictionary<string, TextureSet> dictionary)
	{
		int length = dictionary.Count;
		Random rnd = new Random();
		int randIndex = rnd.Next(0, length);
		return dictionary.ElementAt(randIndex).Value;
	}

	public static Texture2D GetRandomTexture(Dictionary<string, Texture2D> dictionary)
	{
		int length = dictionary.Count;
		Random rnd = new Random();
		int randIndex = rnd.Next(0, length);
		return dictionary.ElementAt(randIndex).Value;
	}
}
