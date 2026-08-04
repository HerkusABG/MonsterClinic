using Godot;
using System;
using System.Collections.Generic;

public static class ClinicActionList
{
    public static Dictionary<string, ClinicAction> Database = new()
    {
        ["Admitted"] = new ClinicAction()
        {
            output = "Patient has been admitted."
        },
        ["Rejected"] = new ClinicAction()
        {
            output = $"Patient has been rejected."
        }
    };
}
