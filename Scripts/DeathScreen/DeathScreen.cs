using Godot;
using System;

public partial class DeathScreen : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        // Set the reason for death in the RichTextLabel. The RichTextLabel will display the reason for the players death
        var Reason = GetNode<RichTextLabel>("Reason");
		Reason.BbcodeEnabled = true;
		Reason.Text = $"[b][font_size=70] You died! {GlobalData.Reasion} [/font_size][/b]";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void _on_retry_pressed()
	{
		// For debugging (testingt) purposes, the Treatment countdown gets set to 2. Scene changes to the Office.
		GlobalData.Countdown = 2;
		GetTree().ChangeSceneToFile("res://Scenes/Main.tscn");
	}
}
