using Godot;
using System.Collections;

public static class GlobalData
{
    // This holds the patient's image while the scenes change
    //part of Princess's system for moving the patient sprite to the patient room, currently deprecated, but just commented out because it might be useful again
    //public static Texture2D AdmittedPatientTexture { get; set; }

    public static int Countdown { get; set; } = 4;

    public static int Money { get; set; } = 150;

    public static int DailyEarnings { get; set; } = 0;

    public static int Medicine1Count { get; set; } = 0;

    public static int Medicine2Count { get; set; } = 0;

    public static int Medicine3Count { get; set; } = 0;

    public static string[] Maladies = { "A", "B", "C" };

    public static string CurrentPatientMalady { get; set; } = "none";

    public static int CurrentPatientSeverity { get; set; } = 0;

    //stack that holds previous scenes accessed, going back to the office, kinda like the branches of a tree
    public static Stack PreviousScenes { get; set; } = new Stack();
}