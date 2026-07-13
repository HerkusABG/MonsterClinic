using Godot;
using System;

public class InventorySlot
{
	public Control control;
	public bool occupied = false;

	public Vector2 GetPosition()
	{
		return control.Position;
	}
}
