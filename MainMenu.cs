using Godot;
using System;

public partial class MainMenu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var ColorRecthide = GetNode<ColorRect>("ColorRect");
		ColorRecthide.Hide();
	}


	private void _on_new_game_button_pressed()
	{

	}

    private void _on_load_game_button_button_down()
	{
        var ColorRecthide = GetNode<ColorRect>("ColorRect");
        ColorRecthide.Show();
        var TextRTL = GetNode<RichTextLabel>("ColorRect/RichTextLabel");
		TextRTL.Text = "Loading Game isnt right now a option, try in the full version again";
	}

	private void _on_options_button_pressed()
	{
        var ColorRecthide = GetNode<ColorRect>("ColorRect");
        ColorRecthide.Show();
        var TextRTL = GetNode<RichTextLabel>("ColorRect/RichTextLabel");
        TextRTL.Text = "Option isnt right ready now, try in the full version again";
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
