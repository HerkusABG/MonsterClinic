using Godot;
using System;

public partial class PauseMenu : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Hide();
        var settings = (HBoxContainer)GetNode("Player_Interactables_Menu").GetNode("Settings_Box");
		settings.Hide();
	}

	public override void _UnhandledInput(InputEvent @event)
	{
        var menu = (VBoxContainer)GetNode("Player_Interactables_Menu").GetNode("Menu_Box");
        var settings = (HBoxContainer)GetNode("Player_Interactables_Menu").GetNode("Settings_Box");
		if (@event is InputEventKey eventKey)
		{
			if (eventKey.Pressed && eventKey.Keycode == Key.Escape)
			{
				if (settings.Visible == true)
				{
					settings.Hide();
					menu.Show();
				}
				else
				{
					Visible = !Visible;
				}
			}
		}
	}

	private void _on_resume_pressed()
	{
		Hide();
	}

	private void _on_settings_pressed()
	{
        var menu = (VBoxContainer)GetNode("Player_Interactables_Menu").GetNode("Menu_Box");
        var settings = (HBoxContainer)GetNode("Player_Interactables_Menu").GetNode("Settings_Box");
		menu.Hide();
		settings.Show();
    }

	private void _on_exit_pressed()
	{
		GetTree().Quit();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
