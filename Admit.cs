using Godot;
using System;

public partial class Admit : Button
{
    // called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Pressed += ButtonPressed;
    }

    private void ButtonPressed()
    {
        Text = "your patient awaits";
        Disabled = true;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    private void _on_visibility_changed()
    {
        if (Visible == true)
        {
            //disables the button and prevents the admission of any new patients while one is already admitted
            if (GlobalData.CurrentPatientMalady != "none")
            {
                Text = "your patient awaits";
                Disabled = true;
            }
            else
            {
                Text = "Admit";
                Disabled = false;
            }
        }
    }
}