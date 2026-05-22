using Godot;

public static class GlobalData
{
    // This holds the patient's image while the scenes change
    public static Texture2D AdmittedPatientTexture { get; set; }

    public static int Countdown { get; set; } = 4;

    public static int Money { get; set; } = 100;
}