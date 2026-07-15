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
    TextureButton GiveMedicine1Button;
    Label Med1Name;
    Label Med1Count;
    TextureButton GiveMedicine2Button;
    Label Med2Name;
    Label Med2Count;
    TextureButton GiveMedicine3Button;
    Label Med3Name;
    Label Med3Count;
    Button Close;

    [Export] TextureButton ButtonTemplate;

    [Export] Control PositionControl;
    private List<InventorySlot> Slots = new List<InventorySlot>();

    private List<MedicineButton> MedicineButtons = new List<MedicineButton>();

    TreatmentManager TreatmentManager;

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
        InventoryButton = GetNode<TextureButton>("Inventory_Button");
		OpenInventory = GetNode<TextureRect>("Open_Inventory");
		ShotgunButton = OpenInventory.GetNode<Button>("Shotgun");

        /*foreach(Node node in OpenInventory.GetChildren())
        {
            TextureButton button = node as TextureButton;
            if(button != null)
            {
                MedicineButtons.Add(button);
            }
        }*/



        //GiveMedicine1Button = OpenInventory.GetNode<TextureButton>("Give_Medicine_1");
        //Med1Name = MedicineButtons[0].GetNode<Label>("Med1_Name");
        //Med1Count = MedicineButtons[0].GetNode("Stripe").GetNode<Label>("Med1_Count");

        //GiveMedicine2Button = OpenInventory.GetNode<TextureButton>("Give_Medicine_2");
        //Med2Name = MedicineButtons[1].GetNode<Label>("Med2_Name");
        //Med2Count = MedicineButtons[1].GetNode("Stripe").GetNode<Label>("Med2_Count");

        //GiveMedicine3Button = OpenInventory.GetNode<TextureButton>("Give_Medicine_3");
        //Med3Name = MedicineButtons[2].GetNode<Label>("Med3_Name");
        //Med3Count = MedicineButtons[2].GetNode("Stripe").GetNode<Label>("Med3_Count");

        Close = OpenInventory.GetNode<Button>("Close");

        foreach(Node node in PositionControl.GetChildren())
        {
            Control control = node as Control;
            if(control != null)
            {
                InventorySlot slot = new InventorySlot();
                slot.control = control;
                Slots.Add(slot);
            }
            else
            {
                GD.Print("ERROR IN INVENTORY.CS, NULL REFERENCE");
            }
        }
        for (int i = 0; i < 4; i++)
        {
            TextureButton newButton = (TextureButton)ButtonTemplate.Duplicate();
            MedicineButton medButton = newButton as MedicineButton;
            medButton.Initialize();
            OpenInventory.AddChild(newButton);
            MedicineButtons.Add(medButton);
            //newButton.Show();
            //newButton.Position = Slots[i].GetPosition();
        }
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
    private void SpawnMedicine()
    {
        for(int i = 0; i < 4; i++)
        {
            TextureButton newButton = (TextureButton)ButtonTemplate.Duplicate();
            OpenInventory.AddChild(newButton);
            newButton.Show();
            newButton.Position = Slots[i].GetPosition();
        }
    }

    public void InventoryActions()
    {
        RenderMedicine();
        UpdateInventory();
        if (TreatmentManager.GetRoom() != null)
        {
            GD.Print("not null");
            GD.Print($"Setting to: {TreatmentManager.GetRoom().GetAlreadyTreated()}");
            SetButtonStatus(TreatmentManager.GetRoom().GetAlreadyTreated());
        }
        else
        {
            GD.Print("null");
        }
    }

    private void RenderMedicine()
    {
        if (Visible)
        {
            for (int i = 0; i < MedicineButtons.Count; i++)
            {
                Medicine medicine = MedicineManager.Database.ElementAt(i).Value;
                if (MedicineButtons[i].GetIsAssigned())
                {
                    MedicineButtons[i].RenderText(medicine);
                }
            }
        }
    }

    public void UpdateInventory()
    {
        for (int i = 0; i < MedicineButtons.Count; i++)
        {
            Medicine medicine = MedicineManager.Database.ElementAt(i).Value;
            if (medicine.amount > 0)
            {
                if (!DoesButtonAlreadyExist(medicine))
                {
                    InventorySlot slot = FindEmptySlot();
                    if (slot == null) return;

                    MedicineButtons[i].AssignToSlot(slot, medicine);
                    TreatmentManager.AddSubscription(MedicineButtons[i]);
                }
            }
            else
            {
                if (DoesButtonAlreadyExist(medicine))
                {
                    TreatmentManager.RemoveSubscription(MedicineButtons[i]);
                    MedicineButtons[i].RemoveFromSlot();
                }
            }
        }
    }

    public void SetButtonStatus(bool isActive)
    {
        foreach(MedicineButton button in MedicineButtons)
        {
            button.Disabled = isActive;
        }
    }

    private bool DoesButtonAlreadyExist(Medicine inputMedicine)
    {
        for (int i = 0; i < MedicineButtons.Count; i++)
        {
            if (MedicineButtons[i].HasSameMedicineReference(inputMedicine))
            {
                return true;
            }
        }
        return false;
    }

    private InventorySlot FindEmptySlot()
    {
        foreach(InventorySlot slot in Slots)
        {
            if(!slot.GetOccupiedStatus())
            {
                return slot;
            }
        }
        return null;
    }
}
