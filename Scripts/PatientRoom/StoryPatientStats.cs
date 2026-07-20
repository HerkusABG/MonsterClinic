using Godot;
using System;
using System.Linq;

public partial class StoryPatientStats : PatientStats
{
    public string name;
    public bool arrived;
    public string entrySpeech;

    public StoryPatientStats()
    {
        patientID += " (Story)";
        arrived = false;
        Random rnd = new Random();
        malady = new Malady();
        isAlive = true;
    }
}
