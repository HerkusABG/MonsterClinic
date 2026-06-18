using Godot;
using System;

public partial class PauseMenu : Node2D
{
	[Export] PackedScene option = ResourceLoader.Load<PackedScene>("res://Menu/option_menu.tscn");
    
    public override void _Ready()
	{
		//hide the scene itself on startup
		Hide();
		SaveSystem.LoadFile_Settings();
		// hide the Control on startup
        var get = GetNode<Control>("Spawn_Options");
		get.Hide();
		
    }

	public override void _UnhandledInput(InputEvent @event)
	{
		//grabbing the reference to the vbox containing the 3 main menu buttons
        var menu = (VBoxContainer)GetNode("Player_Interactables_Menu").GetNode("Menu_Box");
		if (@event is InputEventKey eventKey)
		{
			//if a key is pressed and that key is esc
			if (eventKey.Pressed && eventKey.Keycode == Key.Escape)
			{
				Show();
				
			}
		}
	}

	//when resume pressed, hide the pause menu
	private void _on_resume_pressed()
	{
		Hide();
	}

	//when settings pressed, hide the main section of the pause menu, and show the settings from the option menu
	private void _on_settings_pressed()
	{
        var menu = (VBoxContainer)GetNode("Player_Interactables_Menu").GetNode("Menu_Box");
        

		// Control node gets shown and on this spawns the option menu with the settings
		var get = GetNode<Control>("Spawn_Options");
		get.Show();

		// Spawns the option menu on the Control node
        var optionMenu = option.Instantiate();
        get.AddChild(optionMenu);
		
        // Connects the Signalname from the Option menu to the new callable function OptionMenuClose
        optionMenu.Connect(OptionMenu.SignalName.OptionMenuClose, new Callable(this, nameof(OptionMenuClose)));

    }

	//when exit pressed, quit the game
	private void _on_exit_pressed()
	{
		GetTree().ChangeSceneToFile("res://Menu/main_menu.tscn");
	}

	public void OptionMenuClose(Boolean op_Close)
	{
		// if the op_close is true, then Spawn_options is hide
		if(op_Close == true)
		{
			// The Control gets hidden again
            var get = GetNode<Control>("Spawn_Options");
            get.Hide();
        }
		
    }

}
