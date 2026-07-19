using Godot;
using System;

public partial class Inventory_Access_P_I : VBoxContainer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	//	Hide();
	}
    private void _on_inventory_p_i_pressed()
    {
      //  Visible = !Visible;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
