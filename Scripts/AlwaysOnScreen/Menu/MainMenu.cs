using Godot;
using System;

public partial class MainMenu : Control
{
	// Called when the node enters the scene tree for the first time.
	[Export] PackedScene option = ResourceLoader.Load<PackedScene>("res://Menu/option_menu.tscn");
	[Signal] public delegate void DeleteSaveSystemEventHandler(bool deleteSafe);
    public override void _Ready()
	{
		var ColorRecthide = GetNode<ColorRect>("ColorRect");
		ColorRecthide.Hide();
		SaveSystem.LoadFile_Settings();


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


	private void _on_new_game_button_pressed()
	{
		GetTree().ChangeSceneToFile("res://Main.tscn");


    }

    private void _on_load_game_button_button_down()
	{
		if (FileAccess.FileExists("user://Days.Json"))
		{
            // loads the days and treatment countdown for the player
            SaveSystem.Load_Days();
            GetTree().ChangeSceneToFile("res://Main.tscn");
        }


		
	}

	private void _on_options_button_pressed()
	{
		// spawns the option menu
		var optionMenu = option.Instantiate();
		AddChild(optionMenu);

    }

	private void _on_credits_button_pressed()
	{
        var ColorRecthide = GetNode<ColorRect>("ColorRect");
        ColorRecthide.Show();
        var TextRTL = GetNode<RichTextLabel>("ColorRect/RichTextLabel");
        TextRTL.Text = "Credits arent currently available, try again in the full version :)";
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
		GetTree().Quit();
	}


    private void _on_close_pressed()
	{
        var ColorRecthide = GetNode<ColorRect>("ColorRect");
        ColorRecthide.Hide();
    }

	


}
