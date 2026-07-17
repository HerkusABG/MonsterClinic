using Godot;
using System;
using System.Collections;

public static class GlobalData
{
    // This holds the patient's image while the scenes change
    //part of Princess's system for moving the patient sprite to the patient room, currently deprecated, but just commented out because it might be useful again
    //public static Texture2D AdmittedPatientTexture { get; set; }

    // player ingame days in containment and treatment countdown 
    public static int Player_Ingame_Days = 0;

    public static bool inPatientRoom = false;
    public static int Countdown { get; set; } = 4;

    // self treatment medicine for the player -> the cost and availibility so the player cant spamm the selftreatment
    public static int MedicinePlayer { get; set; } = 0;
    public static int MedicineCost { get; set; } = 150;
    public static int Medicincavailability { get; set; } = 0;

    // the dealer dialog can spawn if the dialog_dealer is true
    public static Boolean Dialog_Dealer { get; set; } = false;
    public static Boolean Dialog_Dealer_Control { get; set; } = true;
    public static string Reasion { get; set; } = "none";

    //public static int Money { get; set; } = 150;

    //money you always get when you go to sleep, separate from the daily earnings, made it a whole variable here so it's easier to tweak later
    public static int PassiveIncome { get; set; } = 20;

    public static int DailyEarnings { get; set; } = 0;

    public static int Medicine1Count { get; set; } = 0;

    public static int Medicine2Count { get; set; } = 0;

    public static int Medicine3Count { get; set; } = 0;

    //bool that becomes when you treat a patient, to prevent switching scenes from reactivating the GiveMedicine buttons when you've already
    //treated the patient that day, and becomes false when you go to bed
    public static bool DailyLockout { get; set; } = false;

    public static int patientCount = 0;
    public static string CurrentPatientMalady { get; set; } = "none";

    // Tracks if there is currently a patient waiting at the office window
    public static bool IsPatientInWindow { get; set; } = false;

    // this is a placeholder for the implementation of the future Malady Class system 
    public static int CurrentPatientSeverity { get; set; } = 0;

    //stack that holds previous scenes accessed, going back to the office, kinda like the branches of a tree
    public static Stack PreviousScenes { get; set; } = new Stack();

    public static bool IsClinicFullyOccupied { get; set; } = false; // Tracks if the clinic is fully occupied and is used for the NoVacancy flashing panel

}
