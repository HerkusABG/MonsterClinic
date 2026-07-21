using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;


//This class manages and control all the inventory buttons inside of the game.
public partial class Inventory : Node2D
{
    //Storing a reference to all the buttons, labels, etc., for easy reference in the methods
    TextureButton InventoryButton;
    TextureRect OpenInventory;
    Button ShotgunButton;
    
    Button Close;

    [Export] TextureButton ButtonTemplate;

    [Export] Control PositionControl;

    [Export] Button UpButton;
    [Export] Button DownButton;

    private List<InventoryUiInstance> InventoryInstances = new List<InventoryUiInstance>();
    private List<InventorySlot> AllSlots = new List<InventorySlot>();
    private List<InventorySlot> ActiveSlots = new List<InventorySlot>();

    TreatmentManager TreatmentManager;

    public MapUI MapUi;

    private int inventoryIndex = 0;

	// Called when the node enters the scene tree for the first time.
    public void Initialize()
    {
        //grabs references to all the necessary nodes
        GetNodes();

        InitializeChildren();

        Subscribe();

        //disables the shotgun until we're in the patient room
        ShotgunButton.Disabled = true;

        SlotSetup();
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

        UpButton.Pressed += () => InventoryNavigation(-1);
        DownButton.Pressed += () => InventoryNavigation(1);
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

    private void SlotSetup()
    {
        for (int i = 0; i < MedicineManager.Database.Count; i++)
        {
            InventorySlot slot = new InventorySlot();
            TextureButton newButton = (TextureButton)ButtonTemplate.Duplicate();
            MedicineButton medButton = newButton as MedicineButton;
            medButton.Initialize();
            slot.medicine = MedicineManager.Database.ElementAt(i).Value;
            slot.button = medButton;
            slot.button.RenderText(slot.medicine);
            AllSlots.Add(slot);
            OpenInventory.AddChild(newButton);
            TreatmentManager.AddSubscription(slot.button, slot.medicine);
            //medicineButtons.Add(medButton);
        }
    }

    private void SetNavigationButtonStatus()
    {
        if(ActiveSlots.Count <= InventoryInstances[1].Slots.Count)
        {
            UpButton.Disabled = true;
            DownButton.Disabled = true;
        }
        else
        {
            UpButton.Disabled = false;
            DownButton.Disabled = false;
        }
        if(inventoryIndex + InventoryInstances[1].Slots.Count > ActiveSlots.Count)
        {
            //UpButton.Disabled = true;
            DownButton.Disabled = true;
        }
        if (inventoryIndex <= 0)
        {
            UpButton.Disabled = true;
            //DownButton.Disabled = true;
        }
    }

    private void InventoryNavigation(int input)
    {
        inventoryIndex += input;
        GD.Print($"index now {inventoryIndex}");
        NewRenderMedicine(InventoryInstances[1]);
        SetNavigationButtonStatus();
    }

    public void InventoryButtonGeneration(Control inputControl, Control parent, TextureButton Template)
    {
        //Generating the buttons for the two inventories.

        //The inventory slots where the buttons will be inserted.
        List<InventorySlot> slots = new List<InventorySlot>();

        //The buttons that will be visible on screen.
        List<MedicineButton> medicineButtons = new List<MedicineButton>();

        //Generating inventory slots, based on the amount of children created in the scene
        //In Computer.tscn and Inventory.tscn
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
        //Generate the buttons themselves
        for (int i = 0; i < slots.Count; i++)
        {
            TextureButton newButton = (TextureButton)Template.Duplicate();
            MedicineButton medButton = newButton as MedicineButton;
            medButton.Initialize();
            parent.AddChild(newButton);
            medicineButtons.Add(medButton);
        }

        //Save references to buttons and their slots
        //That way we can use that info later when we need
        //to update the inventory
        InventoryUiInstance instance = new InventoryUiInstance(slots, medicineButtons);
        InventoryInstances.Add(instance);
    }
    public void InventoryActions()
    {
        //Logic used to update everything inventory related.

        //Running the update for each of the inventory instances.
        //for (int i = 0; i < InventoryInstances.Count; i++)
        for (int i = 1; i < 2; i++)
        {
            NewUpdateInventory(InventoryInstances[i]);
            SetNavigationButtonStatus();
            //DebugSlots();
            NewRenderMedicine(InventoryInstances[i]);
            //Render the medicine text
            //RenderMedicine(InventoryInstances[i]);
            //Update buttons based on medicine count
            //UpdateInventory(InventoryInstances[i]);
            //Updating the button status in case they should be disabled/enabled
            if (TreatmentManager.GetRoom() != null)
            {
                SetButtonStatus(TreatmentManager.GetRoom().GetAlreadyTreated(), InventoryInstances[i]);
            }
        }
    }
    public void SetButtonStatus(bool isActive, InventoryUiInstance instance)
    {
        //Enabling/disabling the buttons
        List<MedicineButton> buttons = instance.MedicineButtons;
        foreach(InventorySlot slot in AllSlots)
        {
            slot.button.Disabled = isActive;
        }
        /*foreach (MedicineButton button in buttons)
        {
            button.Disabled = isActive;
        }*/
    }

    private void NewUpdateInventory(InventoryUiInstance instance)
    {
        for(int i  = 0; i < MedicineManager.Database.Count; i++)
        {
            Medicine medicine = MedicineManager.Database.ElementAt(i).Value;
            if(medicine.amount > 0)
            {
                if(!CheckSlotReferences(medicine))
                {
                    InventorySlot slot = new InventorySlot();
                    slot.medicine = medicine;
                    ActiveSlots.Add(slot);
                }
            }
            else
            {
                InventorySlot slot = FindSlotByMedicine(medicine);
                if (slot != null)
                {
                    //InventorySlot slot = new InventorySlot();
                    //slot.medicine = medicine;
                    //AllSlots.Add(slot);
                    ActiveSlots.Remove(slot);
                }
            }
        }
    }
    private void NewRenderMedicine(InventoryUiInstance instance)
    {
        List<MedicineButton> buttons = instance.MedicineButtons;
        //Going through each button in the list

        foreach (InventorySlot slot in AllSlots)
        {
            slot.button.Hide();
        }
        int index = 0;
        for (int i = inventoryIndex; i < inventoryIndex + instance.Slots.Count; i++)
        {
            if(i < ActiveSlots.Count)
            {
                foreach(InventorySlot slot in AllSlots)
                {
                    if (slot.ReferenceAlreadyExists(ActiveSlots[i].medicine))
                    {
                        slot.button.Show();
                        slot.button.Position = instance.Slots[index].control.Position;
                        slot.button.RenderText(slot.medicine);
                        index++;
                    }
                }
                /*buttons[i].RenderText(ActiveSlots[i].medicine);
                buttons[i].Position = instance.Slots[i].control.Position;
                TreatmentManager.AddSubscription(buttons[i]);
                GD.Print($"Position is {instance.Slots[i].control.Position}");
                buttons[i].Show();*/
            }
        }
     }

    private void DebugSlots()
    {
        GD.Print("PRINTING ALL SLOTS");
        foreach (InventorySlot slot in ActiveSlots)
        {
            GD.Print($"Slot {slot.medicine.name}");
        }
    }

    private bool CheckSlotReferences(Medicine medicine)
    {
        foreach (InventorySlot slot in ActiveSlots)
        {
            if(slot.ReferenceAlreadyExists(medicine))
            {
                return true;
            }
        }
        return false;
    }

    private InventorySlot FindSlotByMedicine(Medicine medicine)
    {
        foreach (InventorySlot slot in ActiveSlots)
        {
            if (slot.ReferenceAlreadyExists(medicine))
            {
                return slot;
            }
        }
        return null;
    }

    
    private void InventoryToggle()
	{
        //Turn the inventory on/off.
        InventoryActions();
        OpenInventory.Visible = !OpenInventory.Visible;
    }

    //kill the patient, show the deceased sprites and disable the shotgun
    private void KillPatient()
    {
        //CURRENTLY NOT FUNCTIONAL
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
        } 
        else
        {
            ShotgunButton.Disabled = true;
        }
    }
    private void CloseInventory(Button button)
    {
        var Parent = button.GetParent<TextureRect>();
        Parent.Hide();
    }
}
