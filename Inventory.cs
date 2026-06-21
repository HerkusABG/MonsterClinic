using Godot;
using System;

public partial class Inventory : Node2D
{
    //Storing a reference to all the buttons, labels, etc., for easy reference in the methods
    Button InventoryButton;
	ColorRect OpenInventory;
	Button ShotgunButton;
    Button GiveMedicine1Button;
    Button GiveMedicine2Button;
    Button GiveMedicine3Button;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        //grabs references to all the necessary nodes
        GetNodes();

        //assigning methods to all the buttons
        InventoryButton.Pressed += InventoryToggle;
        ShotgunButton.Pressed += KillPatient;

        //disables the shotgun until we're in the patient room
        ShotgunButton.Disabled = true;
	}


	private void GetNodes()
	{
        //Basically just grabbing all the nodes
        InventoryButton = GetNode<Button>("Inventory_Button");
		OpenInventory = GetNode<ColorRect>("Open_Inventory");
		ShotgunButton = OpenInventory.GetNode<Button>("Shotgun");
        GiveMedicine1Button = OpenInventory.GetNode<Button>("Give_Medicine_1");
        GiveMedicine2Button = OpenInventory.GetNode<Button>("Give_Medicine_2");
        GiveMedicine3Button = OpenInventory.GetNode<Button>("Give_Medicine_3");
    }

    //press i to toggle the inventory
    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventKey eventKey)
        {
            //if a key is pressed and that key is i
            if (eventKey.Pressed && eventKey.Keycode == Key.I)
            {
                OpenInventory.Visible = !OpenInventory.Visible;
            }

        }
    }
    //you can also press the inventory to open it
	private void InventoryToggle()
	{
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

    //update text whenever the inventory is shown, and also show or hide the medicines depending on if we have any
    private void _on_visibility_changed()
    {
        if (Visible == true)
        {
            if (MedicineManager.Database["Morphine"].amount > 0)
            {
                GiveMedicine1Button.Show();
            }
            else
            {
                GiveMedicine1Button.Hide();
            }
            if (MedicineManager.Database["Aspirin"].amount > 0)
            {
                GiveMedicine2Button.Show();
            }
            else
            {
                GiveMedicine2Button.Hide();
            }
            if (MedicineManager.Database["Ozempic"].amount > 0)
            {
                GiveMedicine3Button.Show();
            }
            else
            {
                GiveMedicine3Button.Hide();
            }
            GiveMedicine1Button.Text = $"{MedicineManager.Database["Morphine"].name} \n Owned: {MedicineManager.Database["Morphine"].amount}";
            GiveMedicine2Button.Text = $"{MedicineManager.Database["Aspirin"].name} \n Owned: {MedicineManager.Database["Aspirin"].amount}";
            GiveMedicine3Button.Text = $"{MedicineManager.Database["Ozempic"].name} \n Owned: {MedicineManager.Database["Ozempic"].amount}";
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
