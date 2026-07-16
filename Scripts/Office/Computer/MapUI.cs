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
    TextureButton GiveMedicine1Button;
    Label Med1Name;
    Label Med1Count;
    TextureButton GiveMedicine2Button;
    Label Med2Name;
    Label Med2Count;
    TextureButton GiveMedicine3Button;
    Label Med3Name;
    Label Med3Count;


    int currentRoomNum = 0;

    private int admittedPatients = 0;

    RoomStructureRenderer RoomRenderer;

    [Export] Control SlotControl;
    [Export] MedicineButton ButtonTemplate;

    private List<InventorySlot> Slots = new List<InventorySlot>();

    private List<MedicineButton> MedicineButtons = new List<MedicineButton>();

    TreatmentManager Treatment;
    Inventory Inventory;

    public void Initialize()
    {
        //resets the number of rooms to 1, prevents an issue when quitting to the main menu and starting a new game
        //should probably be in main, but the order in which the scripts are executed makes it not work, should be moved there after we have flow control
        getNodes();
        MapOffice.Pressed += MapOfficeFunction;
        MapPatientAdmission.Pressed += MapPatientAdmissionFunction;
        MapHallway.Pressed += MapHallwayFunction;
        GiveMedicine1Button.Pressed += () => MedicineOperations(GiveMedicine1Button);
        GiveMedicine2Button.Pressed += () => MedicineOperations(GiveMedicine2Button);
        GiveMedicine3Button.Pressed += () => MedicineOperations(GiveMedicine3Button);
        UpdateUI();
        WarningLabel.Visible = false;
        BuyRoomButton.Pressed += OnBuyRoomButtonPressed;
        RoomRenderer = new RoomStructureRenderer();
        //since now the rooms are generated in 2 different containers, they can no longer be generated en masse in one method, if the method will ever generate more than 3 at once,
        //so now it's a loop using the individual room generation
        for (int i = 1; i <= Upgrades.roomCount; i++)
        {
            if (i < 4)
            {
                RoomRenderer.GenerateRoom(RoomContainer1);
                AssignRoomButtonFunction(i);
            } else
            {
                RoomRenderer.GenerateRoom(RoomContainer2);
                AssignRoomButtonFunction(i);
            }
        }
        DealerWindowMoneyDisplay = GetParent().GetNode<Label>("Dealer_PH").GetNode<Label>("Money_Display");
        Button CloseRoomInfoButton = GetNode<Label>("Room_Info").GetNode<Button>("Close");
        CloseRoomInfoButton.Pressed += CloseRoomInfo;

    }


    private void getNodes()
    {
        Treatment = GetTree().Root.GetNode("Main").GetNode("Inventory").GetNode<TreatmentManager>("Treatment_Manager");
        Inventory = Treatment.GetParent().GetParent().GetNode("Inventory") as Inventory;
        //grabbing all the nodes
        RoomInfo = GetNode<Label>("Room_Info");
        RoomNumber = RoomInfo.GetNode<Label>("Room_Number_Display");
        PatientInfo = RoomInfo.GetNode<Label>("Patient_Info");
        //FastTravel = RoomInfo.GetNode<Button>("Fast_Travel");
        MapOffice = GetNode<Button>("Map_Office");
        MapPatientAdmission = GetNode<Button>("Map_Patient_Admission");
        MapHallway = GetNode<Button>("Map_Hallway");
        RoomContainer1 = GetNode("MapMarginContainer").GetNode<GridContainer>("RoomContainer");
        RoomContainer2 = GetNode("MapMarginContainer2").GetNode<GridContainer>("RoomContainer2");

        MedicineMenu = GetNode<Label>("Medicine_Menu");
        GiveMedicine1Button = MedicineMenu.GetNode<TextureButton>("Give_Medicine_1");
        Med1Name = GiveMedicine1Button.GetNode<Label>("Med1_Name");
        Med1Count = GiveMedicine1Button.GetNode("Stripe").GetNode<Label>("Med1_Count");
        GiveMedicine2Button = MedicineMenu.GetNode<TextureButton>("Give_Medicine_2");
        Med2Name = GiveMedicine2Button.GetNode<Label>("Med2_Name");
        Med2Count = GiveMedicine2Button.GetNode("Stripe").GetNode<Label>("Med2_Count");
        GiveMedicine3Button = MedicineMenu.GetNode<TextureButton>("Give_Medicine_3");
        Med3Name = GiveMedicine3Button.GetNode<Label>("Med3_Name");
        Med3Count = GiveMedicine3Button.GetNode("Stripe").GetNode<Label>("Med3_Count");

        //InitializeRemoteHealing();
        Inventory inv = Treatment.GetParent().GetParent().GetNode<Inventory>("Inventory");
        inv.InventoryButtonGeneration(SlotControl, MedicineMenu, ButtonTemplate);
    }

    public void InitializeRemoteHealing()
    {
        foreach (Node node in SlotControl.GetChildren())
        {
            Control control = node as Control;
            if (control != null)
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
        for (int i = 0; i < 3; i++)
        {
            MedicineButton medButton = ButtonTemplate.Duplicate() as MedicineButton;
            medButton.Initialize();
            MedicineMenu.AddChild(medButton);
            MedicineButtons.Add(medButton);
            medButton.Show();
            medButton.Position = Slots[i].GetPosition();
            Treatment.AddSubscription(medButton);
        }
    }

    private void CloseRoomInfo()
    {
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
            if (Upgrades.roomCount < 4)
            {
                RoomRenderer.GenerateRoom(RoomContainer1);
            } else
            {
                RoomRenderer.GenerateRoom(RoomContainer2);
            }
            //gives the newly created button a method to execute when pressed
            AssignRoomButtonFunction(Upgrades.roomCount);
            UpdateUI();
            Node2D currentScene = GetParent().GetParent<Node2D>();
            Node2D HallwayScene = currentScene.GetParent().GetNode<Node2D>("Hallway");
            Hallway hallway = HallwayScene as Hallway;
            hallway.UpdateHallwayUI();
            DealerWindowMoneyDisplay.Text = "Credits: " + DoctorInventory.Money.ToString();
        }
        else
        {
			WarningLabel.Visible = true;
			await Task.Delay(2000); // wait 2 seconds before hiding the warning again
			WarningLabel.Visible = false;
        }
    }

    private void RoomFastTravel()
    {
        //check inPatientRoom to be true, hide the computer, go to the room corresponding to the most recent room button pressed
        GlobalData.inPatientRoom = true;
        Node2D currentScene = GetParent().GetParent<Node2D>();
        Node2D RoomScene = RoomManager.RoomList[currentRoomNum - 1];
        currentScene.Hide();
        RoomScene.Show();
        Room room = RoomScene as Room;
        //update all the stuff in the room
        room.UpdateSprites();
        Node2D HallwayScene = currentScene.GetParent().GetNode<Node2D>("Hallway");
        Hallway hallway = HallwayScene as Hallway;
        hallway.UpdateHallwayUI();
        Inventory inv = GetParent().GetParent().GetParent().GetNode<Inventory>("Inventory");
        TextureButton GiveMedicine1Button = inv.GetNode("Open_Inventory").GetNode<TextureButton>("Give_Medicine_1");
        TextureButton GiveMedicine2Button = inv.GetNode("Open_Inventory").GetNode<TextureButton>("Give_Medicine_2");
        TextureButton GiveMedicine3Button = inv.GetNode("Open_Inventory").GetNode<TextureButton>("Give_Medicine_3");
        //make the buttons disabled if the patient has already been treated today
        if (room.GetAlreadyTreated())
        {
            GiveMedicine1Button.Disabled = true;
            GiveMedicine2Button.Disabled = true;
            GiveMedicine3Button.Disabled = true;
        }
        TreatmentManager treatment = inv.GetNode<TreatmentManager>("Treatment_Manager");
        treatment.SetTreatmentRoomReference(room);
        treatment.ShowUI();
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

    private void MapPatientAdmissionFunction() {
        //show patient admission related info, make the fast travel button go to patient admission
        RoomInfo.Show();
       // MedicineMenu.Hide();
        RoomNumber.Text = "Patient Admission";
        Label OriginalPatientsLeftLabel = GetParent().GetParent().GetParent().GetNode("Patient_Interface").GetNode("Sprites_PH").GetNode<Label>("PatientsLeftLabel");
        PatientInfo.Text = OriginalPatientsLeftLabel.Text;
        FastTravel.Pressed += PatientAdmissionFastTravel;
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
        if (room.Patient != null && room.GetAlreadyTreated() == false && Upgrades.remoteMedicine.unlocked)
        {
            Inventory.InventoryActions();
            MedicineMenu.Show();
            foreach(Control child in MedicineMenu.GetChildren())
            {
                GD.Print(child.Name);
            }
        }
        else
        {
            Inventory.InventoryActions();
            MedicineMenu.Hide();
        }
        //if there is a patient, display patient info
        if (room.Patient != null)
        {
            PatientInfo.Text = $"Patient info: " +
                       $"\n Malady: {room.Patient.malady.name}" +
                       $"\n Severity: {room.Patient.malady.severity}" +
                       $"\n Age: {room.Patient.age}";
        } else
        //else, this stuff=
        {
            PatientInfo.Text = "Room currently empty";
        }
        //connect fast travel to this specific room
        //FastTravel.Pressed += RoomFastTravel;
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

    //this is basically a copy of MedicineOperations in the treatment manager, modified to work with the medicine buttons here
    private void MedicineOperations(TextureButton medicineChoice)
    {
        //setting up crucial parameters of a medicine, and changing them depending on which medicine is being usesd
        Medicine medicine = null;
        string matchingMalady = "none";
        Room room = RoomManager.RoomList[currentRoomNum - 1] as Room;
        PatientStats patient = room.Patient;
        Label nameBox = null;
        Label countBox = null;
        if (medicineChoice == GiveMedicine1Button)
        {
            medicine = MedicineManager.Database["Morphine"];
            matchingMalady = "an accident";
            nameBox = Med1Name;
            countBox = Med1Count;
        }
        else if (medicineChoice == GiveMedicine2Button)
        {
            medicine = MedicineManager.Database["Aspirin"];
            matchingMalady = "an ccident";
            nameBox = Med2Name;
            countBox = Med2Count;
        }
        else if (medicineChoice == GiveMedicine3Button)
        {
            medicine = MedicineManager.Database["Ozempic"];
            matchingMalady = "Blue Pox";
            nameBox = Med3Name;
            countBox = Med3Count;
        }
        //else if (GlobalData.Medicine1Count > 0)
        if (medicine.amount > 0)
        {
            //use the medicine
            medicine.amount--;
            nameBox.Text = $"{medicine.name}";
            countBox.Text = $"{medicine.amount}";
            //if we're out of the medicine, remove it from the inventory
            if (medicine.amount == 0)
            {
                medicineChoice.Hide();
            }
            if (patient.malady.name == matchingMalady)
            {
                //the correct use of the medicine, severity goes down, the text gets updated
                patient.malady.severity--;
                if (room == null) return;
                if (room.Patient == null) return;
                PatientInfo.Text = $"Patient info: " +
                           $"\n Malady: {room.Patient.malady.name}" +
                           $"\n Severity: {room.Patient.malady.severity}" +
                           $"\n Age: {room.Patient.age}";
                //PatientInfo.Text = "Patient info: \n Malady: " + patient.malady.name + "\n Severity: " + patient.malady.severity; 
                //if you get the severity down to 0, the patient is cured, you get a popup, and you get paid
                if (patient.malady.severity <= 0)
                {
                    GlobalData.patientCount--;
                    GlobalData.DailyEarnings += 40;
                    PatientInfo.Text = "Room currently empty";
                    //MedicineMenu.Hide();
                    room.DeletePatient();
                }
            }
            //disable the buttons, and prevent them form being reenabled by switching scenes until the lockout is disabled by going to bed
            room.SetAlreadyTreated(false);
            
            //MedicineMenu.Hide();
        }
    }

    public void OnMapButtonPressed()
    {
        MedicineMenu.Hide();
    }

    //a whole lotta ensuring the medicine buttons show the correct data
    private void _on_visibility_changed()
    {
       /* MedicineMenu.Hide();
        if (GiveMedicine1Button == null) return;
        if (Visible)
        {
            Label RoomInfo = GetNode<Label>("Room_Info");
            RoomInfo.Hide();
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
        }*/
    }
}