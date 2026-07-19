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

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventKey eventKey)
        {
            if (eventKey.Pressed && eventKey.Keycode == Key.Escape)
            {
                //normally pressing esc with the options menu open will close both it and the pause menu, but hiding the pause menu with this method is a trick that makes it stay visible
                var pause = (Node2D)GetParent().GetParent();
                pause.Hide();
                // Save Settings from the settings the player input
                SaveSystem.SaveToFile_Settings();

                // Shoots a Signal op_close is true
                EmitSignal(SignalName.OptionMenuClose, true);

                // deletes the option menu
                QueueFree();
            }
        }
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
