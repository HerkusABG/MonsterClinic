using Godot;
using System;
using System.Collections.Generic;

//class governing using medicine to treat the patient in the patient room
public partial class TreatmentManager : Node
{

    //Storing a reference to all the buttons, labels, etc., for easy reference in the methods
    Sprite2D PatientDisplay;
    Label PatientInfo;

    Label PatientImmunePopup;
    Label WrongMedicinePopup;
    Label NoPatientPopup;
    Label PatientCuredPopup;
    Label CorrectMedicinePopup;
    Button ClosePatientImmunePopup;
    Button CloseWrongMedicinePopup;
    Button CloseNoPatientPopup;
    Button ClosePatientCuredPopup;
    Button CloseCorrectMedicinePopup;

    private Room Room = null;

    Inventory Inventory;

    private readonly Dictionary<MedicineButton, Action> Subscriptions = new();

    //MapUi reference so that the treatment there is linked with 
    //the treatment in the inventory
    MapUI MapUi;

    public void Initialize()
    {
        GetNodes();

        CloseWrongMedicinePopup.Pressed += () => CloseParent(CloseWrongMedicinePopup);
        CloseNoPatientPopup.Pressed += () => CloseParent(CloseNoPatientPopup);
        ClosePatientCuredPopup.Pressed += () => CloseParent(ClosePatientCuredPopup);
        CloseCorrectMedicinePopup.Pressed += () => CloseParent(CloseCorrectMedicinePopup);
        ClosePatientImmunePopup.Pressed += () => CloseParent(ClosePatientImmunePopup);
    }

    private void GetNodes()
    {
        //Basically just grabbing all the nodes
        Inventory = GetParent() as Inventory;
        MapUi = Inventory.MapUi;
        WrongMedicinePopup = GetNode<Label>("Wrong_Medicine_Popup");
        PatientImmunePopup = GetNode<Label>("Patient_Immune_Popup");
        NoPatientPopup = GetNode<Label>("No_Patient_Popup");
        PatientCuredPopup = GetNode<Label>("Patient_Cured_Popup");
        CorrectMedicinePopup = GetNode<Label>("Correct_Medicine_Popup");

        ClosePatientImmunePopup = PatientImmunePopup.GetNode<Button>("Close_Patient_Immune_Popup");
        CloseWrongMedicinePopup = WrongMedicinePopup.GetNode<Button>("Close_Wrong_medicine_Popup");
        CloseNoPatientPopup = NoPatientPopup.GetNode<Button>("Close");
        ClosePatientCuredPopup = PatientCuredPopup.GetNode<Button>("Close_Patient_Cured_Popup");
        CloseCorrectMedicinePopup = CorrectMedicinePopup.GetNode<Button>("Close_Correct_medicine_Popup");
    }
    public void AddSubscription(MedicineButton button, Medicine inputMedicine)
    {
        //Adding a subscription to a medicine button.
        //This will then apply medicine based on the button pressed.
        Action handler = () => ApplyMedicine(inputMedicine);

        Subscriptions[button] = handler;
        button.Pressed += handler;
    }

    public void RemoveSubscription(MedicineButton button)
    {
        //Removing the subscription.
        if (Subscriptions.TryGetValue(button, out var handler))
        {
            button.Pressed -= handler;
            Subscriptions.Remove(button);
        }
    }

    public void SetTreatmentRoomReference(Room room)
    {
        //Used for connecting the treatment functinality to
        //the individual rooms
        Room = room;
        //In case the patient got better overnight, let the patient go.
        if (Room.curedInAbsence)
        {

            PatientCured();
        }
    }

    public Room GetRoom()
    {
        //Grabbing the room reference
        return Room;
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

    private void ApplyMedicine(Medicine inputMedicine)
    {
        //The new and improved, modular version of MedicineOperations.
        //Triggered on button press.
        //Grabbing the medicine type, which is stored inside of the button.
        Medicine medicine = inputMedicine;
        if (!Room.HasPatient())
        {
            //No patient -- medicine can't be applied.
            NoPatientPopup.Show();
            return;
        }
        if (Room == null)
        {
            //If we're not in a room we can't apply medicine.
            NoPatientPopup.Show();
            return;
        }
        //Do we have medicine? If yes...
        if (medicine.amount > 0)
        {
            //Then apply medicine.
            medicine.amount--;

            Room.Patient.TriggerInteractionTags();

            if(Room.Patient.malady.isImmune)
            {
                Room.Patient.malady.isImmune = false;
                PatientImmunePopup.Show();
                Room.SetAlreadyTreated(true);
            }
            //Checking to see if the medicine works
            else if (Room.Patient.TryCurePatient(medicine))
            {
                //If medicine type is correct
                //Is the patient cured?
                if (Room.Patient.IsPatientCured())
                {
                    //severity lower than 0? Then fully cure the patient.
                    PatientCured();
                    //MapUi.UpdateComputerPatientText(Room);
                }
                else
                {
                    //Otherwise the patients needs to stay there for longer.
                    Room.SetAlreadyTreated(true);
                    CorrectMedicinePopup.Show();
                }
            }
            else
            {
                //Wrong medicine used, come back tomorrow.
                WrongMedicinePopup.Show();
            }
            //Updating the relevant visual information
            Room.UpdateSprites();
            //if(!Room.GetIsEmpty())
            {
                MapUi.UpdateComputerPatientText(Room);
            }
            //Updating inventory
            Inventory.InventoryActions();
        }
    }

    private void PatientCured()
    {
        //Reset the room in which the patient existed.
        //Add daily earnings.
        //Show relevant information
        GlobalData.patientCount--;
        PatientCuredPopup.Show();
        Economy.GiveDailyEarnings(40);
        Room.SetAlreadyTreated(false);
        Room.DeletePatient();
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
            Room.UpdateSprites();
            //GlobalData.CurrentPatientMalady = "none";
            //GlobalData.CurrentPatientSeverity = 0;
        }
    }
}
