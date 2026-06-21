using Godot;
using System;

//class governing using medicine to treat the patient in the patient room
public partial class TreatmentManager : Node
{
    //Storing a reference to all the buttons, labels, etc., for easy reference in the methods
    Sprite2D PatientDisplay;
    Label PatientInfo;
    ColorRect MedicineBackground;
    Button GiveMedicine1Button;
    Button GiveMedicine2Button;
    Button GiveMedicine3Button;
    Label WrongMedicinePopup;
    Label NoPatientPopup;
    Label PatientCuredPopup;
    Label CorrectMedicinePopup;

    Room Room;

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
        Room = GetParent() as Room;

        PatientDisplay = GetParent().GetNode<Sprite2D>("Patient_Display");
        PatientInfo = GetParent().GetNode<Label>("Patient_Info");

        MedicineBackground = GetParent().GetNode<ColorRect>("Stylish_Medicine_Background");
        GiveMedicine1Button = MedicineBackground.GetNode<Button>("Give_Medicine_1");
        GiveMedicine2Button = MedicineBackground.GetNode<Button>("Give_Medicine_2");
        GiveMedicine3Button = MedicineBackground.GetNode<Button>("Give_Medicine_3");
        WrongMedicinePopup = MedicineBackground.GetNode<Label>("Wrong_Medicine_Popup");
        NoPatientPopup = MedicineBackground.GetNode<Label>("No_Patient_Popup");
        PatientCuredPopup = MedicineBackground.GetNode<Label>("Patient_Cured_Popup");
        CorrectMedicinePopup = MedicineBackground.GetNode<Label>("Correct_Medicine_Popup");
    }

    private void MedicineOperations(Button medicineChoice)
	{
        //setting up crucial parameters of a medicine, and changing them depending on which medicine is being usesd
        Medicine medicine = null;
        string matchingMalady = "none";
        PatientStats patient = Room.Patient;
        if (medicineChoice == GiveMedicine1Button)
        {
            medicine = MedicineManager.Database["Morphine"];
            matchingMalady = "Accident";
        }
        else if (medicineChoice == GiveMedicine2Button)
        {
            medicine = MedicineManager.Database["Aspirin"];
            matchingMalady = "Accident";
        }
        else if (medicineChoice == GiveMedicine3Button)
        {
            medicine = MedicineManager.Database["Ozempic"];
            matchingMalady = "Blue Pox";
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
            //if you try to use the medicine on the wrong malady, you get the appropriate popup, and the medicine buttons get disabled until you close it
            if (patient.malady.name != matchingMalady)
            {
                WrongMedicinePopup.Show();
            }
            else
            {
                //the correct use of the medicine, severity goes down, the text gets updated
                patient.malady.severity--;
                PatientInfo.Text = "Patient info: \n Malady: " + patient.malady.name + "\n Severity: " + patient.malady.severity; 
                //if you get the severity down to 0, the patient is cured, you get a popup, and you get paid
                if (patient.malady.severity <= 0)
                {
                    PatientCuredPopup.Show();
                    GlobalData.DailyEarnings += 40;
                    Room.isEmpty = true;
                } 
                else
                {
                    //give the popup about the patient needing to rest, and asking to check back in tomorrow. It needs to be there to explain to players why they can't use more medicine, 
                    //and what they need to do to fix that, but it's probably gonna get annoying if it happens every time, in the final game,
                    //something like this should probably only happen the first time
                    CorrectMedicinePopup.Show();
                }
            }
            GiveMedicine1Button.Disabled = true;
            GiveMedicine2Button.Disabled = true;
            GiveMedicine3Button.Disabled = true;
        }

    }

    public void ReenableMedicine()
    {
        GiveMedicine1Button.Disabled = false;
        GiveMedicine2Button.Disabled = false;
        GiveMedicine3Button.Disabled = false;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
