using Godot;
using System;
using Godot.Collections;

//welcome to my great failed attempt at drag and drop, god i hope I fix this before anyone finds it
public partial class DragMedicine : Label
{
    // Called when the node enters the scene tree for the first time.

    private int pain = 1;
	public override void _Ready()
	{
	}

    public override Variant _GetDragData(Vector2 atPosition)
    {
        ColorRect move = new ColorRect();
        move.Size = new Vector2(100, 150);
        move.Color = new Color((float)0.15, (float)0.745, (float)0.175);
        SetDragPreview(move);
        return new Dictionary() {
                { "shape", pain },
                { "source", this } };

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
