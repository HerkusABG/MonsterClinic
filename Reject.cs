using Godot;
using System;

public partial class Reject : Button
{
    [Export] public Contents_P_I PatientManager; 

    public void Initialize()
    {
        Pressed += OnRejectPressed;
    }

    // This is the method that was missing!
    private void OnRejectPressed()
    {
        if (PatientManager != null)
        {
           //the mthod i added to Contents_P_I to refresh the patient data and update the UI.
            PatientManager.GenerateNewPatient();
        }
    }
}