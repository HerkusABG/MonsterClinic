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
                SymptomList.Database["Headache"]
            },
            pulseSymptoms =
            {
                SymptomList.Database["HeartProblems"]
            }
        },
        ["BluePox"] = new Malady 
        { 
            name = "Blue Pox",
            dialogueSymptoms =
            {
                SymptomList.Database["Sneezing"],
                SymptomList.Database["Headache"]
            },
            temperatureSymptoms =
            {
                SymptomList.Database["Fever"]
            }
        },
    };
}
