using Godot;
using System;
using System.Threading.Tasks; // i added this for delays, but it may not be necessary 

public partial class MapUI : Control
{
    [Export] public Label RoomCount;
    [Export] public Label PatientCount;
    [Export] public Button BuyRoomButton;
	[Export] public Label WarningLabel;
    [Export] GridContainer RoomContainer;
    Label DealerWindowMoneyDisplay;
   
    private int admittedPatients = 0;

    RoomStructureRenderer RoomRenderer;


    public void Initialize()
    {
        UpdateUI();
        WarningLabel.Visible = false;
        BuyRoomButton.Pressed += OnBuyRoomButtonPressed;
        RoomRenderer = new RoomStructureRenderer();
        RoomRenderer.GenerateRooms(RoomContainer, Upgrades.roomCount);
        DealerWindowMoneyDisplay = GetParent().GetNode<Label>("Dealer_PH").GetNode<Label>("Money_Display");
    }

    private void UpdateUI()
    {
        BuyRoomButton.Text = $"Price: {Economy.roomCost}";
        RoomCount.Text = $"Rooms: {Upgrades.roomCount}";
        PatientCount.Text = $"Patients: {admittedPatients}";
        //disable the button if we're at max rooms
        if (Upgrades.roomCount == 6)
        {
            BuyRoomButton.Disabled = true;
        }
    }

    public async void OnBuyRoomButtonPressed()
    {
        if (DoctorInventory.Money >= Economy.roomCost)
        {
            DoctorInventory.Money -= Economy.roomCost;
            Upgrades.AddNewRoom();
            RoomRenderer.GenerateRoom(RoomContainer);
            UpdateUI();
            DealerWindowMoneyDisplay.Text = "Credits: " + DoctorInventory.Money.ToString();
        }
        else
        {
			WarningLabel.Visible = true;
			await Task.Delay(2000); // wait 2 sseconds before hiding the warning again
			WarningLabel.Visible = false;
        }
    }
}