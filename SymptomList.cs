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
                "It's not normal for things\n to ache when you breathe, right?"
            }
        },
        ["Headache"] = new Symptom
        {
            name = "Headache",
            quotes =
            {
                "My head is splitting. Please do something before I die!",
                "Please, just take my headache away,\n or take me out of my misery..."
            }
        },
        ["Sneezing"] = new Symptom
        {
            name = "Sneezing",
            quotes =
            {
                "I can't stop sneezing and I can feel\n my lungs giving up. Can you fix me?",
                "You're a - ATCHOO - doctor right?\n Do you have anything to - ATCHOO - help me?"
            }
        },
        ["HeartProblems"] = new Symptom
        {
            name = "Heart Problems",
            quotes =
            {
                "Their heart rate is all over the place."
            }
        },
        ["Fever"] = new Symptom
        {
            name = "Fever",
            quotes =
            {
                "I could fry an egg on them."
            }
        },
        ["Vomitting"] = new Symptom
        {
            name = "Vomitting",
            quotes =
            {
                "I am constantly throwing up. \n I can't even drink water."
            }
        },
        ["Vertigo"] = new Symptom
        {
            name = "Vertigo",
            quotes =
            {
                "Everything is spinning, my balance is WAY off."
            }
        },
        ["SkinPeel"] = new Symptom
        {
            name = "Peeling Skin",
            quotes =
            {
                "My skin is coming off of my body. \n I am really scared, please help me!"
            }
        },
        ["Delirious"] = new Symptom
        {
            name = "Delirious",
            quotes =
            {
                "*The patient is unintelligible*"
            }
        },
        ["Paralysis"] = new Symptom
        {
            name = "Paralysis",
            quotes =
            {
                "I woke up and my left all the way up to \n my shoulder was asleep. \n Nothing like this has happened before."
            }
        }
    };
}
