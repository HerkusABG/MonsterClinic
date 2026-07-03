using Godot;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
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


    public override void _Ready()
    {
        UpdateUI();
        WarningLabel.Visible = false;
        BuyRoomButton.Pressed += OnBuyRoomButtonPressed;
        RoomRenderer = new RoomStructureRenderer();
        RoomRenderer.GenerateRooms(RoomContainer, Upgrades.roomCount);
        for (int i = 1; i <= Upgrades.roomCount; i++)
        {
            AssignRoomButtonFunction(i);
        }
        DealerWindowMoneyDisplay = GetParent().GetNode<Label>("Dealer_PH").GetNode<Label>("Money_Display");
        Button CloseRoomInfoButton = GetNode<Label>("Room_Info").GetNode<Button>("Close");
        CloseRoomInfoButton.Pressed += CloseRoomInfo;
    }

    private void CloseRoomInfo()
    {
        Label RoomInfo = GetNode<Label>("Room_Info");
        RoomInfo.Hide();
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
            //var test = RoomContainer.GetNode("newButton");
            GD.Print(RoomContainer.GetChildren().ToString());
            AssignRoomButtonFunction(Upgrades.roomCount);
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

    private void FastTravelFunction(int roomNum)
    {
        GlobalData.inPatientRoom = true;
        GD.Print("going to room number " + roomNum);
        Node2D currentScene = GetParent().GetParent<Node2D>();
        Node2D RoomScene = RoomManager.RoomList[roomNum - 1];
        currentScene.Hide();
        RoomScene.Show();
        Room room = RoomScene as Room;
        room.UpdatePatientInfoLabel();
        Inventory inv = GetParent().GetParent().GetParent().GetNode<Inventory>("Inventory");
        TreatmentManager treatment = inv.GetNode<TreatmentManager>("Treatment_Manager");
        treatment.SetTreatmentRoomReference(room);
        treatment.ShowUI();
        //push the scene we're entering to the previous scenes stack
        GlobalData.PreviousScenes.Push(RoomScene.GetPath());
    }

    private void RoomButtonFunction(int roomNum)
    {
        Label RoomInfo = GetNode<Label>("Room_Info");
        Label RoomNumber = RoomInfo.GetNode<Label>("Room_Number_Display");
        Label PatientInfo = RoomInfo.GetNode<Label>("Patient_Info");
        Button FastTravel = RoomInfo.GetNode<Button>("Fast_Travel");
        RoomInfo.Show();
        Room room = RoomManager.RoomList[roomNum - 1] as Room;
        RoomNumber.Text = "Room " + roomNum.ToString();
        if (room.Patient != null)
        {
            PatientInfo.Text = $"Patient info: " +
                       $"\n Malady: {room.Patient.malady.name}" +
                       $"\n Severity: {room.Patient.malady.severity}" +
                       $"\n Age: {room.Patient.age}";
        } else
        {
            PatientInfo.Text = "Room currently empty";
        }
        FastTravel.Pressed += () => FastTravelFunction(roomNum);
    } 

    private void AssignRoomButtonFunction(int roomNum)
    {
        Button roomButton = GetNode("MapMarginContainer").GetNode("RoomContainer").GetNode<Button>("MapRoom" + roomNum.ToString());
        roomButton.Pressed += () => RoomButtonFunction(roomNum);
    }

    private void _on_visibility_changed()
    {
        if (Visible)
        {
            Label RoomInfo = GetNode<Label>("Room_Info");
            RoomInfo.Hide();
        }
    }
}