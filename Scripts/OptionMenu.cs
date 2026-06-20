using Godot;
using System;

public partial class OptionMenu : Control
{
    //
    [Signal]
    public delegate void OptionMenuCloseEventHandler(bool op_Close);

    public override void _Ready()
	{
	}

   private void _on_exit_pressed()
	{
        // Save Settings from the settings the player input
		SaveSystem.SaveToFile_Settings();

        // Shoots a Signal op_close is true
        EmitSignal(SignalName.OptionMenuClose, true);
     
        // deletes the option menu
        QueueFree();
	}
}
