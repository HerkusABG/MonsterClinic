using Godot;
using System;

public class DealerSlot
{
    public int index;
	public string name;
	int cost;
	int amountOwned;
	Medicine medicine;

	public DealerSlot(Medicine medicine, int index)
	{
		this.index = index;
		name = medicine.name;
        cost = medicine.cost;
		amountOwned = medicine.amount;
		this.medicine = medicine;
	}

	public string GetSlotText()
	{
		return $"{medicine.name} \n " +
			$"(Price: {medicine.cost}) \n " +
			$"\n Owned: {medicine.amount}";
    }

	public void BuyMedicine()
	{
		medicine.amount++;
		DoctorInventory.Money -= medicine.cost;
        GD.Print($"Bought {name}");
    }
}
