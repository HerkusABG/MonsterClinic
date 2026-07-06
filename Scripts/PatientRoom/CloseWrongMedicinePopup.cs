using Godot;
using System;

public partial class CloseWrongMedicinePopup : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Pressed += ButtonPressed;
	}


	//closing the popup makes the medicine buttons clickable again
	private void ButtonPressed()
	{
		var med1 = (Button)GetParent().GetParent().GetNode("Give_Medicine_1");
        var med2 = (Button)GetParent().GetParent().GetNode("Give_Medicine_2");
        var med3 = (Button)GetParent().GetParent().GetNode("Give_Medicine_3");

		med1.Disabled = false;
		med2.Disabled = false;
		med3.Disabled = false;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
