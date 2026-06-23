using Godot;
using System;
using System.Linq;

public partial class PatientStats
{
    // This class is used for storing the patient's data inside of the patient admission interface.
    // This will later be plugged in a way where this gets instantiated every time there is a new patient to be admitted.
    // The relevant stats will be changed according to the game designer's wishes.
    // This data will then be used for diagnosis, once we develop that further.

    // Defining the values which will be used for diagnosis.
    public int heartRate;
    public int skinStatus;

    //Patients ID
    public string patientID;
    public int age;
    public Color PortraitColor;

    // Also defining a bool that tracks if the patient is alive, in case he gets SHOT
    public bool isAlive;

    public Malady malady;

    public PatientStats()
    {
        PatientInitialize();
    }

    public string GetDialogue()
    {
        if(malady.dialogueSymptoms.Count > 0)
        {
            string returnDialogue = malady.dialogueSymptoms[0].quotes[0];
            return returnDialogue;
        }
        return "...";
    }

    public string GetPulse()
    {
        if (malady.pulseSymptoms.Count > 0)
        {
            string returnDialogue = malady.pulseSymptoms[0].quotes[0];
            return returnDialogue;
        }
        return "A nice steady rhythm.";
    }

    public string GetTemperature()
    {
        if (malady.temperatureSymptoms.Count > 0)
        {
            string returnDialogue = malady.temperatureSymptoms[0].quotes[0];
            return returnDialogue;
        }
        return "Not too hot, not too cold!";
    }
    private void PatientInitialize()
	{
        // refresh the patient's data.
        // For just assigning random numbers, this will be overhauled later.
        isAlive = true;
        Random rnd = new Random();
        malady = MaladyList.Database.ElementAt(rnd.Next(1, MaladyList.Database.Count)).Value;
        malady.severity = rnd.Next(2, 5);

        heartRate = rnd.Next(50, 151);  
        skinStatus = rnd.Next(1, 6);
        patientID = rnd.Next(1, 1000).ToString("D3");//  "D3" writes the ID as a 3-digit string  005 
        age = rnd.Next(18, 91); // random ages of patients between 18 and 90 seemed appropriate for the game

        // Assigning a random color to the patient's portrait, This will be changed later when we have actual portraits.
        PortraitColor = new Color(
            (float)rnd.NextDouble(), 
            (float)rnd.NextDouble(), 
            (float)rnd.NextDouble()
        );
    }
}
    

   