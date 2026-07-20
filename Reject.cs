using Godot;
using System;

public partial class Reject : TextureButton
{
    [Export] public Contents_P_I PatientManager; 

    public override void _Ready()
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