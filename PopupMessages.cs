using Godot;
using System;
using System.Collections.Generic;

public static class PopupMessages
{

    public static Dictionary<string, string> TreatmentMessages = new()
    {
        ["Nothing"] = "",
        ["WrongMedicine"] = "This medicine won't work for this patient.\r\n\r\nBut what's done is done -- check in again tomorrow.",
        ["NoPatient"] = "No patient currently admitted.",
        ["Cured"] = "The patient is cured!",
        ["CorrectMedicine"] = "Now let the patient rest, \r\nor the side effects might kick in. \r\n\r\nCheck back in tomorrow.",
        ["Immune"] = "The medicine applied was correct, but the malady has resisted its treatment.\r\n\r\nCheck back in tomorrow.",
        ["NoRoom"] = "You cannot give medicine when not in a patient room."
    };

    public static Dictionary<string, string> ComputerMessages = new()
    {
        ["Nothing"] = "",
        ["NoMoney"] = "You don't have enough money to buy the item.",
        ["NoMedicine"] = "No medicine available, check back in tomorrow."
    };
}
