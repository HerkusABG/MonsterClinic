using Godot;
using System;

public partial class PatientStats : Node
{
    // This class is used for storing the patient's data inside of the patient admission interface.
    // This will later be plugged in a way where this gets instantiated every time there is a new patient to be admitted.
    // The relevant stats will be changed according to the game designer's wishes.
    // This data will then be used for diagnosis, once we develop that further.

    // Defining the values which will be used for diagnosis.
    public int heartRate;
    public int skinStatus;
    public string dialogue = "Hello I am a patient";

    //Patients ID
    public string PatientID;
    public int Age;
    public Color PortraitColor;

    // Also defining a bool that tracks if the patient is alive, in case he gets SHOT
    public bool isAlive;
    public void PatientInitialize()
	{
        // refresh the patient's data.
        // For just assigning random numbers, this will be overhauled later.
        isAlive = true;
        Random rnd = new Random();
        heartRate = rnd.Next(50, 151);  
        skinStatus = rnd.Next(1, 6);
        PatientID = rnd.Next(1, 1000).ToString("D3");//  "D3" writes the ID as a 3-digit string  005 
        Age = rnd.Next(18, 91); // random ages of patients between 18 and 90 seemed appropriate for the game

        // Assigning a random color to the patient's portrait, This will be changed later when we have actual portraits.
        PortraitColor = new Color(
            (float)rnd.NextDouble(), 
            (float)rnd.NextDouble(), 
            (float)rnd.NextDouble()
        );
    }
}
    

   