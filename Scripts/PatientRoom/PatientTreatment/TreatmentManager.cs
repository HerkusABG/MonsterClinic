using Godot;
using System;
using System.Collections.Generic;

//class governing using medicine to treat the patient in the patient room
public partial class TreatmentManager : Node
{
    //Storing a reference to all the buttons, labels, etc., for easy reference in the methods
    Sprite2D PatientDisplay;
    Label PatientInfo;
    
    Label WrongMedicinePopup;
    Label NoPatientPopup;
    Label PatientCuredPopup;
    Label CorrectMedicinePopup;
    Button CloseWrongMedicinePopup;
    Button CloseNoPatientPopup;
    Button ClosePatientCuredPopup;
    Button CloseCorrectMedicinePopup;

    private Room Room = null;

    Inventory Inventory;

    private readonly Dictionary<MedicineButton, Action> Subscriptions = new();

    MapUI MapUi;

    public void Initialize()
    {
        GetNodes();

        CloseWrongMedicinePopup.Pressed += () => CloseParent(CloseWrongMedicinePopup);
        CloseNoPatientPopup.Pressed += () => CloseParent(CloseNoPatientPopup);
        ClosePatientCuredPopup.Pressed += () => CloseParent(ClosePatientCuredPopup);
        CloseCorrectMedicinePopup.Pressed += () => CloseParent(CloseCorrectMedicinePopup);
    }

    public void AddSubscription(MedicineButton button)
    {
        Action handler = () => ApplyMedicine(button);

        Subscriptions[button] = handler;
        button.Pressed += handler;
    }

    public void RemoveSubscription(MedicineButton button)
    {
        if (Subscriptions.TryGetValue(button, out var handler))
        {
            button.Pressed -= handler;
            Subscriptions.Remove(button);
        }
    }

    private void GetNodes()
    {
        //Basically just grabbing all the nodes
        Inventory = GetParent() as Inventory;
        MapUi = Inventory.MapUi;
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
            Room.UpdateSprites();
            //GlobalData.CurrentPatientMalady = "none";
            //GlobalData.CurrentPatientSeverity = 0;
        }
    }

    public void ShowUI()
    {
        if(Room != null)
        {
            if (Room.Patient.IsPatientAlive() == true)
            {
                PatientInfo.Show();
                PatientDisplay.Show();
            }
        }
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

    private void ApplyMedicine(MedicineButton button)
    {
        Medicine medicine = button.GetMedicineType();
        GD.Print($"APPLYING {medicine.name} MEDICINE");
        if (!Room.HasPatient())
        {
            NoPatientPopup.Show();
            return;
        }
        if (Room == null)
        {
            NoPatientPopup.Show();
            return;
        }
        if (medicine.amount > 0)
        {
            medicine.amount--;
            if (Room.Patient.TryCurePatient(medicine))
            {
                if (Room.Patient.IsPatientCured())
                {
                    PatientCured();
                }
                else
                {
                    Room.SetAlreadyTreated(true);
                    Room.Patient.TriggerInteractionTags();
                    CorrectMedicinePopup.Show();
                }
            }
            else
            {
                WrongMedicinePopup.Show();
            }
            Room.UpdateSprites();
            if(!Room.GetIsEmpty())
            {
                MapUi.UpdateComputerPatientText(Room);
            }
            Inventory.InventoryActions();
        }
    }

    public void SetTreatmentRoomReference(Room room)
    {
        Room = room;
        if(Room.curedInAbsence)
        {
            PatientCured();
        }
    }

    public Room GetRoom()
    {
       return Room;
    }

    private void PatientCured()
    {
        GlobalData.patientCount--;
        PatientCuredPopup.Show();
        Economy.GiveDailyEarnings(40);
        Room.SetAlreadyTreated(false);
        Room.DeletePatient();
    }
}
