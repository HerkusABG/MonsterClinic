using Godot;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

public partial class Room : ExpNode2D 
{
    //Storing a reference to all the buttons, labels, etc., for easy reference in the methods
    Button LeaveRoomButton;
    [Export] Sprite2D PatientDisplay;
    [Export] Control Corpse;
    [Export] Label PatientInfo;

    //Is the room empty?
    private bool isEmpty = true;

    public bool curedInAbsence;

    //Pointer to the patient information
    public PatientStats Patient;



    //boolean that checks whether you can treat the patient.
    private bool notYetTreated = true;

    Inventory invy;
    [Export] public SpeechManager SpeechManagerAccess;
    public void Initialize(Action HideUIAction)
    {
        //grabs references to all the necessary nodes
        GetNodes();

        //assigning methods to all the buttons
        LeaveRoomButton.MouseEntered += HoverOn;
        LeaveRoomButton.MouseExited += HoverOff;
        LeaveRoomButton.Pressed += LeaveRoom;
        LeaveRoomButton.Pressed += HideUIAction;

        curedInAbsence = false;
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

    public override void OnRoomEnter()
    {
        //Piece of logic that gets executed whenever you enter the room.
        
        UpdateSprites();
        ShowSpeechDialogue();
    }

    public override void OnRoomExit()
    {
        //when leaving the room, hide it, show the office, and pop the room off the previous scenes stack, to not interfere with the right click functionality
        
        SpeechManagerAccess.SetBubbleStatus(false);
    }

    private void HoverOff()
    {
        //makes the text disappear when you stop hovering
        LeaveRoomButton.Text = "";
    }

    private void LeaveRoom()
    {
        RoomTracker.GoBack();
    }

    
    private void CloseParent(Button button)
    {
      
    }

    public void UpdateSprites()
    {
        if(!isEmpty)
        {
            if (Patient.IsPatientAlive())
            {
                SetPatientUIStatus(true, true);
                SetPatientRoomText();
            }
            else
            {
                SetPatientUIStatus(true, false);
                SetPatientRoomText();
            }


        }
        else
        {
            SetPatientUIStatus(false, false);
            Corpse.Hide();
            PatientDisplay.Hide();
        }
    }

    private void SetPatientUIStatus(bool status, bool alive)
    {
        if(status)
        {
            if(alive)
            {
                PatientDisplay.Show();
                Corpse.Hide();
                PatientInfo.Show();
            }
            else
            {
                PatientDisplay.Hide();
                Corpse.Show();
                PatientInfo.Show();
            }
        }
        else
        {
            PatientDisplay.Hide();
            PatientInfo.Hide();
        }
    }

    private void SetPatientRoomText()
    {
        string input;
        if(Patient.IsPatientAlive())
        {
            input = "Alive";
        }
        else
        {
            input = "Dead";
        }
        PatientInfo.Text = $"Malady: {Patient.malady.name}" +
            $" \n Age: {Patient.age}" +
            $" \n Severity: {Patient.malady.severity} " +
            $"\n Status: {input}";
    }

    public bool HasPatient()
    {
        return Patient != null;
    }

    public void KillPatient()
    {
        DeletePatient();
        UpdateSprites();
        OutsideWorld.ChangeReputation((int)ReputationValue.ShotPatient);
    }

    public void DeletePatient()
    {
        GD.Print("patient deleted");
        Patient = null;
        isEmpty = true;
    }

    public void AssignPatient(PatientStats patient)
    {
        Patient = patient;
        Patient.AssignRoom(this);
        PatientDisplay.Modulate = patient.PortraitColor;
        isEmpty = false;
    }

    public void PatientCuredInAbsence()
    {
        curedInAbsence = true;
        DeletePatient();
    }

    public void SetAlreadyTreated(bool input)
    {
        notYetTreated = !input;
    }

    public bool GetAlreadyTreated()
    {
        return !notYetTreated;
    }

    public bool GetIsEmpty()
    {
        return isEmpty;
    }
    private void ShowSpeechDialogue()
    {
        if (SpeechManagerAccess == null) return;
        if (Patient == null) return;
        //if the patient is their story patient, they do their lil intro
        //SpeechManagerAccess.SpeechText(Patient.GetAdmittedDialogue());
        if (Patient is not StoryPatientStats)
        {
            SpeechManagerAccess.SpeechText(Patient.GetAdmittedDialogue());
        }
        else
        {
            SpeechManagerAccess.SpeechText(((StoryPatientStats)Patient).GetAdmittedDialogue());
        }
    }
}