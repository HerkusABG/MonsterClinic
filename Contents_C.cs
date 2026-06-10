using Godot;
using System;

public partial class Contents_C : Node2D
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Hide();
    }

    private void _on_log_o_pressed()
    {
        //when leaving the room, hide it, show the office, and pop the room off the previous scenes stack, to not interfere with the right click functionality
        Hide();
        var OfficeScene = (Node2D)GetParent().GetNode("Office");
        OfficeScene.Show();
        GlobalData.PreviousScenes.Pop();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
