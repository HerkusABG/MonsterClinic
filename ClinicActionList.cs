using Godot;
using System;
using System.Collections.Generic;

public static class ClinicActionList
{
    public static Dictionary<string, ClinicAction> Actions = new()
    {
        ["Admitted"] = new ClinicAction()
        {
            output = "The patient is admitted"
        },
        ["GiveMedicine"] = new ClinicAction()
        {
            output = $"Gave medicine: "
        },
        ["Dead"] = new ClinicAction()
        {
            output = "The patient has died"
        },
        ["Worsened"] = new ClinicAction()
        {
            output = "The patient's condition has worsened."
        },
        ["Healed"] = new ClinicAction()
        {
            output = "The patient's condition has improved."
        },
        ["Unstable"] = new ClinicAction()
        {
            output = "The lack of appropriate treatment has worsened the patient's condition."
        }
    };

    public static Dictionary<string, ClinicAction> Results = new()
    {
        ["MedSuccess"] = new ClinicAction()
        {
            output = "The treatment reduced the severity."
        },
        ["MedFail"] = new ClinicAction()
        {
            output = $". The treatment did not work."
        },
        ["MedImmune"] = new ClinicAction()
        {
            output = $". The treatment worked, but the malady resisted it!"
        }
    };
}
