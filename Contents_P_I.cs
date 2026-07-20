using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;

public partial class Contents_P_I : Node2D
{
    //Storing a reference to all the buttons as well as the speech manager, which is responsible for the dialogue inside the P.A.
    //Since the inventory currently is a container, I also store a reference to that so i don't have to show and hide both of the buttons individually.
    // Patient stats stores patient symptoms and other relevant info. Currently I just added it into the scene but later we'll have it instantiated.

    public PatientStats PatientPointer;
    [Export] SpeechManager SpeechManagerAccess;

    TextureButton ReturnButton;
    TextureButton DialogueButton;
    TextureButton ZoomButton;
    TextureButton PulseButton;
    TextureButton RejectButton;
    TextureButton AdmitButton;
    TextureButton VisitButton;
    TextureButton InventoryButton;
    TextureButton DiagnosisButton;
    TextureButton ShotgunButton;
    TextureButton VisitPatientButton;
    VBoxContainer InventoryContainer;

    private int patientsLeft;


    //References to the "DECEASED" sprites which show up when you kill the patient
    [Export] Sprite2D DeceasedSprite1;
    [Export] Label PatientLabel;
    [Export] Label AgeLabel;
    [Export] Label PatientsLeftLabel;
    [Export] public Sprite2D PortraitSprite;

    [Export] AdmissionManager AdmissionManagerAccess;
    [Export] Diagnosis_Box Diagnosis;

    Node2D LatestRoom = null;

    PatientStats nullPatient = new PatientStats();

    public override void _Ready()
	{
        

        Hide();
        //Grabbing the references to all the buttons
        GetAllButtons();
        //Grabbing the patient stats.
        NewDay(false);
        PatientPointer = GenerateNewPatient();

        //Assigning functionality to each of the buttons.
        ReturnButton.Pressed += ReturnToOffice;
        DialogueButton.Pressed += ShowSpeechDialogue;
        ZoomButton.Pressed += ShowSpeechZoom;
        PulseButton.Pressed += ShowSpeechHeartrate;
        InventoryButton.Pressed += ToggleInventory;
        DiagnosisButton.Pressed += ShowSpeechDiagnosis;
        ShotgunButton.Pressed += KillPatient;

        VisitButton.Pressed += VisitLatestPatient;
        VisitButton.Disabled = true;

        //Hiding the inventory and the "DECEASED" sprites which show up when patient is killed.
        DeceasedSprite1.Hide();
        InventoryContainer.Hide();
    }

    public void UpdatePatientInterfaceUI()
    {
        if(patientsLeft > 0)
        {
            AdmissionManagerAccess.IsClinicFull();
            PatientsLeftLabel.Text = $"Patients left: {patientsLeft}";
        }
        else
        {
            PatientsLeftLabel.Text = $"Patients left: {0}";
        }
    }

    private void GetAllButtons()
    {
        //Basically just grabbing all buttons. I have to reference the control because
        //otherwise they wouldn't be found.
        Control control = GetNode<Control>("ControlPatientInterface");
        ReturnButton = control.GetNode<TextureButton>("Return");
        DialogueButton = control.GetNode<TextureButton>("Dialogue");
        ZoomButton = control.GetNode<TextureButton>("Zoom");
        PulseButton = control.GetNode<TextureButton>("Pulse");
        RejectButton = control.GetNode<TextureButton>("Reject");
        AdmitButton = control.GetNode<TextureButton>("Admit");
        VisitButton = control.GetNode<TextureButton>("VisitPatient");
        InventoryButton = control.GetNode<TextureButton>("Inventory");

        //Going one step deeper for the inventory buttons.
        InventoryContainer = control.GetNode<VBoxContainer>("InventoryContainer");
        DiagnosisButton = InventoryContainer.GetNode<TextureButton>("Diagnosis");
        ShotgunButton = InventoryContainer.GetNode<TextureButton>("Shotgun");
    }

    //All the show speech methods are just calling the speech manager and
    //displaying different information pulled from the PatientStats class.
    //In the future this could probably be done in a more sleek way, but for now it's functional.

    private void ShowSpeechDialogue()
    {
        SpeechManagerAccess.SpeechText(PatientPointer.GetDialogue());
    }
  
    private void ShowSpeechZoom()
    {
        SpeechManagerAccess.SpeechText(PatientPointer.GetTemperature());
    }

    private void ShowSpeechHeartrate()
    {
        SpeechManagerAccess.SpeechText(PatientPointer.GetPulse());
    }

    private void ShowSpeechDiagnosis()
    {
        SpeechManagerAccess.SpeechText("soooo, you are telling me \n THAT is gonna help you diagnose me??");
    }
    //Toggling the inventory, pretty simple.
    private void ToggleInventory()
    {
        InventoryContainer.Visible = !InventoryContainer.Visible;
        if(!PatientPointer.isAlive)
        {
            ShotgunButton.Disabled = true;
        }
        else
        {
            ShotgunButton.Disabled = false;
        }
    }
   
