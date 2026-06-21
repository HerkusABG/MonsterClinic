using Godot;
using System;

public partial class Admit : Button
{
    // called when the node enters the scene tree for the first time.
    [Export] public Contents_P_I PatientManager;
    public override void _Ready()
    {
        //Pressed += ButtonPressed;
    }

    private void ButtonPressed()
    {
        //PatientManager.GenerateNewPatientVoid();
        //Text = "your patient awaits";
    }

    public void SetButtonStatus(bool status)
    {
        Disabled = !status;
        if (status)
        {
            Text = "Admit";
        }
        else
        {
            Text = "Clinic is full!";
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        //disables the button and prevents the admission of any new patients while one is already admitted
        /*if (GlobalData.CurrentPatientMalady != "none")
        {
            Text = "your patient awaits";
            Disabled = true;
        }
        else
        {
            Text = "Admit";
            Disabled = false;
        } */
    }
}