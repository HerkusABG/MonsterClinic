using Godot;
using System;

public partial class AdmissionManager : Node
{
    [Export] public Sprite2D PatientSpriteDisplay;

    public void _on_admit_pressed()
    {
        //part of Princess's system for moving the patient sprite to the patient room, currently deprecated, but just commented out because it might be useful again
        // saves patient sprite
        /* if (PatientSpriteDisplay != null)
        {
            GlobalData.AdmittedPatientTexture = PatientSpriteDisplay.Texture;
        }*/


        Node mainNode = GetParent().GetParent();


        var roomNode = mainNode.GetNode<Node2D>("Room");
        var patientInterface = mainNode.GetNode<Node2D>("Patient_Interface");
        var patient = roomNode.GetNode<Node2D>("Patient_Display");
        var patientInfo = roomNode.GetNode<CanvasItem>("Patient_Info");

        //give the newly admitted patient a random malady at a random severity
        Random rnd = new Random();
        GlobalData.CurrentPatientMalady = GlobalData.Maladies[rnd.Next(0, 3)];
        GlobalData.CurrentPatientSeverity = rnd.Next(2, 5);

        //hide the patient admission screen, show the patient room, with the patient sprite and info now visible
        roomNode.Show();
        patientInterface.Hide();
        patient.Show();
        patientInfo.Show();
        //push the scene we're entering to the previous scenes stack
        GlobalData.PreviousScenes.Push(roomNode.GetPath());
    }
}