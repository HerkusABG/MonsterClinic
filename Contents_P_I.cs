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

    Button ReturnButton;
    Button DialogueButton;
    Button ZoomButton;
    Button PulseButton;
    Button RejectButton;
    Button AdmitButton;
    Button VisitButton;
    Button InventoryButton;
    Button DiagnosisButton;
    Button ShotgunButton;
    Button VisitPatientButton;
    VBoxContainer InventoryContainer;
     private Timer DiagnosisTimer;

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
        //RejectButton.Pressed += ShowSpeechReject;
        //InventoryButton.Pressed += ToggleInventory;
        DiagnosisButton.Pressed += ShowSpeechDiagnosis;

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
        ReturnButton = control.GetNode<Button>("Return");
        DialogueButton = control.GetNode<Button>("Dialogue");
        ZoomButton = control.GetNode<Button>("Zoom");
        PulseButton = control.GetNode<Button>("Pulse");
        RejectButton = control.GetNode<Button>("Reject");
        AdmitButton = control.GetNode<Button>("Admit");
        VisitButton = control.GetNode<Button>("VisitPatient");
        InventoryButton = control.GetNode<Button>("Inventory");

        //Going one step deeper for the inventory buttons.
        InventoryContainer = control.GetNode<VBoxContainer>("InventoryContainer");
        DiagnosisButton = InventoryContainer.GetNode<Button>("Diagnosis");
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
    public void HideSpeechBubble()
    {
        SpeechManagerAccess.SetBubbleStatus(false);
    }
    private void ShowSpeechDiagnosis()
    {
        SpeechManagerAccess.SpeechText("soooo, you are telling me \n THAT is gonna help you diagnose me??");
        
        // Timer from the scene
        var sceneTimer = GetNode<Timer>("Diagnosis_Timer");
        sceneTimer.OneShot = true;

        // connect the signals
        sceneTimer.Timeout += OnSceneTimerTimeout;

        // timer is getting set to 3 seconds and starts
        sceneTimer.Start(3.0);
    }
    private void OnSceneTimerTimeout()
    {
        var speech = SpeechManagerAccess.GetNode<Label>("SpeechBubble");
        speech.Hide();
    }
    //Toggling the inventory, pretty simple.
    private void ToggleInventory()
    {
        InventoryContainer.Visible = !InventoryContainer.Visible;
        if(!PatientPointer.isAlive)
        {
            //ShotgunButton.Disabled = true;
        }
        else
        {
            //ShotgunButton.Disabled = false;
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

    private void _on_deceased_sprite_visibility_changed()
    {
        //since now the shotgun is in a different scene, we can't easily access the local instance of PatientStats when using it anymore, 
        //so now we do this operation when the deceased sprite shows up, which is the exact same moment, but allows us to do this in this scene
        if (DeceasedSprite1.Visible == true)
        {
            PatientPointer.isAlive = false;
            //also disable the admit button while we're at it, you're not admitting a dead man
            AdmitButton.Disabled = true;
        }
    }

    private void ReturnToOffice()
    {
        //when leaving the room, hide it, show the office, and pop the room off the previous scenes stack, to not interfere with the right click functionality
        Hide();
        SpeechManagerAccess.SetBubbleStatus(false);
        Diagnosis.ClearAllBoxes();
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
        //ShotgunButton.Disabled = false;
        GD.Print("Line 206 in Contents_P_I.cs");
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

        //ShotgunButton.Disabled = false;
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
        GlobalData.inPatientRoom = true;
        SpeechManagerAccess.SetBubbleStatus(false);
        Inventory inv = GetParent().GetNode<Inventory>("Inventory");
        TreatmentManager treatment = inv.GetNode<TreatmentManager>("Treatment_Manager");
        var hallway = LatestRoom.GetParent().GetParent().GetNode<Node2D>("Hallway");
        var patient = treatment.GetNode<Node2D>("Patient_Display");
        var patientInfo = treatment.GetNode<CanvasItem>("Patient_Info");
        //hide the patient admission screen, show the patient room, with the patient sprite and info now visible
        Hide();
        Hallway hallwayAccess = hallway as Hallway;
        hallwayAccess.GoToRoom(LatestRoom);
        Room roomRef = LatestRoom as Room;
        if(roomRef.HasPatient())
        {
            LatestRoom.Show();
            patient.Show();
            patientInfo.Show();
        }
        //we don't need to go back to this scene from the patient room after they're admitted, better have the right click go back to the office, so we're removing the patient admission from the stack here
        GlobalData.PreviousScenes.Pop();
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

    public bool AnyPatientsLeft()
    {
        if(patientsLeft >0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

