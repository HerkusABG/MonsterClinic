using Godot;
using System;
using System.Collections.Generic;

//class governing using medicine to treat the patient in the patient room
public partial class TreatmentManager : Node
{
    //Storing a reference to all the buttons, labels, etc., for easy reference in the methods
    Sprite2D PatientDisplay;
    Label PatientInfo;

    [Export] Popup Popup;

    private Room Room = null;

    Inventory Inventory;

    private readonly Dictionary<MedicineButton, Action> Subscriptions = new();

    //MapUi reference so that the treatment there is linked with 
    //the treatment in the inventory
    MapUI MapUi;

    public void Initialize()
    {
        GetNodes();

        Popup.Initialize();
    }

    private void GetNodes()
    {
        //Basically just grabbing all the nodes
        Inventory = GetParent() as Inventory;
        MapUi = Inventory.MapUi;

        Popup = GetNode<Popup>("Popup");
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
        if (Room == null)
        {
            //If we're not in a room we can't apply medicine.
            Popup.DisplayPopup(PopupMessages.TreatmentMessages["NoRoom"]);
            //NoPatientPopup.Show();
            return;
        }
        if (!Room.HasPatient())
        {
            //No patient -- medicine can't be applied.
            Popup.DisplayPopup(PopupMessages.TreatmentMessages["NoPatient"]);
            //NoPatientPopup.Show();
            return;
        }
        //Do we have medicine? If yes...
        if (medicine.amount > 0)
        {
            //Then apply medicine.
            medicine.amount--;
            Room.IncrementTreated();

            Room.Patient.TriggerInteractionTags();

            string result = "";
            
            //Checking to see if the medicine works
            if (Room.Patient.TryCurePatient(medicine))
            {
                if (Room.Patient.malady.isImmune)
                {
                    Room.Patient.malady.isImmune = false;
                    //Popup.DisplayPopup(PopupMessages.TreatmentMessages["Immune"]);
                    //PatientImmunePopup.Show();
                    if (Room.GetTimesTreated() >= 3)
                    {
                        Room.SetAlreadyTreated(true);
                        Room.Patient.AddSeverity();
                        Popup.DisplayPopup(PopupMessages.TreatmentMessages["ImmuneFinal"]);
                    }
                    else
                    {
                        Room.Patient.AddSeverity();
                        Popup.DisplayPopup(PopupMessages.TreatmentMessages["ImmuneCorrect"]);
                    }
                    result = ClinicActionList.Results["MedImmune"].output;
                }
                else
                {
                    Room.Patient.ShowCorrectMedicineDialogue(Room.SpeechManagerAccess);
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
                        Popup.DisplayPopup(PopupMessages.TreatmentMessages["CorrectMedicine"]);
                        //CorrectMedicinePopup.Show();
                    }
                    result = ClinicActionList.Results["MedSuccess"].output;
                }
            }
            else
            {
                Room.Patient.ShowIncorrectMedicineDialogue(Room.SpeechManagerAccess);
                if (Room.GetTimesTreated() >= 3)
                {
                    Room.SetAlreadyTreated(true);
                    Popup.DisplayPopup(PopupMessages.TreatmentMessages["WrongMedicineFinal"]);
                }
                else
                {
                    //Wrong medicine used, come back tomorrow.
                    Popup.DisplayPopup(PopupMessages.TreatmentMessages["WrongMedicine"]);
                }
                //WrongMedicinePopup.Show();
                result = ClinicActionList.Results["MedFail"].output;
            }
            if(Room.Patient != null)
            {
                Room.Patient.NewClinicAction(ClinicActionList.Actions["GiveMedicine"].output, medicine.name, result);
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
        Popup.DisplayPopup(PopupMessages.TreatmentMessages["Cured"]);
        //PatientCuredPopup.Show();
        Room.Patient.GivePayout();
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
        //Room.UpdateSprites();
        //GlobalData.CurrentPatientMalady = "none";
        //GlobalData.CurrentPatientSeverity = 0;
    }
}
