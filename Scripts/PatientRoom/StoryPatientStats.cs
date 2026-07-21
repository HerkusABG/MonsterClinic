using Godot;
using System;
using System.Linq;

//am extension of the patientStats class, with additions for story patients
public partial class StoryPatientStats : PatientStats
{
    public string name;
    public string entrySpeech;

    //little method for adding data common to all story patients
    public void AddInfo()
    {
        patientID += " (Story)";
    }

    public void StoryPatientSetup(int index)
    {
        StoryPatientStats patient = StoryPatientList.Database.ElementAt(index).Value;
        
        // refresh the patient's data.
        // For just assigning random numbers, this will be overhauled later.
        Random rnd = new Random();
        malady = new Malady();
        AssignMaladyValues(patient.malady);
        if (malady.severity == -1)
        {
            malady.severity = rnd.Next(3, 5);
        }
        isAlive = true;
        patientID = patient.patientID;//  "D3" writes the ID as a 3-digit string  005 
        age = patient.age; // random ages of patients between 18 and 90 seemed appropriate for the game
        entrySpeech = patient.entrySpeech;
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
        malady = inputMalady.Clone();
    }
}
