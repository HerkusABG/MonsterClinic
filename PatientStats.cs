using Godot;
using System;
using System.Linq;

public partial class PatientStats
{
    // This class is used for storing the patient's data inside of the patient admission interface.
    // This will later be plugged in a way where this gets instantiated every time there is a new patient to be admitted.
    // The relevant stats will be changed according to the game designer's wishes.
    // This data will then be used for diagnosis, once we develop that further.

    //Patients ID
    public string patientID;
    public int age;
    public Color PortraitColor;

    // Also defining a bool that tracks if the patient is alive, in case he gets SHOT
    public bool isAlive;

    public Malady malady;

    public PatientStats()
    {
        // refresh the patient's data.
        // For just assigning random numbers, this will be overhauled later.
        Random rnd = new Random();
        malady = new Malady();
        AssignMaladyValues(MaladyList.Database.ElementAt(rnd.Next(1, MaladyList.Database.Count)).Value);
        if (malady.severity == -1)
        {
            malady.severity = rnd.Next(2, 5);
        }
        if(malady.severity >= 5)
        {
            isAlive = false;
        }
        else
        {
            isAlive = true;
        }
        patientID = rnd.Next(1, 1000).ToString("D3");//  "D3" writes the ID as a 3-digit string  005 
        age = rnd.Next(18, 91); // random ages of patients between 18 and 90 seemed appropriate for the game

        // Assigning a random color to the patient's portrait, This will be changed later when we have actual portraits.
        PortraitColor = new Color(
            (float)rnd.NextDouble(),
            (float)rnd.NextDouble(),
            (float)rnd.NextDouble()
        );
    }

    private void AssignMaladyValues(Malady inputMalady)
    {
        //ALL malady related information must go through here,
        //otherwise the malady reference is static and curing one patient cures all patients.
        malady.name = inputMalady.name;
        malady.allSymptoms = inputMalady.allSymptoms;
        malady.dialogueSymptoms = inputMalady.dialogueSymptoms;
        malady.temperatureSymptoms = inputMalady.temperatureSymptoms;
        malady.pulseSymptoms = inputMalady.pulseSymptoms;
    }

    public string GetDialogue()
    {
        //Grab generic dialogue.
        if(malady.dialogueSymptoms.Count > 0)
        {
            string returnDialogue = malady.dialogueSymptoms[0].quotes[0];
            return returnDialogue;
        }
        return "...";
    }

    public string GetPulse()
    {
        //Grab stethoscope dialogue
        if (malady.pulseSymptoms.Count > 0)
        {
            string returnDialogue = malady.pulseSymptoms[0].quotes[0];
            return returnDialogue;
        }
        return "A nice steady rhythm.";
    }

    public string GetTemperature()
    {
        //Grab temperature dialogue
        if (malady.temperatureSymptoms.Count > 0)
        {
            string returnDialogue = malady.temperatureSymptoms[0].quotes[0];
            return returnDialogue;
        }
        return "Not too hot, not too cold!";
    }

    public bool IsPatientAlive()
    {
        return isAlive;
    }
}
    

   