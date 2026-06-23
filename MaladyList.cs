using System;
using System.Collections.Generic;

public static class MaladyList

    //all the Maladies that we have (placeholders ofc)
{
    public static Dictionary<string, Malady> Database = new()
    {
        ["Nothing"] = new Malady
        {
            name = "",
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
            name = "an accident",
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
            },
            allSymptoms =
            {
                SymptomList.Database["Sneezing"].name,
                SymptomList.Database["Headache"].name,
                SymptomList.Database["Fever"].name
            }
        },
    };
}
