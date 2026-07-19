using Godot;
using System;
using System.Linq;

public partial class MedicineButton : TextureButton
{
    //Logic that is crucial for the functionality of the inventory.
    //Stores the information necessary for a modular inventory

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
        //Setting up all the relevant info so that the button appears
        //In the inventory views.
        //Making sure everybody knows this button is already used for something
        isAssigned = true;

        //settingthe slot where the button will live
        slot = inputSlot;
        Position = slot.GetPosition();

        Show();
        //Button remembers what medicine it represents
        medicine = inputMedicine;
        //Render button text based on the medicine type
        RenderText(inputMedicine);
        //Telling the slot that it's occupied
        slot.SetOccupiedStatus(true);
    }

    public void RemoveFromSlot()
    {
        //Reverse logic of AssignToSlot
        //I'm free, I can be used represent new data!
        isAssigned = false;
        //The slot no longer has something assigned
        slot.SetOccupiedStatus(false);
        slot = null;

        //Same as with isAssigned
        medicine = null;
        //Hiding the button.
        Hide();
    }

    public void RenderText(Medicine inputMedicine)
    {
        //Update the button text
        //Shows the amount and the name of the medicine
        MedName.Text = $"{inputMedicine.name}";
        MedCount.Text = $"{inputMedicine.amount}";
    }

    public bool HasSameMedicineReference(Medicine inputMedicine)
    {
        //Used for inventory logic.
        //That way each button only represents one
        //medicine type at a time.

        if(medicine == inputMedicine)
        {
            //Yes! this is my medicine type
            return true;
        }
        else
        {
            //No, my medicine type is different
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
