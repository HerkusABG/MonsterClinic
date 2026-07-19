using Godot;
using System;

public partial class InsufficientFunds : Label
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//hidden by default
		Hide();
	}
    private void _on_close_pressed()
    {
		//hide it when you close it
        Hide();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
