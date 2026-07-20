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
        isAlive = true;
    }
}
