using Godot;
using System;
using System.Linq;

public partial class AdmissionManager : Node
{
    [Export] Contents_P_I PatientAdmission;
    [Export] public Sprite2D PatientSpriteDisplay;
    [Export] Button AdmitButton;

    private int patientsLeft;

    PatientStats nullPatient = new PatientStats();

    private PatientStats InternalPatient;

    Node2D LatestRoom = null;

    public void Initialize()
    {
        NullPatientInitialize();
    }
    public void Admit()
    {
        Node mainNode = GetParent().GetParent();
        //var roomNode = mainNode.GetNode<Node2D>("Room");

       if(IsClinicFull())
       {
            var roomNode = RoomManager.FindEmptyRoom();
            if (roomNode != null)
            {
                //Inventory inv = GetParent().GetParent().GetNode<Inventory>("Inventory");
                //TreatmentManager treatment = inv.GetNode<TreatmentManager>("Treatment_Manager");

                Room room = roomNode as Room;
                //treatment.SetTreatmentRoomReference(room);
                //PatientAdmission.GenerateNewPatientVoid();

                room.AssignPatient(PatientAdmission.PatientPointer);
                GlobalData.patientCount++;

                //treatment.UpdateTreatmentText();
                //treatment.ReenableMedicine();

                SetLatestPatientRoom(room);
                IsClinicFull();
            }
       }
    }
    public PatientStats GetNullPatient()
    {
        return nullPatient;
    }
    public void PatientQueueLogic()
    {
        patientsLeft--;
    }

    public int HowManyPatientsLeft()
    {
        return patientsLeft;
    }

    public void NewDayLogic()
    {
        LatestRoom = null;
        patientsLeft = Upgrades.newPatientSlots;
    }

    public Node2D GetLatestRoom()
    {
        return LatestRoom;
    }
    public void Reject()
    {

    }

    public bool IsClinicFull()
    {
        int emptyRoomCount = RoomManager.GetEmptyRoomCount();
        if (emptyRoomCount > 0)
        {
            SetButtonStatus(AdmitButton, true);
        }
        else
        {
            SetButtonStatus(AdmitButton, false);
        }
        if (emptyRoomCount > 0)
        {
            return true;
        }
        return false;
    }

    public void SetButtonStatus(Button button, bool status)
    {
        button.Disabled = !status;
    }
    public PatientStats GenerateNewPatient()
    {
        GD.Print("Generating new patient");
        //  generate new data
        PatientStats patientStats;
        patientStats = new PatientStats();

        InternalPatient = patientStats;
        return patientStats;
    }

    public void SetLatestPatientRoom(Node2D room)
    {
        LatestRoom = room;
    }
    private void VisitInternalLogic()
    {
        GlobalData.inPatientRoom = true;
    }
    private void NullPatientInitialize()
    {
        nullPatient.malady = MaladyList.Database.ElementAt(0).Value;
        nullPatient.age = 0;
        nullPatient.patientID = "";
    }
}