using Godot;
using System;
using System.Collections.Generic;

public static class ClinicActionList
{
    public static Dictionary<string, ClinicAction> Actions = new()
    {
        ["Admitted"] = new ClinicAction()
        {
            output = "Patient has been admitted."
        },
        ["GiveMedicine"] = new ClinicAction()
        {
            output = $"Gave medicine: "
        }
    };

    public static Dictionary<string, ClinicAction> Results = new()
    {
        ["MedSuccess"] = new ClinicAction()
        {
            output = "It was applied successfully."
        },
        ["MedFail"] = new ClinicAction()
        {
            output = $"The treatment did not work."
        },
        ["MedImmune"] = new ClinicAction()
        {
            output = $"The treatment worked, but the malady resisted it!"
        }
    };
}
