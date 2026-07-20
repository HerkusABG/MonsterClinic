using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public static class StoryPatientList
{


    public static Dictionary<string, StoryPatientStats> Database = new()
    {
        ["Jimbo"] = new StoryPatientStats
        {
            name = "Jimbo the Diseased",
            entrySpeech = "Hi, I'm Jimbo the Diseased, and you'll never guess how my health is holding up",
            patientID = "1001",
            age = 23,
            PortraitColor = new Color(1f, 0f, 0f),
            malady = MaladyList.Database.ElementAt(1).Value,
            
        },
        ["Phil"] = new StoryPatientStats
        {
            name = "Legless Phil",
            entrySpeech = "What's up, I'm Legless Phil, and despite what my sprite may indicate right now, I have no legs",
            patientID = "1002",
            age = 42,
            PortraitColor = new Color(0f, 1f, 0f),
            malady = MaladyList.Database.ElementAt(1).Value,

        },
        ["Stephanie"] = new StoryPatientStats
        {
            name = "Depressed Stephanie",
            entrySpeech = "Oh hi, I'm... hell, I hardly know who I even am these days...",
            patientID = "1003",
            age = 31,
            PortraitColor = new Color(0f, 0f, 1f),
            malady = MaladyList.Database.ElementAt(1).Value,

        }
    };

}
