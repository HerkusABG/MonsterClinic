using Godot;
using System;

public static class DoctorInventory
{
    //This class is an attempt to split up the information found in GlobalData
    //into more managable chunks. This class is reserved for information associated
    //with the most outwardly doctor-related stats, for example money or days left until treatment.
    public static int Money { get; set; } = 15000;

    static void ChangeMoneyAmount()
    {

    }
}