    //For now killing the patient doesn't have any advanced functionality. Just showing the sprites.
    private void KillPatient()
    {
        if(PatientPointer.isAlive)
        {
            PatientPointer.isAlive = false;
            DeceasedSprite1.Show();
        }
    }
    
    private void ReturnToOffice()
    {
        //when leaving the room, hide it, show the office, and pop the room off the previous scenes stack, to not interfere with the right click functionality
        Hide();
        var OfficeScene = (Node2D)GetParent().GetNode("Office");
        OfficeScene.Show();
        GlobalData.PreviousScenes.Pop();
    }

    public PatientStats GenerateNewPatient()
    {
        //This is only here until the milestone, then it should be put somewhere else.
        SpeechManagerAccess.SetBubbleStatus(false);
        Diagnosis.ClearAllBoxes();
        //  generate new data
        PatientStats patientStats = new PatientStats();

       // random tint to the portrait
        PortraitSprite.Modulate = patientStats.PortraitColor;
        DeceasedSprite1.Hide();

        PatientLabel.Text = "Patient: " + patientStats.patientID; //convert data to strings to display it on Labels  and '+' operator connects static text "ID: " with the variable value
        AgeLabel.Text = "Age: " + patientStats.age.ToString(); //used stringt o convert the integer age to a string for display purposes

        PatientQueue();
        ShotgunButton.Disabled = false;
        return patientStats;
    }

    public void GenerateNewPatientVoid()
    {
        //This is only here until the milestone, then it should be put somewhere else.
        SpeechManagerAccess.SetBubbleStatus(false);
        Diagnosis.ClearAllBoxes();

        //  generate new data
        PatientStats patientStats = new PatientStats();

        // random tint to the portrait
        PortraitSprite.Modulate = patientStats.PortraitColor;
        DeceasedSprite1.Hide();

        PatientLabel.Text = "Patient: " + patientStats.patientID; //convert data to strings to display it on Labels  and '+' operator connects static text "ID: " with the variable value
        AgeLabel.Text = "Age: " + patientStats.age.ToString(); //used stringt o convert the integer age to a string for display purposes

        PatientQueue();
        PatientPointer = patientStats;

        ShotgunButton.Disabled = false;
    }

    public void SetLatestPatientRoom(Node2D room)
    {
        LatestRoom = room;
        if(VisitButton.Disabled)
        {
            VisitButton.Disabled = false;
        }
    }
    private void VisitLatestPatient()
    {
        SpeechManagerAccess.SetBubbleStatus(false);
        var hallway = LatestRoom.GetParent().GetParent().GetNode<Node2D>("Hallway");
        var patient = LatestRoom.GetNode<Node2D>("Patient_Display");
        var patientInfo = LatestRoom.GetNode<CanvasItem>("Patient_Info");
        //hide the patient admission screen, show the patient room, with the patient sprite and info now visible
        Hide();
        LatestRoom.Show();
        patient.Show();
        patientInfo.Show();
        //we don't need to go back to this scene from the patient room after they're admitted, better have the right click go back to the office, so we're removing the patient admission from the stack here
        GlobalData.PreviousScenes.Pop();
        //push the scene we're entering to the previous scenes stack
        GlobalData.PreviousScenes.Push(hallway.GetPath());
        GlobalData.PreviousScenes.Push(LatestRoom.GetPath());
    }

    private void NullPatientInitialize()
    {
        nullPatient.malady = MaladyList.Database.ElementAt(0).Value;
        nullPatient.age = 0;
        nullPatient.patientID = "";
        PatientPointer = nullPatient;
        PatientLabel.Text = "Patient: " + nullPatient.patientID; //convert data to strings to display it on Labels  and '+' operator connects static text "ID: " with the variable value
        AgeLabel.Text = "Age: " + nullPatient.age.ToString();
    }

    public void NewDay(bool generateNewPatient)
    {
        patientsLeft = Upgrades.newPatientSlots;
        PortraitSprite.Show();
        Diagnosis.SetAllCheckboxStatus(true);
        RejectButton.Disabled = false;
        AdmitButton.Disabled = false;
        if(generateNewPatient)
        {
            GenerateNewPatientVoid();
        }
    }
    private void PatientQueue()
    {
        patientsLeft--;
        PatientsLeftLabel.Text = $"Patients left: {patientsLeft}";
        if(patientsLeft < 0)
        {
            PortraitSprite.Hide();
            PatientsLeftLabel.Text = $"Patients left: {0}";
            RejectButton.Disabled = true;
            AdmitButton.Disabled = true;
            Diagnosis.SetAllCheckboxStatus(false);
            NullPatientInitialize();
        }
    }
}

