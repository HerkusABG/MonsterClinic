using Godot;
using System;

public partial class TabletAccess : VBoxContainer
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Hide();
    }
    private void _on_tablet_pressed()
    {
        Visible = !Visible;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
