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
            description = "",
            category = CategoryList.Database["Nothing"],
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
            description = "A physical injury that leaves the patient weak while they recover from the damage. " +
            "To your knowledge you know that Bandages would definitly help the patient, but the dealer" +
            " has also in his shop fancy bandages and the god medicine as an option for dealing with them. " +
            "There is the risk of further damaging them, do you take it?",
            category = CategoryList.Database["Injury"],
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
            description = "A highly infectious virus that presents with cold-like " +
            "symptoms and purplish spots in areas on the patient´s skin. " +
            "Some Doctors noticed that Aspirin, Antibiotics, fany Antibiotics " +
            "and God medicine has affects on the patient, on how bad or good t" +
            "hese are isnt noted",
            category = CategoryList.Database["Virus"],
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
            description = "A cousin strain of the Blue Pox, well known for ravaging communities in [REDACTED], " +
            "luckily most cases can be treated with Antibiotics.",
            category = CategoryList.Database["Virus"],
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
            description = "A condition caught by people who have breathed in low-quality air for an extended period of time, " +
            "making bloodflow to the limbs and brain difficult. Can be treated with Aptomitol and Morphine.",
            category = CategoryList.Database["Injury"],
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
            description = "Named after the soldier who were among the first to be afflicted by it, " +
            "soldier's gut refers to an airborne virus which shuts down the digestive system in mere hours. " +
            "Antibiotics usually help, but the virus is known to be unpredictable.",
            category = CategoryList.Database["Injury"],
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
            description = "A rare and incredibly deadly virus which can do irreperable damage to a person's body in mere days. " +
            "Usually treated with Aptomitol and Phranax",
            category = CategoryList.Database["Virus"],
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

    public static void Googa()
    {

    }
}
