using Godot;
using System;

public class InventorySlot
{
	public Control control;
	private bool occupied = false;
    public Medicine medicine;
    public Vector2 Position;
    public MedicineButton button;

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

    public bool ReferenceAlreadyExists(Medicine inputMedicine)
    {
        //Used for inventory logic.
        //That way each button only represents one
        //medicine type at a time.
        //GD.Print($"My type is {medicine.name}, their type is {inputMedicine.name}");

        if (medicine == inputMedicine)
        {
            //GD.Print("THE SAME");
            //Yes! this is my medicine type
            return true;
        }
        else
        {
            //GD.Print("NOT THE SAME");
            //No, my medicine type is different
            return false;
        }
    }
}
