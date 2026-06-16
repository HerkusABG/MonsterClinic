using Godot;
using System;
using System.Threading.Tasks; // i added this for delays, but it may not be necessary 

public partial class MapUI : Control
{
    [Export] public Label RoomCount;
    [Export] public Label Funds;
    [Export] public Label PatientCount;
    [Export] public Button BuyRoomButton;
	[Export] public Label WarningLabel;
    [Export] GridContainer RoomContainer;
   
    private int admittedPatients = 0;

    RoomStructureRenderer RoomRenderer;


    public override void _Ready()
    {
        UpdateUI();
        WarningLabel.Visible = false;
        BuyRoomButton.Pressed += OnBuyRoomButtonPressed;
        RoomRenderer = new RoomStructureRenderer();
        RoomRenderer.GenerateRooms(RoomContainer, Upgrades.roomCount);
    }

    private void UpdateUI()
    {
        BuyRoomButton.Text = $"Price: {Economy.roomCost}";
        RoomCount.Text = $"Rooms: {Upgrades.roomCount}";
        Funds.Text = $"Funds: {DoctorInventory.Money} credits";
        PatientCount.Text = $"Patients: {admittedPatients}";
    }

    public async void OnBuyRoomButtonPressed()
    {
        if (DoctorInventory.Money >= Economy.roomCost)
        {
            DoctorInventory.Money -= Economy.roomCost;
            Upgrades.AddNewRoom();
            RoomRenderer.GenerateRoom(RoomContainer);
            UpdateUI();
        }
        else
        {
			WarningLabel.Visible = true;
			await Task.Delay(2000); // wait 2 sseconds before hiding the warning again
			WarningLabel.Visible = false;
        }
    }
}