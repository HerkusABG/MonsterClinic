using Godot;
using System;

public class DealerSlot
{
	public SlotType type;
    public int index;
	public Medicine medicine;
	public IncrementalUpgrade upgrade;


	public DealerSlot(Medicine medicine, int index)
	{
		this.index = index;
		this.medicine = medicine;
	}

    public DealerSlot(IncrementalUpgrade upgrade, int index)
    {
        this.index = index;

        BooleanUpgrade bUpgrade = upgrade as BooleanUpgrade;
        if (bUpgrade != null)
        {
            type = SlotType.Boolean;
        }
        else
        {
            type = SlotType.Integer;
        }
        this.upgrade = upgrade;
    }

    public string GetSlotName()
    {
        if (medicine != null)
        {
            return $"{medicine.name}";
        }
        else
        {
            return $"{upgrade.name}";
        }
    }

    public string GetSlotText()
	{
		if(medicine != null)
		{
            return $"{medicine.name} \n" +
            $"(Price: {medicine.cost})  \n" +
            $" Owned: {medicine.amount}";
        }
        else
        {
            return $"{upgrade.name} \n" +
            $"(Price: {upgrade.price}) \n";
        }
    }

	public bool BuyMedicine()
	{
        if(DoctorInventory.Money >= medicine.cost)
        {
            medicine.amount++;
            DoctorInventory.Money -= medicine.cost;
            return true;
        }
        else
        {
            return false;
        }
    }
    public void BuyUpgrade()
    {
        if (type == SlotType.Boolean)
        {
            BooleanUpgrade bUpgrade = upgrade as BooleanUpgrade;
        }
        else
        {
            type = SlotType.Integer;
        }
    }
    public enum SlotType
    {
        Integer,
        Boolean
    }
}
