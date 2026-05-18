using Godot;
using System;

public partial class Contents_P_I : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        Hide();
	}
    private void _on_previous_s_pressed()
    {
        Hide();
        var OfficeScene = (Node2D)GetParent().GetNode("Office");
        OfficeScene.Show();
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
