using Godot;
using System;

public partial class MainMenu : Control
{
	// Called when the node enters the scene tree for the first time.
	[Export] PackedScene option = ResourceLoader.Load<PackedScene>("res://Menu/option_menu.tscn");
	public override void _Ready()
	{
		var ColorRecthide = GetNode<ColorRect>("ColorRect");
		ColorRecthide.Hide();
		SaveSystem.LoadFile_Settings();


	}


	private void _on_new_game_button_pressed()
	{
		GetTree().ChangeSceneToFile("res://Main.tscn");
	}

    private void _on_load_game_button_button_down()
	{
		GetTree().ChangeSceneToFile("res://Main.tscn");
		
	}

	private void _on_options_button_pressed()
	{

		var optionMenu = option.Instantiate();
		AddChild(optionMenu);

    }

	private void _on_credits_button_pressed()
	{
        var ColorRecthide = GetNode<ColorRect>("ColorRect");
        ColorRecthide.Show();
        var TextRTL = GetNode<RichTextLabel>("ColorRect/RichTextLabel");
        TextRTL.Text = "Credits arent right ready now, try in the full version again";
    }

	private void _on_exit_button_pressed()
	{
		GetTree().Quit();
	}


    private void _on_close_pressed()
	{
        var ColorRecthide = GetNode<ColorRect>("ColorRect");
        ColorRecthide.Hide();
    }

	


}
