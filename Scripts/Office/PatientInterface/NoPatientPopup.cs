using Godot;
using System;

public partial class NoPatientPopup : Label
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Hide();
	}

	private void _on_close_pressed()
	{
		Hide();
	}

	//also gets hidden when you leave the room, it's a popup, no reason to make it stick around
	private void _on_room_visibility_changed()
	{
		Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
