using Godot;
using System;

public partial class Deceased_Sprite : Sprite2D
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
       //Hide();
    }
    private void _on_shotgun_pressed()
    {
        //Show();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}

