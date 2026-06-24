using Godot;
using System;

//class governing using medicine to treat the patient in the patient room
public partial class TreatmentManager : Node
{
    //Storing a reference to all the buttons, labels, etc., for easy reference in the methods
    Sprite2D PatientDisplay;
    Label PatientInfo;
    TextureButton GiveMedicine1Button;
    Label Med1Name;
    Label Med1Count;
    TextureButton GiveMedicine2Button;
    Label Med2Name;
    Label Med2Count;
    TextureButton GiveMedicine3Button;
    Label Med3Name;
    Label Med3Count;
    Label WrongMedicinePopup;
    Label NoPatientPopup;
    Label PatientCuredPopup;
    Label CorrectMedicinePopup;
    Button CloseWrongMedicinePopup;
    Button CloseNoPatientPopup;
    Button ClosePatientCuredPopup;
    Button CloseCorrectMedicinePopup;

    private Room Room;
    

    public void Initialize()
    {
        GetNodes();

        GiveMedicine1Button.Pressed += () => MedicineOperations(GiveMedicine1Button);
        GiveMedicine2Button.Pressed += () => MedicineOperations(GiveMedicine2Button);
        GiveMedicine3Button.Pressed += () => MedicineOperations(GiveMedicine3Button);

        

        

        CloseWrongMedicinePopup.Pressed += () => CloseParent(CloseWrongMedicinePopup);
        CloseNoPatientPopup.Pressed += () => CloseParent(CloseNoPatientPopup);
        ClosePatientCuredPopup.Pressed += () => CloseParent(ClosePatientCuredPopup);
        CloseCorrectMedicinePopup.Pressed += () => CloseParent(CloseCorrectMedicinePopup);
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        //grabs references to all the necessary nodes
        //GetNodes();

        //assigning methods to all the buttons, yes this looks kinda wacky, but apparently that's how I gotta write it if I want to have methods that take arguments
        //GiveMedicine1Button.Pressed += () => MedicineOperations(GiveMedicine1Button);
        //GiveMedicine2Button.Pressed += () => MedicineOperations(GiveMedicine2Button);
        //GiveMedicine3Button.Pressed += () => MedicineOperations(GiveMedicine3Button);
    }
    private void GetNodes()
    {
        //Basically just grabbing all the nodes
        PatientDisplay = GetNode<Sprite2D>("Patient_Display");
        PatientInfo = GetNode<Label>("Patient_Info");
        //GiveMedicine1Button = GetParent().GetNode("Inventory").GetNode("Open_Inventory").GetNode<TextureButton>("Give_Medicine_1");
        GiveMedicine1Button = GetParent().GetNode("Open_Inventory").GetNode<TextureButton>("Give_Medicine_1");
        Med1Name = GiveMedicine1Button.GetNode<Label>("Med1_Name");
        Med1Count = GiveMedicine1Button.GetNode("Stripe").GetNode<Label>("Med1_Count");
        GiveMedicine2Button = GetParent().GetNode("Open_Inventory").GetNode<TextureButton>("Give_Medicine_2");
        Med2Name = GiveMedicine2Button.GetNode<Label>("Med2_Name");
        Med2Count = GiveMedicine2Button.GetNode("Stripe").GetNode<Label>("Med2_Count");
        GiveMedicine3Button = GetParent().GetNode("Open_Inventory").GetNode<TextureButton>("Give_Medicine_3");
        Med3Name = GiveMedicine3Button.GetNode<Label>("Med3_Name");
        Med3Count = GiveMedicine3Button.GetNode("Stripe").GetNode<Label>("Med3_Count");
        WrongMedicinePopup = GetNode<Label>("Wrong_Medicine_Popup");
        NoPatientPopup = GetNode<Label>("No_Patient_Popup");
        PatientCuredPopup = GetNode<Label>("Patient_Cured_Popup");
        CorrectMedicinePopup = GetNode<Label>("Correct_Medicine_Popup");

        CloseWrongMedicinePopup = WrongMedicinePopup.GetNode<Button>("Close_Wrong_medicine_Popup");
        CloseNoPatientPopup = NoPatientPopup.GetNode<Button>("Close");
        ClosePatientCuredPopup = PatientCuredPopup.GetNode<Button>("Close_Patient_Cured_Popup");
        CloseCorrectMedicinePopup = CorrectMedicinePopup.GetNode<Button>("Close_Correct_medicine_Popup");
    }

