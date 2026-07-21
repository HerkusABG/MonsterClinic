using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks; // i added this for delays, but it may not be necessary 

public partial class MapUI : Control
{
    [Export] public Label RoomCount;
    [Export] public Label PatientCount;
    [Export] public Button BuyRoomButton;
	[Export] public Label WarningLabel;
    [Export] GridContainer RoomContainer1;
    [Export] GridContainer RoomContainer2;
    Label DealerWindowMoneyDisplay;
    Label RoomInfo;
    Label RoomNumber;
    Label PatientInfo;
    Button FastTravel;
    Button MapOffice;
    Button MapPatientAdmission;
    Button MapHallway;

    Label MedicineMenu;

    int currentRoomNum = 0;

    private int admittedPatients = 0;

    RoomStructureRenderer RoomRenderer;

    [Export] Control SlotControl;
    [Export] TextureButton ButtonTemplate;

    public int inventoryIndex = 0;
    [Export] Button UpButton;
    [Export] Button DownButton;

    TreatmentManager Treatment;
    Inventory Inventory;

    public void Initialize()
    {
        //resets the number of rooms to 1, prevents an issue when quitting to the main menu and starting a new game
        //should probably be in main, but the order in which the scripts are executed makes it not work, should be moved there after we have flow control
        GetNodes();
        MapOffice.Pressed += MapOfficeFunction;
        MapPatientAdmission.Pressed += MapPatientAdmissionFunction;
        MapHallway.Pressed += MapHallwayFunction;

        UpButton.Pressed += () => InventoryNavigation(-1);
        DownButton.Pressed += () => InventoryNavigation(1);

        UpdateUI();
        WarningLabel.Visible = false;
        //BuyRoomButton.Pressed += OnBuyRoomButtonPressed;
        RoomRenderer = new RoomStructureRenderer();
        //since now the rooms are generated in 2 different containers, they can no longer be generated en masse in one method, if the method will ever generate more than 3 at once,
        //so now it's a loop using the individual room generation
        for (int i = 1; i <= Upgrades.IntUpgradeDatabase["Rooms"].incrementTarget; i++)
        {
            if (i < 4)
            {
                RoomRenderer.GenerateRoom(RoomContainer1);
                AssignRoomButtonFunction(i);
            } 
            else
            {
                RoomRenderer.GenerateRoom(RoomContainer2);
                AssignRoomButtonFunction(i);
            }
        }
        DealerWindowMoneyDisplay = GetParent().GetNode<Control>("Dealer_PH").GetNode<Label>("Money_Display");
        Button CloseRoomInfoButton = GetNode<Label>("Room_Info").GetNode<Button>("Close");
        CloseRoomInfoButton.Pressed += CloseRoomInfo;
        Upgrades.IntUpgradeDatabase["Rooms"].OnUpgradePressed = BuyRoomActions;
    }

    private void GetNodes()
    {
        Treatment = GetTree().Root.GetNode("Main").GetNode("Inventory").GetNode<TreatmentManager>("Treatment_Manager");
        Inventory = Treatment.GetParent().GetParent().GetNode("Inventory") as Inventory;
        MedicineMenu = GetNode<Label>("Medicine_Menu");
        Inventory.InventoryButtonGeneration(SlotControl, MedicineMenu, ButtonTemplate);
        //grabbing all the nodes
        RoomInfo = GetNode<Label>("Room_Info");
        RoomNumber = RoomInfo.GetNode<Label>("Room_Number_Display");
        PatientInfo = RoomInfo.GetNode<Label>("Patient_Info");
        FastTravel = RoomInfo.GetNode<Button>("Fast_Travel");
        MapOffice = GetNode<Button>("Map_Office");
        MapPatientAdmission = GetNode<Button>("Map_Patient_Admission");
        MapHallway = GetNode<Button>("Map_Hallway");
        RoomContainer1 = GetNode("MapMarginContainer").GetNode<GridContainer>("RoomContainer");
        RoomContainer2 = GetNode("MapMarginContainer2").GetNode<GridContainer>("RoomContainer2");
    }

    public void OnMapUiClose()
    {
        CloseRoomInfo();
    }

