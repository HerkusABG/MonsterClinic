using Godot;
using System;
using System.Collections.Generic;

public static class StoryPatientList
{
    public static int StoryPatientsLeft = Database.Count;

    public static Dictionary<string, StoryPatientStats> Database = new()
    {
        ["Jimbo"] = new StoryPatientStats
        {
            name = "Jimbo the Diseased",
            entrySpeech = "HI, I'm Jimbo the Diseased, and you'll never guess how my health is holding up",
            patientID = "1001",
            age = 23,
            PortraitColor = new Color(1f, 0f, 0f),
        }
    };
}
