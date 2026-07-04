using Godot;
using System;
using static System.Net.Mime.MediaTypeNames;

public partial class Room : Node2D 
{
    //Storing a reference to all the buttons, labels, etc., for easy reference in the methods
    Button LeaveRoomButton;
    [Export] Sprite2D PatientDisplay;
    [Export] Label PatientInfo;

    //Is the room empty?
    private bool isEmpty = true;

    //Pointer to the patient information
    public PatientStats Patient;

    //boolean that checks whether you can treat the patient.
    public bool alreadyTreated = false;

    public void Initialize(Action HideUIAction)
    {
        //grabs references to all the necessary nodes
        GetNodes();

        //assigning methods to all the buttons
        LeaveRoomButton.MouseEntered += HoverOn;
        LeaveRoomButton.MouseExited += HoverOff;
        LeaveRoomButton.Pressed += LeaveRoom;
        LeaveRoomButton.Pressed += HideUIAction;

        //yes this looks kinda wacky, but apparently that's how I gotta write it if I want to have methods that take arguments

    }

    private void GetNodes()
    {
        //Grab references
        LeaveRoomButton = GetNode<Button>("Leave_Room");
    }

    private void HoverOn()
    {
        //makes the text show up when hovering over the button
        LeaveRoomButton.Text = "Leave";
    }

    public void OnRoomEnter()
    {
        //Piece of logic that gets executed whenever you enter the room.
        UpdateSprites();
    }
    private void HoverOff()
    {
        //makes the text disappear when you stop hovering
        LeaveRoomButton.Text = "";
    }

    private void LeaveRoom()
    {
        //when leaving the room, hide it, show the office, and pop the room off the previous scenes stack, to not interfere with the right click functionality
        Hide();
        var HallwayScene = (Node2D)GetParent().GetParent().GetNode("Hallway");
        HallwayScene.Show();
        GlobalData.inPatientRoom = false;
        if(GlobalData.PreviousScenes.Count == 0)
        {
            GlobalData.PreviousScenes.Pop();
        }
    }

    
    private void CloseParent(Button button)
    {
      
    }

    
    private void _on_patient_room_background_visibility_changed()
    {
        
    }

    public void UpdateSprites()
    {
        if(!isEmpty)
        {
            SetPatientUIStatus(true);
            SetPatientRoomText();
        }
        else
        {
            SetPatientUIStatus(false);
        }
    }

    private void SetPatientUIStatus(bool status)
    {
        if(status)
        {
            PatientDisplay.Show();
            PatientInfo.Show();
        }
        else
        {
            PatientDisplay.Hide();
            PatientInfo.Hide();
        }
    }

    private void SetPatientRoomText()
    {
         PatientInfo.Text = $"Malady: {Patient.malady.name} \n Age: {Patient.age} \n Severity: {Patient.malady.severity}";
    }

    public bool HasPatient()
    {
        return Patient != null;
    }

    public void DeletePatient()
    {
        Patient = null;
        isEmpty = true;
    }

    public void AssignPatient(PatientStats patient)
    {
        Patient = patient;
        isEmpty = false;
    }

    /*public void UpdateTreatmentText()
    {
        if (Room == null) return;
        if (Room.Patient == null) return;
        PatientInfo.Text = $"Patient info: " +
                   $"\n Malady: {Room.Patient.malady.name}" +
                   $"\n Severity: {Room.Patient.malady.severity}" +
                   $"\n Age: {Room.Patient.age}";
    }*/
}