    private void CloseRoomInfo()
    {
        RoomInfo.Hide();
        if (FastTravel.IsConnected(Button.SignalName.Pressed, Callable.From(RoomFastTravel)))
        {
            FastTravel.Pressed -= RoomFastTravel;
        }
        //FastTravel.Pressed -= RoomFastTravel;
    }

    private void UpdateUI()
    {
        /*BuyRoomButton.Text = $"Price: {Economy.roomCost}";
        RoomCount.Text = $"Rooms: {Upgrades.IntUpgradeDatabase["Rooms"].incrementTarget}";
        PatientCount.Text = $"Patients: {admittedPatients}";
        //disable the button if we're at max rooms
        if (Upgrades.IntUpgradeDatabase["Rooms"].incrementTarget == 6)
        {
            BuyRoomButton.Disabled = true;
        }*/
    }

    public async void BuyRoomActions()
    {
        if (Upgrades.IntUpgradeDatabase["Rooms"].incrementTarget < 4)
        {
            RoomRenderer.GenerateRoom(RoomContainer1);
        } 
        else
        {
            RoomRenderer.GenerateRoom(RoomContainer2);
        }
        AssignRoomButtonFunction(Upgrades.IntUpgradeDatabase["Rooms"].incrementTarget);
    }
    private void AssignRoomButtonFunction(int roomNum)
    {
        //connect the roomButton variable with the actual button in the scene, and assign a method to be executed when pressed
        if (roomNum < 4)
        {
            Button roomButton = GetNode("MapMarginContainer").GetNode("RoomContainer").GetNode<Button>("MapRoom" + roomNum.ToString());
            roomButton.Pressed += () => RoomButtonFunction(roomNum);
        }
        else
        {
            Button roomButton = GetNode("MapMarginContainer2").GetNode("RoomContainer2").GetNode<Button>("MapRoom" + roomNum.ToString());
            roomButton.Pressed += () => RoomButtonFunction(roomNum);
        }
    }
    private void InventoryNavigation(int input)
    {
        inventoryIndex += input;
        GD.Print($"index now {inventoryIndex}");
        Inventory.InventoryActions();
        //Inventory.NewRenderMedicine(Inventory.InventoryInstances[1], inventoryIndex);
        //SetNavigationButtonStatus(InventoryInstances[0]);
    }
    public void SetNavigationButtonStatus(InventoryUiInstance instance)
    {
        if (instance.ActiveSlots.Count <= instance.Slots.Count)
        {
            UpButton.Disabled = true;
            DownButton.Disabled = true;
        }
        else
        {
            UpButton.Disabled = false;
            DownButton.Disabled = false;
        }
        if (inventoryIndex + instance.Slots.Count > instance.ActiveSlots.Count)
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
    private void RoomFastTravel()
    {
        //check inPatientRoom to be true, hide the computer, go to the room corresponding to the most recent room button pressed
        GlobalData.inPatientRoom = true;
        Node2D currentScene = GetParent().GetParent<Node2D>();
        Node2D RoomScene = RoomManager.RoomList[currentRoomNum - 1];
        Node2D HallwayScene = currentScene.GetParent().GetNode<Node2D>("Hallway");
        Hallway hallway = HallwayScene as Hallway;
        hallway.UpdateHallwayUI();
        hallway.GoToRoom(RoomScene);
        currentScene.Hide();

        //RoomScene.Show();
        Room room = RoomScene as Room;
        //update all the stuff in the room
        //room.UpdateSprites();


        //make the buttons disabled if the patient has already been treated today
        Inventory.InventoryActions();
        Treatment.SetTreatmentRoomReference(room);
        //push the scene we're entering to the previous scenes stack
        GlobalData.PreviousScenes.Push(RoomScene.GetPath());
        //unbind the method from the fast travel button
        //FastTravel.Pressed -= RoomFastTravel;
    }

    private void OfficeFastTravel()
    {
        //hide the computer, show the office
        Node2D currentScene = GetParent().GetParent<Node2D>();
        Node2D officeScene = currentScene.GetParent().GetNode<Node2D>("Office");
        //show the patient info for future map room activities after it was hidden for the office button, easier to do it here than for every button that uses it
        PatientInfo.Show();
        currentScene.Hide();
        officeScene.Show();
        //unbind the method from the fast travel button
        //FastTravel.Pressed -= OfficeFastTravel;
    }

    private void PatientAdmissionFastTravel()
    {
        //hide the computer, show patient admission
        Node2D currentScene = GetParent().GetParent<Node2D>();
        Node2D PatientScene = currentScene.GetParent().GetNode<Node2D>("Patient_Interface");
        currentScene.Hide();
        PatientScene.Show();
        //push the scene we're entering to the previous scenes stack
        GlobalData.PreviousScenes.Push(PatientScene.GetPath());
        //unbind the method from the fast travel button
        //FastTravel.Pressed -= PatientAdmissionFastTravel;
    }

    private void HallwayFastTravel()
    {
        //hide the computer. show the hallway
        Node2D currentScene = GetParent().GetParent<Node2D>();
        Node2D HallwayScene = currentScene.GetParent().GetNode<Node2D>("Hallway");
        Hallway hallway = HallwayScene as Hallway;
        hallway.UpdateHallwayUI();
        //show the patient info for future map room activities after it was hidden for the hallway button, easier to do it here than for every button that uses it
        PatientInfo.Show();
        currentScene.Hide();
        HallwayScene.Show();
        //push the scene we're entering to the previous scenes stack
        GlobalData.PreviousScenes.Push(HallwayScene.GetPath());
        //unbind the method from the fast travel button
        //FastTravel.Pressed -= HallwayFastTravel;
    }

    private void MapOfficeFunction()
    {
        //show office related info, make the fast travel button go to the office
        RoomInfo.Show();
        PatientInfo.Hide();
        //MedicineMenu.Hide();
        RoomNumber.Text = "Office";
        //FastTravel.Pressed += OfficeFastTravel;
    }

    private void MapPatientAdmissionFunction() 
    {
        //show patient admission related info, make the fast travel button go to patient admission
        RoomInfo.Show();
       // MedicineMenu.Hide();
        RoomNumber.Text = "Patient Admission";
        Label OriginalPatientsLeftLabel = GetParent().GetParent().GetParent().GetNode("Patient_Interface").GetNode("Sprites_PH").GetNode<Label>("PatientsLeftParent").GetNode<Label>("PatientsLeftLabel");
        PatientInfo.Text = OriginalPatientsLeftLabel.Text;
        //FastTravel.Pressed += PatientAdmissionFastTravel;
    }

    private void MapHallwayFunction()
    {
        //show hallway related info, make the fast travel button go to the hallway
        RoomInfo.Show();
        PatientInfo.Hide();
        //MedicineMenu.Hide();
        RoomNumber.Text = "Hallway";
        //FastTravel.Pressed += HallwayFastTravel;
    }

    private void RoomButtonFunction(int roomNum)
    {
        //show room info, set curretnRoomNum to the room we are now dealing with
        RoomInfo.Show();
        currentRoomNum = roomNum;
        Room room = RoomManager.RoomList[roomNum - 1] as Room;
        RoomNumber.Text = "Room " + roomNum.ToString();
        //show medicine menu if the room has an untreated patient, and the player has unlocked remote medicine
        if (room.Patient != null && room.GetAlreadyTreated() == false && Upgrades.BoolUpgradeDatabase["RemoteMedicine"].unlocked)
        {
            Treatment.SetTreatmentRoomReference(room);
            Inventory.InventoryActions();
            MedicineMenu.Show();
        }
        else
        {
            Inventory.InventoryActions();
            MedicineMenu.Hide();
        }
        //if there is a patient, display patient info
        if (room.Patient != null)
        {
            PatientInfo.Show();
            UpdateComputerPatientText(room);
        } 
        else
        {
            PatientInfo.Hide();
            UpdateComputerPatientText(null);
        }
        //connect fast travel to this specific room
        if (!FastTravel.IsConnected(Button.SignalName.Pressed, Callable.From(RoomFastTravel)))
        {
            FastTravel.Pressed += RoomFastTravel;
        }
    } 

    

    public void UpdateComputerPatientText(Room room)
    {
        if(room != null && room.Patient != null)
        {
            PatientInfo.Text = $"Patient info: " +
                       $"\n Malady: {room.Patient.malady.name}" +
                       $"\n Severity: {room.Patient.malady.severity}" +
                       $"\n Age: {room.Patient.age}";
        }
        else
        {
            PatientInfo.Text = "Room currently empty";
        }
    }
    public void OnMapButtonPressed()
    {
        MedicineMenu.Hide();
    }
}