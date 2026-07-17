using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;

public partial class Inventory : Node2D
{
    //Storing a reference to all the buttons, labels, etc., for easy reference in the methods
    TextureButton InventoryButton;
    TextureRect OpenInventory;
    Button ShotgunButton;
    
    Button Close;

    [Export] TextureButton ButtonTemplate;

    [Export] Control PositionControl;

    private List<InventoryUiInstance> InventoryInstances = new List<InventoryUiInstance>();

    TreatmentManager TreatmentManager;

    public MapUI MapUi;

	// Called when the node enters the scene tree for the first time.
    public void Initialize()
    {
        //grabs references to all the necessary nodes
        GetNodes();

        InitializeChildren();

        Subscribe();

        //disables the shotgun until we're in the patient room
        ShotgunButton.Disabled = true;
    }

    private void InitializeChildren()
    {
        TreatmentManager treatment = GetNode<TreatmentManager>("Treatment_Manager");
        treatment.Initialize();
        TreatmentManager = treatment;
    }

    private void Subscribe()
    {
        //assigning methods to all the buttons
        InventoryButton.Pressed += InventoryToggle;
        ShotgunButton.Pressed += KillPatient;
        Close.Pressed += () => CloseInventory(Close);
    }


	private void GetNodes()
	{
        //Basically just grabbing all the nodes
        MapUi = GetTree().Root.GetNode("Main").GetNode("Computer").GetNode("Player_Interactables_C").GetNode<MapUI>("MapControl");
        InventoryButton = GetNode<TextureButton>("Inventory_Button");
		OpenInventory = GetNode<TextureRect>("Open_Inventory");
		ShotgunButton = OpenInventory.GetNode<Button>("Shotgun");

        Close = OpenInventory.GetNode<Button>("Close");

        InventoryButtonGeneration(PositionControl, OpenInventory, ButtonTemplate);
    }
    public void InventoryButtonGeneration(Control inputControl, Control parent, TextureButton Template)
    {
        List<InventorySlot> slots = new List<InventorySlot>();

        List<MedicineButton> medicineButtons = new List<MedicineButton>();
        foreach (Node node in inputControl.GetChildren())
        {
            Control control = node as Control;
            if (control != null)
            {
                InventorySlot slot = new InventorySlot();
                slot.control = control;
                slots.Add(slot);
            }
            else
            {
                GD.Print("ERROR IN INVENTORY.CS, NULL REFERENCE");
            }
        }
        for (int i = 0; i < 3; i++)
        {
            TextureButton newButton = (TextureButton)Template.Duplicate();
            MedicineButton medButton = newButton as MedicineButton;
            medButton.Initialize();
            parent.AddChild(newButton);
            medicineButtons.Add(medButton);
        }
        InventoryUiInstance instance = new InventoryUiInstance(slots, medicineButtons);
        InventoryInstances.Add(instance);
    }
    //you can also press the inventory to open it
	private void InventoryToggle()
	{
        InventoryActions();
        OpenInventory.Visible = !OpenInventory.Visible;
    }

    //kill the patient, show the deceased sprites and disable the shotgun
    private void KillPatient()
    {
        var PatientAdmission = GetParent().GetNode<Node2D>("Patient_Interface");
        var patientRoomSprites = PatientAdmission.GetNode<Node2D>("Sprites_PH");
        var DeceasedSprite1 = patientRoomSprites.GetNode<Sprite2D>("Deceased_Sprite");
        var DeceasedSprite2 = patientRoomSprites.GetNode<Sprite2D>("Deceased_Sprite_2");

        DeceasedSprite1.Show();
        DeceasedSprite2.Show();
        ShotgunButton.Disabled = true;
    }

    //disable the shotgun if we're not in the patient admission room
    private void _on_patient_interface_visibility_changed()
    {
        var PatientAdmission = GetParent().GetNode<Node2D>("Patient_Interface");

        if (PatientAdmission.Visible == true)
        {
            ShotgunButton.Disabled = false;
        } else
        {
            ShotgunButton.Disabled = true;
        }
    }
    private void CloseInventory(Button button)
    {
        var Parent = button.GetParent<TextureRect>();
        Parent.Hide();
    }

    public void InventoryActions()
    {
        for(int i = 0; i < InventoryInstances.Count; i++)
        {
            RenderMedicine(InventoryInstances[i]);
            UpdateInventory(InventoryInstances[i]);
            if (TreatmentManager.GetRoom() != null)
            {
                SetButtonStatus(TreatmentManager.GetRoom().GetAlreadyTreated(), InventoryInstances[i]);
            }
        }
    }

    private void RenderMedicine(InventoryUiInstance instance)
    {
        List<MedicineButton> buttons = instance.MedicineButtons;
        for (int i = 0; i < buttons.Count; i++)
        {
            Medicine medicine = MedicineManager.Database.ElementAt(i).Value;
            if (buttons[i].GetIsAssigned())
            {
                buttons[i].RenderText(medicine);
            }
        }
    }

    public void UpdateInventory(InventoryUiInstance instance)
    {
        List<MedicineButton> buttons = instance.MedicineButtons;
        for (int i = 0; i < buttons.Count; i++)
        {
            Medicine medicine = MedicineManager.Database.ElementAt(i).Value;
            if (medicine.amount > 0)
            {
                if (!DoesButtonAlreadyExist(medicine, instance))
                {
                    InventorySlot slot = FindEmptySlot(instance);
                    if (slot == null)
                    {
                        GD.Print("ERROR IN INVENTORY.CS, NULL");
                        return;
                    }
                    buttons[i].AssignToSlot(slot, medicine);
                    TreatmentManager.AddSubscription(instance.MedicineButtons[i]);
                }
            }
            else
            {
                if (DoesButtonAlreadyExist(medicine, instance))
                {
                    TreatmentManager.RemoveSubscription(instance.MedicineButtons[i]);
                    buttons[i].RemoveFromSlot();
                }
            }
        }
    }

    public void SetButtonStatus(bool isActive, InventoryUiInstance instance)
    {
        List<MedicineButton> buttons = instance.MedicineButtons;
        foreach (MedicineButton button in buttons)
        {
            button.Disabled = isActive;
        }
    }

    private bool DoesButtonAlreadyExist(Medicine inputMedicine, InventoryUiInstance instance)
    {
        List<MedicineButton> buttons = instance.MedicineButtons;
        for (int i = 0; i < buttons.Count; i++)
        {
            if (buttons[i].HasSameMedicineReference(inputMedicine))
            {
                return true;
            }
        }
        return false;
    }

    private InventorySlot FindEmptySlot(InventoryUiInstance instance)
    {
        foreach (InventorySlot slot in instance.Slots)
        {
            if(!slot.GetOccupiedStatus())
            {
                return slot;
            }
        }
        return null;
    }
}
