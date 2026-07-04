using Godot;
using System;

public partial class Reject : Button
{
    [Export] public Contents_P_I PatientManager; 

    public override void _Ready()
    {
        
        Pressed += OnRejectPressed;
    }


    private void OnRejectPressed()
    {
        if (PatientManager != null)
        {
           //the mthod i added to Contents_P_I to refresh the patient data and update the UI.
         PatientManager.UpdatePatientInterfaceUI();
        }
        
    }
}