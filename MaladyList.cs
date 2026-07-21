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
        ["Injury"] = new Malady {
            name = "Injury",
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
                TagList.Database["Deadly"].Clone()
            },
            cures =
            {
                MedicineManager.Database["Morphine"],
                MedicineManager.Database["Bandages"]
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
                TagList.Database["Worsening"]
            },
            cures =
            {
                MedicineManager.Database["Antibiotics"],
                MedicineManager.Database["FancyAntibiotics"]
            }
        },
        ["GreenPox"] = new Malady
        {
            name = "Green Pox",
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
                TagList.Database["Deadly"],
                TagList.Database["Worsening"]
            },
            cures =
            {
                MedicineManager.Database["Antibiotics"],
                MedicineManager.Database["FancyAntibiotics"]
            }
        },
        ["Sthyricoids"] = new Malady
        {
            name = "Sthyricoids",
            dialogueSymptoms =
            {
                SymptomList.Database["Vertigo"],
                SymptomList.Database["Paralysis"]
            },
            temperatureSymptoms =
            {
                SymptomList.Database["HeartProblems"]
            },
            allSymptoms =
            {
                SymptomList.Database["Vertigo"].name,
                SymptomList.Database["Paralysis"].name,
                SymptomList.Database["HeartProblems"].name
            },
            tags =
            {
                TagList.Database["Resistant"],
                TagList.Database["Healing"]
            },
            cures =
            {
                MedicineManager.Database["Aptomitol"],
                MedicineManager.Database["Morphine"]
            }
        },
        ["SoliderGut"] = new Malady
        {
            name = "Soldier's Gut",
            dialogueSymptoms =
            {
                SymptomList.Database["Vertigo"],
                SymptomList.Database["Vomitting"]
            },
            allSymptoms =
            {
                SymptomList.Database["Vertigo"].name,
                SymptomList.Database["Vomitting"].name,
            },
            tags =
            {
                TagList.Database["Unstable"],
                TagList.Database["WeakHealing"]
            },
            cures =
            {
                MedicineManager.Database["FancyAntibiotics"],
                MedicineManager.Database["Antibiotics"]
            }
        },
        ["Slithic"] = new Malady
        {
            name = "Slithic",
            dialogueSymptoms =
            {
                SymptomList.Database["Vomitting"],
                SymptomList.Database["SkinPeel"]
            },
            temperatureSymptoms =
            {
                SymptomList.Database["Fever"]
            },
            allSymptoms =
            {
                SymptomList.Database["Vomitting"].name,
                SymptomList.Database["SkinPeel"].name,
                SymptomList.Database["Fever"].name
            },
            tags =
            {
                TagList.Database["Deadly"],
                TagList.Database["StrongWorsening"],
                TagList.Database["Unstable"]
            },
            cures =
            {
                MedicineManager.Database["Aptomitol"],
                MedicineManager.Database["Phranax"]
            }
        }
    };
}
