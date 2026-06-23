using Godot;
using System;

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

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        //grabs references to all the necessary nodes
        GetNodes();

        //assigning methods to all the buttons
        InventoryButton.Pressed += InventoryToggle;
        ShotgunButton.Pressed += KillPatient;
        Close.Pressed += () => CloseInventory(Close);

        //disables the shotgun until we're in the patient room
        ShotgunButton.Disabled = true;
	}


	private void GetNodes()
	{
        //Basically just grabbing all the nodes
        InventoryButton = GetNode<TextureButton>("Inventory_Button");
		OpenInventory = GetNode<TextureRect>("Open_Inventory");
		ShotgunButton = OpenInventory.GetNode<Button>("Shotgun");
        GiveMedicine1Button = OpenInventory.GetNode<TextureButton>("Give_Medicine_1");
        Med1Name = GiveMedicine1Button.GetNode<Label>("Med1_Name");
        Med1Count = GiveMedicine1Button.GetNode("Stripe").GetNode<Label>("Med1_Count");
        GiveMedicine2Button = OpenInventory.GetNode<TextureButton>("Give_Medicine_2");
        Med2Name = GiveMedicine2Button.GetNode<Label>("Med2_Name");
        Med2Count = GiveMedicine2Button.GetNode("Stripe").GetNode<Label>("Med2_Count");
        GiveMedicine3Button = OpenInventory.GetNode<TextureButton>("Give_Medicine_3");
        Med3Name = GiveMedicine3Button.GetNode<Label>("Med3_Name");
        Med3Count = GiveMedicine3Button.GetNode("Stripe").GetNode<Label>("Med3_Count");
        Close = OpenInventory.GetNode<Button>("Close");
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
    private void CloseInventory(Button button)
    {
        var Parent = button.GetParent<TextureRect>();
        Parent.Hide();
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
            Med1Name.Text = $"{MedicineManager.Database["Morphine"].name}";
            Med1Count.Text = $"{MedicineManager.Database["Morphine"].amount}";
            Med2Name.Text = $"{MedicineManager.Database["Aspirin"].name} ";
            Med2Count.Text = $"{MedicineManager.Database["Aspirin"].amount}";
            Med3Name.Text = $"{MedicineManager.Database["Ozempic"].name}";
            Med3Count.Text = $"{MedicineManager.Database["Ozempic"].amount}";
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
