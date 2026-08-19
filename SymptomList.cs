using Godot;
using System;
using System.Collections.Generic;
public static class SymptomList
{
	public static Dictionary<string, Symptom> Database = new()
	{
		["Nothing"] = new Symptom
		{
			name = "",
			quotes =
			{
                "..."
			}
		},
		["BodyPain"] = new Symptom
		{
			name = "Body Pain",
			quotes =
			{
				"Doc, you have to help me, everything hurts so much!",
                "It's not normal for...everything\n to ache when you breathe, right?"
			}
		},
		["Headache"] = new Symptom
		{
			name = "Headache",
			quotes =
			{
				"Agh, head is splitting. It feels like it's going to explode",
                "Please, just take my headache away,\n or take me out of my misery..."
			}
		},
		["Sneezing"] = new Symptom
		{
			name = "Sneezing",
			quotes =
			{
				"I can't stop sneezing and I can feel\n my lungs giving out. Can you fix me?",
                "You're a - ATCHOO - doctor right?\n Do you have anything to - ATCHOO - help me?"
			}
		},
		["HeartProblems"] = new Symptom
		{
			name = "Heart Problems",
			quotes =
			{
				"Their heart rate is all over the place.",
				"Sounds less like a steady beat and more like a dying car."
			}
		},
		["Fever"] = new Symptom
		{
			name = "Fever",
			quotes =
			{
				"So warm I could fry an egg on them.",
				"How haven't they melted into a puddle already?"
			}
		},
		["Nausea"] = new Symptom
		{
			name = "Nausea",
			quotes =
			{
				"I am constantly throwing up. \n I can't even drink water.",
				"Urgh, do you have a bucket I could use for a moment?"
			}
		},
		["Dizziness"] = new Symptom
		{
			name = "Dizziness",
			quotes =
			{
				"Everything is spinning, my balance is WAY off.",
				"If the room doesn't stop swaying I'm going to fall over."
			}
		},
		["SkinPeel"] = new Symptom
		{
			name = "Peeling Skin",
			quotes =
			{
				"My skin is flaking off like crazy. \n I swear I don't even sunbathe.",
				"Ew, why is my skin trying to imitate a snake?"
			}
		},
		["Delirious"] = new Symptom
		{
			name = "Delirious",
			quotes =
			{
				"Excuse me I'm not talking to you, \n I'm talking to the little man on your shoulder.",
				"I'll take a number four with large fries and a milkshake!"
			}
		},
		["Paralysis"] = new Symptom
		{
			name = "Paralysis",
			quotes =
			{
				"I woke up today and I couldn't move a muscle. \n It was like something was holding me down.",
				
			}
		}
	};
}
