using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public static class StoryPatientList
{


    public static Dictionary<string, StoryPatientStats> Database = new()
    {
        ["Karl"] = new StoryPatientStats
        {
            name = "Karl",
            entrySpeech = "My condition is rapidly getting worse. Please help me!",
            patientID = "1001",
            age = 23,
            PortraitColor = new Color(1f, 0f, 0f),
            malady = MaladyList.Database["Slithic"],
        }/*,
        ["Mina"] = new StoryPatientStats
        {
            name = "Mina",
            entrySpeech = "Please help -- I tried all sorts of treatments and nothing worked!",
            patientID = "1002",
            age = 42,
            PortraitColor = new Color(0f, 1f, 0f),
            malady = MaladyList.Database.ElementAt(1).Value,

        }*/
    };

}
