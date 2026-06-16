using Godot;
using System;

public partial class OptionMenu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

   private void _on_exit_pressed()
	{
		SaveSystem.SaveToFile_Settings();
        QueueFree();
	}
}
