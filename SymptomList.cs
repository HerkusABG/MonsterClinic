using Godot;
using System;
using System.Collections.Generic;
public static class SymptomList
{
    public static Dictionary<string, Symptom> Database = new()
    {
        ["BodyPain"] = new Symptom
        {
            name = "Body Pain",
            quotes =
            {
                "Doc, you have to help me, everything hurts so much!",
                "It's not normal for things to ache when you breathe, right?"
            }
        },
        ["Headache"] = new Symptom
        {
            name = "Headache",
            quotes =
            {
                "My head is splitting. Please do something before I die!",
                "Please, just take my headache away, or take me out of my misery..."
            }
        },
        ["Sneezing"] = new Symptom
        {
            name = "Sneezing",
            quotes =
            {
                "I can't stop sneezing and I can feel my lungs giving up. Can you fix me?",
                "You're a - ATCHOO - doctor right? Do you have anything to - ATCHOO - help me?"
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
        }
    };
}
