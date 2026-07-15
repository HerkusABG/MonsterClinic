using Godot;
using System;

public class InventorySlot
{
	public Control control;
	private bool occupied = false;

	public Vector2 GetPosition()
	{
		return control.Position;
	}

	public void SetOccupiedStatus(bool input)
	{
		occupied = input;
	}

	public bool GetOccupiedStatus()
	{
		return occupied;
	}
}
