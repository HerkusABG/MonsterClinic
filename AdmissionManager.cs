using Godot;
using System;
using System.Linq;

public partial class AdmissionManager : Node
{
    [Export] Contents_P_I PatientAdmission;
    [Export] public Sprite2D PatientSpriteDisplay;
    [Export] Admit AdmitButton;

    public void _on_admit_pressed()
    {
        //part of Princess's system for moving the patient sprite to the patient room, currently deprecated, but just commented out because it might be useful again
        // saves patient sprite
        /* if (PatientSpriteDisplay != null)
        {
            GlobalData.AdmittedPatientTexture = PatientSpriteDisplay.Texture;
        }*/


        Node mainNode = GetParent().GetParent();


        //var roomNode = mainNode.GetNode<Node2D>("Room");

       if(IsClinicFull())
       {
            var roomNode = RoomManager.FindEmptyRoom();
            if (roomNode != null)
            {
                Inventory inv = GetParent().GetParent().GetNode<Inventory>("Inventory");
                TreatmentManager treatment = inv.GetNode<TreatmentManager>("Treatment_Manager");
                Random random = new Random();
                var patientInterface = mainNode.GetNode<Node2D>("Patient_Interface");
                Node2D patient = treatment.GetNode<Node2D>("Patient_Display");
                var patientInfo = treatment.GetNode<CanvasItem>("Patient_Info");

                Room room = roomNode as Room;
                treatment.Room = room;
                room.Patient = PatientAdmission.PatientPointer;
                PatientAdmission.GenerateNewPatientVoid();
                GlobalData.patientCount++;

                //give the newly admitted patient a random malady at a random severity
                //room.Patient.malady = GlobalData.Maladies[rnd.Next(0, 3)];

                treatment.UpdateTreatmentText();
                treatment.ReenableMedicine();
                


                PatientAdmission.SetLatestPatientRoom(room);
                IsClinicFull();
            }
       }
    }

    public bool IsClinicFull()
    {
        int emptyRoomCount = RoomManager.GetEmptyRoomCount();
        if (emptyRoomCount > 0)
        {
            AdmitButton.SetButtonStatus(true);
        }
        else
        {
            AdmitButton.SetButtonStatus(false);
        }
        if (emptyRoomCount > 0)
        {
            return true;
        }
        return false;
    }
}