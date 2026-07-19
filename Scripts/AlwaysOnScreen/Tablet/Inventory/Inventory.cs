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
        for (int i = 0; i < 3; i++)
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
        for (int i = 0; i < InventoryInstances.Count; i++)
        {
            //Render the medicine text
            RenderMedicine(InventoryInstances[i]);
            //Update buttons based on medicine count
            UpdateInventory(InventoryInstances[i]);
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
        foreach (MedicineButton button in buttons)
        {
            button.Disabled = isActive;
        }
    }

    public void UpdateInventory(InventoryUiInstance instance)
    {
        List<MedicineButton> buttons = instance.MedicineButtons;
        //For each button on the list
        for (int i = 0; i < buttons.Count; i++)
        {
            Medicine medicine = MedicineManager.Database.ElementAt(i).Value;
            //Medicine count larger than 0?
            if (medicine.amount > 0)
            {
                //If yes, then does another button already
                //show the same type of medicine?
                if (!DoesButtonAlreadyExist(medicine, instance))
                {
                    //If no, then assign a new button to represent the medicine
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
                //Medicine count is 0 or less
                if (DoesButtonAlreadyExist(medicine, instance))
                {
                    //If the button exists, it should stop being rendered.
                    TreatmentManager.RemoveSubscription(instance.MedicineButtons[i]);
                    buttons[i].RemoveFromSlot();
                }
            }
        }
    }
    //you can also press the inventory to open it
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

    

    private void RenderMedicine(InventoryUiInstance instance)
    {
        List<MedicineButton> buttons = instance.MedicineButtons;
        //Going through each button in the list
        for (int i = 0; i < buttons.Count; i++)
        {
            Medicine medicine = MedicineManager.Database.ElementAt(i).Value;
            //Is the button actively in use by the inventory?
            if (buttons[i].GetIsAssigned())
            {
                //If yes, update the text
                buttons[i].RenderText(medicine);
            }
        }
    }

    

    

    private bool DoesButtonAlreadyExist(Medicine inputMedicine, InventoryUiInstance instance)
    {
        //Checking to see if a button representing a particular medicine
        //type already existss
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
        //Find an empty slot to put the button in.
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
