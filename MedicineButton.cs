using Godot;
using System;
using System.Linq;

public partial class MedicineButton : TextureButton
{

    Label MedName;
    Label MedCount;
    Medicine medicine;
    InventorySlot slot;
    bool isAssigned;
    public void Initialize()
	{
        MedName = GetNode<Label>("Name");
        MedCount = GetNode("Stripe").GetNode<Label>("Count");
        Disabled = false;
    }

    public void AssignToSlot(InventorySlot inputSlot, Medicine inputMedicine)
    {
        isAssigned = true;
        slot = inputSlot;
        Position = slot.GetPosition();
        Show();
        medicine = inputMedicine;
        GD.Print($"My name is {medicine.name} I am showing up now.");
        RenderText(inputMedicine);
        slot.SetOccupiedStatus(true);
    }

    public void RemoveFromSlot()
    {
        isAssigned = false;
        slot.SetOccupiedStatus(false);
        slot = null;
        medicine = null;
        Hide();
    }

    public void RenderText(Medicine inputMedicine)
    {
        MedName.Text = $"{inputMedicine.name}";
        MedCount.Text = $"{inputMedicine.amount}";
    }

    public bool HasSameMedicineReference(Medicine inputMedicine)
    {
        if(medicine == inputMedicine)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public Medicine GetMedicineType()
    {
        return medicine;
    }

    public bool GetIsAssigned()
    {
        return isAssigned;
    }
}
