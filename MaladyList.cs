using System;
using System.Collections.Generic;

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
            },
            tags =
            {
                TagList.Database["Unstable"]
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
            },
            tags =
            {
                TagList.Database["Unstable"]
            }
        },
    };
}
