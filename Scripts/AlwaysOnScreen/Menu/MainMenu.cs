using Godot;
using System;

public partial class MainMenu : Control
{
	// Called when the node enters the scene tree for the first time.
	[Export] PackedScene option = ResourceLoader.Load<PackedScene>("res://Scenes/option_menu.tscn");
	[Signal] public delegate void DeleteSaveSystemEventHandler(bool deleteSafe);

	[Export] TextureButton ExitButton;
	[Export] TextureButton NewGameButton;
	[Export] TextureButton SettingsButton;
	[Export] TextureButton CreditsButton;
	public override void _Ready()
	{
		var ColorRecthide = GetNode<ColorRect>("ColorRect");
		ColorRecthide.Hide();
		SaveSystem.LoadFile_Settings();

		ExitButton.Pressed += Exit;
		NewGameButton.Pressed += NewGame;
		SettingsButton.Pressed += Settings;
		CreditsButton.Pressed += Credits;

		// Player indicator for not having any save files
		var LockColor = GetNode<ColorRect>("Lock_Color");
		if (FileAccess.FileExists("user://Days.Json"))
		{
		   
			LockColor.Hide();
		}
		else
		{
			LockColor.Show();
		}


	}

	private void NewGame()
	{
		GetTree().ChangeSceneToFile("res://Scenes/Main.tscn");
	}

	private void _on_load_game_button_button_down()
	{
		if (FileAccess.FileExists("user://Days.Json"))
		{
			// loads the days and treatment countdown for the player
			SaveSystem.Load_Days();
			GetTree().ChangeSceneToFile("res://Scenes/Main.tscn");
		}


		
	}

	

	private void Settings()
	{
		// spawns the option menu
		var optionMenu = option.Instantiate();
		AddChild(optionMenu);
	}

	private void _on_credits_button_pressed()
	{
		
	}

	private void Credits()
	{
		var ColorRecthide = GetNode<ColorRect>("ColorRect");
		ColorRecthide.Show();
		var TextRTL = GetNode<RichTextLabel>("ColorRect/RichTextLabel");
		TextRTL.Text = "Can - Producer \n" +
			"Rome - Art \n" +
			"Tilda - Game Design & Production \n" +
			"Herkus - Programming & Production \n" +
			"Jacob - Programming \n" +
			"Nadia - Programming \n" +
			"Princess - Programming \n" +
			"OJ - Programming \n" +
			"Fox - Programming";
	}

	private void _on_delete_save_pressed()
	{
		var LockColor = GetNode<ColorRect>("Lock_Color");
		LockColor.Show();
		// checks if File exists
		if (FileAccess.FileExists("user://Days.Json"))
		{
			// deletes the save json
			SaveSystem.Delete_Days();
		}

		// Player indicator for not having any save files
		


	}

	private void _on_exit_button_pressed()
	{
		// closes the game
		
	}

	private void Exit()
	{
		GetTree().Quit();
	}

	private void _on_close_pressed()
	{
		var ColorRecthide = GetNode<ColorRect>("ColorRect");
		ColorRecthide.Hide();
	}

	


}
