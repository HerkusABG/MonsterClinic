using Godot;
using System;

public class DealerSlot
{
	public SlotType type;
    public int index;
	Medicine medicine;
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

    public string GetSlotText()
	{
		if(medicine != null)
		{
            return $"{medicine.name} \n " +
            $"(Price: {medicine.cost}) \n " +
            $"\n Owned: {medicine.amount}";
        }
        else
        {
            return $"{upgrade.name} \n " +
            $"(Price: {upgrade.price}) \n ";
        }
    }

	public void BuyMedicine()
	{
		medicine.amount++;
		DoctorInventory.Money -= medicine.cost;
    }
    public void BuyUpgrade()
    {
        if (type == SlotType.Boolean)
        {
            BooleanUpgrade bUpgrade = upgrade as BooleanUpgrade;
            //bUpgrade.
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
