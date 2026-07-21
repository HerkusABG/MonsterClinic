using Godot;
using System;
using System.Collections.Generic;

public class InventoryUiInstance
{
    public List<InventorySlot> Slots = new List<InventorySlot>();

    public List<InventorySlot> AllSlots = new List<InventorySlot>();

    public List<MedicineButton> MedicineButtons = new List<MedicineButton>();
    public InventoryUiInstance(List<InventorySlot> slots, List<MedicineButton> medicineButtons)
    {
        Slots = slots;
        MedicineButtons = medicineButtons;
    }
}
