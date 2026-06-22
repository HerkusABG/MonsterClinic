using System;
using System.Collections.Generic;

public static class MaladyList

    //all the Maladies that we have (placeholders ofc)
{
    public static Dictionary<string, Malady> Database = new()
    {
        ["Accident"] = new Malady { 
            name = "Accident",
            dialogueSymptoms =
            {
                SymptomList.Database["BodyPain"],
                SymptomList.Database["HeartProblems"],
                SymptomList.Database["Headache"]
            } 
        },
        ["BluePox"] = new Malady 
        { 
            name = "Blue Pox",
            dialogueSymptoms =
            {
                SymptomList.Database["Fever"],
                SymptomList.Database["Sneezing"],
                SymptomList.Database["Headache"]
            }
        },
    };
}
