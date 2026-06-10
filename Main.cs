using Godot;
using System;

public partial class Main : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        //always keep the office at the bottom of the previous scenes stack, so the reference on how to return to it is always there
        GlobalData.PreviousScenes.Push(GetNode("Office").GetPath());
	}

    public override void _UnhandledInput(InputEvent @event)
    {
        //grab paths for the office and the pause menu
        var office = (Node2D)GetNode("Office");
        var pause = (Node2D)GetNode("Pause");
        if (@event is InputEventMouseButton eventKey)
        {
            //if a key is pressed and that key is the right mouse button, and if the pause menu and the office aren't visible
            if (eventKey.Pressed && eventKey.ButtonIndex == MouseButton.Right && pause.Visible == false && office.Visible == false)
            {
                //pop a scene from the previous scenes stack, this is the scene currently in use
                var current_scene = (Node2D)GetNode(GlobalData.PreviousScenes.Pop().ToString());
                //hide it
                current_scene.Hide();
                //pop a scene again, this is the scene we were previously in
                var parent = (Node2D)GetNode(GlobalData.PreviousScenes.Peek().ToString());
                //show it
                parent.Show();

            }
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
