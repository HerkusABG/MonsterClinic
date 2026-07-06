using Godot;
using System;

public partial class LeaveRoom : Button
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        MouseEntered += HoverOn;
        MouseExited += HoverOff;
    }

    private void HoverOn()
    {
        //makes the text show up when hovering over the button
        Text = "Leave room";
    }

    private void HoverOff()
    {
        //makes the text disappear when you stop hovering
        Text = "";
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
