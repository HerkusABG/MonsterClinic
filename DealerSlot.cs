using Godot;
using System;

public class DealerSlot
{
    public int index;
	public string name;
	int cost;
	int amountOwned;

	public DealerSlot(Medicine medicine, int index)
	{
		this.index = index;
		name = medicine.name;
        cost = medicine.cost;
		amountOwned = medicine.amount;
	}

	public string GetSlotText()
	{
		return $"{name} \n " +
			$"(Price: {cost}) \n " +
			$"\n Owned: {amountOwned}";
    }
}
