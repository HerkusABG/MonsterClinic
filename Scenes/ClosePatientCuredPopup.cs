using Godot;
using System;

public partial class ClosePatientCuredPopup : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Pressed += ButtonPressed;
	}

	//closing the popup removes the patient from the room, enables the mdeicine buttons, and resets the current patient malady data to the defaults
	private void ButtonPressed()
	{
        var med1 = (Button)GetParent().GetParent().GetNode("Give_Medicine_1");
        var med2 = (Button)GetParent().GetParent().GetNode("Give_Medicine_2");
        var med3 = (Button)GetParent().GetParent().GetNode("Give_Medicine_3");

        med1.Disabled = false;
        med2.Disabled = false;
        med3.Disabled = false;

        var patient = GetParent().GetParent().GetParent().GetNode<Node2D>("Patient_Display");
		var patientInfo = GetParent().GetParent().GetParent().GetNode<CanvasItem>("Patient_Info");

		patient.Hide();
		patientInfo.Hide();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
