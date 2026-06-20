using Godot;
using System;

public partial class PatientInfo : Label
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Hide();
		Text = "Patient info: \n Malady: Malady " + GlobalData.CurrentPatientMalady + "\n Severity: " + GlobalData.CurrentPatientSeverity;
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
        Text = "Patient info: \n Malady: Malady " + GlobalData.CurrentPatientMalady + "\n Severity: " + GlobalData.CurrentPatientSeverity;
	}
}
