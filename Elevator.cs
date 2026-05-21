using Godot;
using System;

public partial class Elevator : Button
{
    public override void _Ready()
    {
        //the button's "Pressed" signal 
        Pressed += _on_elevator_pressed;
    }

    private void _on_elevator_pressed()
    {
        // This takes the user to the room
        GetTree().ChangeSceneToFile("res://Scenes/Room.tscn");
    }

    public override void _Process(double delta)
    {
    }
}