    private void CloseParent(Button button)
    {
        var Parent = button.GetParent();
        if (Parent.GetClass() == "Label")
        {
            Label ParentLabel = (Label)Parent;
            ParentLabel.Hide();
        }
        if (Parent.GetClass() == "Control")
        {
            var ControlParent = (Control)Parent;
            ControlParent.Hide();
        }


        //in this specific case, we also remove the patient and reset patient malady data
        if (button == ClosePatientCuredPopup)
        {
            PatientDisplay.Hide();
            PatientInfo.Hide();

            //GlobalData.CurrentPatientMalady = "none";
            //GlobalData.CurrentPatientSeverity = 0;
        }
    }

    public void ShowUI()
    {
        if(Room != null)
        {
            if (Room.Patient != null)
            {
                PatientInfo.Show();
                PatientDisplay.Show();
            }
        }
        UpdateTreatmentText();
    }
    public void UpdateTreatmentText()
    {
        if (Room == null) return;
        if (Room.Patient == null) return;
        PatientInfo.Text = $"Patient info: " +
                   $"\n Malady: {Room.Patient.malady.name}" +
                   $"\n Severity: {Room.Patient.malady.severity}" +
                   $"\n Age: {Room.Patient.age}";
    }
    public void HideUI()
    {
        foreach(Node child in GetChildren())
        {
            Node2D child2d = child as Node2D;
            if(child2d != null)
            {
                child2d.Hide();
            }
            Control controlChild = child as Control;
            if (controlChild != null)
            {
                controlChild.Hide();
            }
        }
    }

    private void MedicineOperations(TextureButton medicineChoice)
	{
        //setting up crucial parameters of a medicine, and changing them depending on which medicine is being usesd
        Medicine medicine = null;
        string matchingMalady = "none";
        PatientStats patient = Room.Patient;
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

        if (PatientDisplay.Visible == false)
        {
            NoPatientPopup.Show();
        }
        //else if (GlobalData.Medicine1Count > 0)
        else if (medicine.amount > 0)
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
            //if you try to use the medicine on the wrong malady, you get the appropriate popup, and the medicine buttons get disabled until you close it
            if (patient.malady.name != matchingMalady)
            {
                WrongMedicinePopup.Show();
            }
            else
            {
                //the correct use of the medicine, severity goes down, the text gets updated
                patient.malady.severity--;
                UpdateTreatmentText();
                GD.Print($"Room: {Room.myIndex}");
                //PatientInfo.Text = "Patient info: \n Malady: " + patient.malady.name + "\n Severity: " + patient.malady.severity; 
                //if you get the severity down to 0, the patient is cured, you get a popup, and you get paid
                if (patient.malady.severity <= 0)
                {
                    GlobalData.patientCount--;
                    PatientCuredPopup.Show();
                    GlobalData.DailyEarnings += 40;
                    Room.isEmpty = true;
                    Room.DeletePatient();
                } 
                else
                {
                    //give the popup about the patient needing to rest, and asking to check back in tomorrow. It needs to be there to explain to players why they can't use more medicine, 
                    //and what they need to do to fix that, but it's probably gonna get annoying if it happens every time, in the final game,
                    //something like this should probably only happen the first time
                    CorrectMedicinePopup.Show();
                }
            }
            //disable the buttons, and prevent them form being reenabled by switching scenes until the lockout is disabled by going to bed
            GiveMedicine1Button.Disabled = true;
            GiveMedicine2Button.Disabled = true;
            GiveMedicine3Button.Disabled = true;
            Room.alreadyTreated = true;
            GlobalData.DailyLockout = true;
        }
    }

    public void ReenableMedicine()
    {
        GiveMedicine1Button.Disabled = false;
        GiveMedicine2Button.Disabled = false;
        GiveMedicine3Button.Disabled = false;
    }

    public void SetTreatmentRoomReference(Room room)
    {
        Room = room;
    }

    public Room GetRoom()
    {
        return Room;
    }
}
