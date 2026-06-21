using Godot;
using System;

//class governing using medicine to treat the patient in the patient room
public partial class TreatmentManager : Node
{
    //Storing a reference to all the buttons, labels, etc., for easy reference in the methods
    Sprite2D PatientDisplay;
    Label PatientInfo;
    Button GiveMedicine1Button;
    Button GiveMedicine2Button;
    Button GiveMedicine3Button;
    Label WrongMedicinePopup;
    Label NoPatientPopup;
    Label PatientCuredPopup;
    Label CorrectMedicinePopup;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        //grabs references to all the necessary nodes
        GetNodes();

        //assigning methods to all the buttons, yes this looks kinda wacky, but apparently that's how I gotta write it if I want to have methods that take arguments
        GiveMedicine1Button.Pressed += () => MedicineOperations(GiveMedicine1Button);
        GiveMedicine2Button.Pressed += () => MedicineOperations(GiveMedicine2Button);
        GiveMedicine3Button.Pressed += () => MedicineOperations(GiveMedicine3Button);
    }
    private void GetNodes()
    {
        //Basically just grabbing all the nodes
        PatientDisplay = GetParent().GetNode<Sprite2D>("Patient_Display");
        PatientInfo = GetParent().GetNode<Label>("Patient_Info");

        GiveMedicine1Button = GetParent().GetParent().GetNode("Inventory").GetNode("Open_Inventory").GetNode<Button>("Give_Medicine_1");
        GiveMedicine2Button = GetParent().GetParent().GetNode("Inventory").GetNode("Open_Inventory").GetNode<Button>("Give_Medicine_2");
        GiveMedicine3Button = GetParent().GetParent().GetNode("Inventory").GetNode("Open_Inventory").GetNode<Button>("Give_Medicine_3");
        WrongMedicinePopup = GetParent().GetNode<Label>("Wrong_Medicine_Popup");
        NoPatientPopup = GetParent().GetNode<Label>("No_Patient_Popup");
        PatientCuredPopup = GetParent().GetNode<Label>("Patient_Cured_Popup");
        CorrectMedicinePopup = GetParent().GetNode<Label>("Correct_Medicine_Popup");
    }

    private void MedicineOperations(Button medicineChoice)
	{
        //setting up crucial parameters of a medicine, and changing them depending on which medicine is being usesd
        Medicine medicine = null;
        string matchingMalady = "none";
        if (medicineChoice == GiveMedicine1Button)
        {
            medicine = MedicineManager.Database["Morphine"];
            matchingMalady = "A";
        }
        else if (medicineChoice == GiveMedicine2Button)
        {
            medicine = MedicineManager.Database["Aspirin"];
            matchingMalady = "B";
        }
        else if (medicineChoice == GiveMedicine3Button)
        {
            medicine = MedicineManager.Database["Ozempic"];
            matchingMalady = "C";
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
            medicineChoice.Text = $"{medicine.name} \n Owned: {medicine.amount}";
            //if we're out of the medicine, remove it from the inventory
            if (medicine.amount == 0)
            {
                medicineChoice.Hide();
            }
            //if you try to use the medicine on the wrong malady, you get the appropriate popup, and the medicine buttons get disabled until you close it
            if (GlobalData.CurrentPatientMalady != matchingMalady)
            {
                WrongMedicinePopup.Show();
            }
            else
            {
                //the correct use of the medicine, severity goes down, the text gets updated
                GlobalData.CurrentPatientSeverity -= 1;
                PatientInfo.Text = "Patient info: \n Malady: Malady " + GlobalData.CurrentPatientMalady + "\n Severity: " + GlobalData.CurrentPatientSeverity; 
                //if you get the severity down to 0, the patient is cured, you get a popup, and you get paid
                if (GlobalData.CurrentPatientSeverity == 0)
                {
                    PatientCuredPopup.Show();
                    GlobalData.DailyEarnings += 40;
                } else
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
            GlobalData.DailyLockout = true;
        }

    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
