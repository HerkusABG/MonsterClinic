using Godot;
using System;

public partial class Bed_Time : Label
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Hide();
    }

    private void _on_bed_pressed()
    {
        GetTree().ChangeSceneToFile("res://Bed/bed.tscn");
    }

    private void _on_close_pressed()
    {
        Hide();
        GlobalData.PreviousScenes.Pop();
    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
