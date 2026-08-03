using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;

public partial class Contents_P_I : ExpNode2D
{
    //Storing a reference to all the buttons as well as the speech manager, which is responsible for the dialogue inside the P.A.
    //Since the inventory currently is a container, I also store a reference to that so i don't have to show and hide both of the buttons individually.
    // Patient stats stores patient symptoms and other relevant info. Currently I just added it into the scene but later we'll have it instantiated.

    public PatientStats PatientPointer;

    Button ReturnButton;
    BaseButton DialogueButton;
    BaseButton ZoomButton;
    BaseButton PulseButton;
    BaseButton RejectButton;
    BaseButton AdmitButton;
    Button VisitButton;
    Button InventoryButton;
    Button DiagnosisButton;
    Button ShotgunButton;
    Button VisitPatientButton;
    VBoxContainer InventoryContainer;
    private Timer DiagnosisTimer;



    //References to the "DECEASED" sprites which show up when you kill the patient
    [Export] Sprite2D DeceasedSprite1;
    [Export] Label PatientLabel;
    [Export] Label AgeLabel;
    [Export] Label PatientsLeftLabel;
    [Export] public Sprite2D PortraitSprite;

    [Export] AdmissionManager AdmissionManagerAccess;
    [Export] Diagnosis_Box Diagnosis;
    [Export] SpeechManager SpeechManagerAccess;

    public void Initialize()
	{
        Hide();
        //Grabbing the references to all the buttons
        GetNodes();

        Subscribe();
        VisitButton.Disabled = true;

        InitializeChildren();
        NewDay();

        //Hiding the inventory and the "DECEASED" sprites which show up when patient is killed.
        DeceasedSprite1.Hide();
        InventoryContainer.Hide();
    }

    private void Subscribe()
    {
        //Assigning functionality to each of the buttons.
        ReturnButton.Pressed += ReturnToOffice;
        DialogueButton.Pressed += ShowSpeechDialogue;
        ZoomButton.Pressed += ShowSpeechZoom;
        PulseButton.Pressed += ShowSpeechHeartrate;
        AdmitButton.Pressed += OnAdmitPressed;
        RejectButton.Pressed += OnRejectPressed;
        //InventoryButton.Pressed += ToggleInventory;
        DiagnosisButton.Pressed += ShowSpeechDiagnosis;

        VisitButton.Pressed += Visit;
    }

    private void InitializeChildren()
    {
        // Initialize the children.
        SpeechManagerAccess.Initialize();
        AdmissionManagerAccess.Initialize();
        Diagnosis.Initialize();
    }
    public void NewDay()
    {
        //Stuff that needs to happen when a new day is there.
        AdmissionManagerAccess.NewDayLogic();
        NextPatient();
        PortraitSprite.Show();
        Diagnosis.SetAllCheckboxStatus(true);
        RejectButton.Disabled = false;
        AdmitButton.Disabled = false;
    }
    public void UpdatePatientInterfaceUI()
    {
        //Update how many patients are left in the queue.
        int patientsLeft = AdmissionManagerAccess.HowManyPatientsLeft();
        if (patientsLeft > 0)
        {
            AdmissionManagerAccess.IsClinicFull();
            PatientsLeftLabel.Text = $"Patients left: {patientsLeft}";
        }
        else
        {
            PatientsLeftLabel.Text = $"Patients left: {0}";
        }
    }

    private void GetNodes()
    {
        //Basically just grabbing all buttons. I have to reference the control because
        //otherwise they wouldn't be found.
        Control control = GetNode<Control>("ControlPatientInterface");
        ReturnButton = control.GetNode<Button>("Return");
        DialogueButton = control.GetNode<BaseButton>("Dialogue");
        ZoomButton = control.GetNode<BaseButton>("Zoom");
        PulseButton = control.GetNode<BaseButton>("Pulse");
        RejectButton = control.GetNode<BaseButton>("Reject");
        AdmitButton = control.GetNode<BaseButton>("Admit");
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
        //if the patient is their story patient, they do their lil intro
        SpeechManagerAccess.SpeechText(PatientPointer.GetDialogue());
        /*if (PatientPointer is StoryPatientStats)
        {
            SpeechManagerAccess.SpeechText(((StoryPatientStats)PatientPointer).entrySpeech);
        }
        else
        {
            SpeechManagerAccess.SpeechText(PatientPointer.GetDialogue());
        }*/
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
  
    private void _on_deceased_sprite_visibility_changed()
    {
        //since now the shotgun is in a different scene, we can't easily access the local instance of PatientStats when using it anymore, 
        //so now we do this operation when the deceased sprite shows up, which is the exact same moment, but allows us to do this in this scene
        if (DeceasedSprite1.Visible == true)
        {
            //PatientPointer.isAlive = false;
            PatientPointer.KillPatient();
            //also disable the admit button while we're at it, you're not admitting a dead man
            AdmitButton.Disabled = true;
        }
    }

    private void ReturnToOffice()
    {
        //when leaving the room, hide it, show the office, and pop the room off the previous scenes stack, to not interfere with the right click functionality
        SpeechManagerAccess.SetBubbleStatus(false);
        Diagnosis.ClearAllBoxes();
        GlobalData.PreviousScenes.Pop();
        int patients = AdmissionManagerAccess.HowManyPatientsLeft();
        GlobalData.IsPatientInWindow = (patients > 0);

        RoomTracker.GoBack();

        //shows the Dialog for the dealer, when return to the office
        var DialogScene = (Control)GetParent().GetNode("Dialog");
        if (GlobalData.Dialog_Dealer == true)
        {
            DialogScene.Show();
        }
        else
        {
            DialogScene.Hide();
        }
    }

    private void OnRejectPressed()
    {
        //Stuff that happens when the reject button is pressed.
        NextPatient();
    }

    public void ShootPatient()
    {
        //if(GlobalData.inPatientRoom)
        
        //else if(GlobalData.inPatientAdmission)
        if (RoomTracker.IsInRoom(ActiveRoom.Admission))
        {
            int patients = AdmissionManagerAccess.HowManyPatientsLeft();
            if (patients >= 0)
            {
                OnRejectPressed();
                OutsideWorld.ChangeReputation((int)ReputationValue.ShotPatient);
                GD.Print($"Reputation now is {OutsideWorld.GetReputation()}");
                SpeechManagerAccess.SpeechText("Patient killed. Sending in the next patient.");
            }
            else
            {
                SpeechManagerAccess.SpeechText("No more patients!");
            }
        }
    }

    private void OnAdmitPressed()
    {
        if (RoomManager.GetEmptyRoomCount() <= 0)
        {
            // Find our NoVacancy panel child node and trigger its flicker
            var noVacancyPanel = GetNode<NoVacancy>("Sprites_PH/NoVacancy");
            if (noVacancyPanel != null)
            {
                noVacancyPanel.TriggerFullWarning();
            }
            return; // STOP execution here so they aren't assigned a room or advanced
        }
        //Stuff that happens when the admit button is pressed.
        AdmissionManagerAccess.Admit();
        NextPatient();
        SetVisitButtonStatus();
    }

    private void NextPatient()
    {
        //NEXT PATIENT!!
        //This is only here until the milestone, then it should be put somewhere else.
        SpeechManagerAccess.SetBubbleStatus(false);
        //Clearing the diagnosis box
        Diagnosis.ClearAllBoxes();
        //Anything that the admission manager needs to do when a new patient
        //appears in the admission is activated
        AdmissionManagerAccess.PatientQueueLogic();
        //How many patients left? Saved in variable "patients"
        int patients = AdmissionManagerAccess.HowManyPatientsLeft();
        if (patients >= 0)
        {
            //If this wasn't the last patient, we generate a new one.
            PatientsLeftLabel.Text = $"Patients left: {patients}";
            PatientPointer = AdmissionManagerAccess.GenerateNewPatient();
            RenderNewPatient();
        }
        else
        {
            //If this was the last patient, the logic is different.
            //Disabling some stuff, telling the player that there are no more patients.
            PortraitSprite.Hide();
            PatientsLeftLabel.Text = $"Patients left: {0}";
            RejectButton.Disabled = true;
            AdmitButton.Disabled = true;
            Diagnosis.SetAllCheckboxStatus(false);
            //Null patient is used for making it so that no new dialogue options appear
            //Also if there's no patient, the age and ID of patient will not show up.
            PatientPointer = AdmissionManagerAccess.GetNullPatient();
        }

        // random tint to the portrait
        PortraitSprite.Modulate = PatientPointer.PortraitColor;
        DeceasedSprite1.Hide();

        PatientLabel.Text = "Patient: " + PatientPointer.patientID; //convert data to strings to display it on Labels  and '+' operator connects static text "ID: " with the variable value
        AgeLabel.Text = "Age: " + PatientPointer.age.ToString(); //used stringt o convert the integer age to a string for display purposes

        GlobalData.IsPatientInWindow = (patients > 0); // update Globaldata  value
    }

    private void RenderNewPatient()
    {
       TextureUnit unit = PatientPointer.GetPatientTextures();
       PortraitSprite.Texture = unit.BodySet.standing;
    }

    private void SetVisitButtonStatus()
    {
        //Based on whether a patient has been assigned to a room today
        //Turn on/off the visit button.
        if (AdmissionManagerAccess.GetLatestRoom() != null)
        {
            VisitButton.Disabled = false;
        }
    }

    private void Visit()
    {
        //GlobalData.inPatientAdmission = false;
        RoomTracker.RoomTrack(ActiveRoom.PatientRoom);
        Inventory inv = GetParent().GetNode<Inventory>("Inventory");
        inv.InventoryActions();
        //Visit button logic.
        SpeechManagerAccess.SetBubbleStatus(false);
        //For making the RMB "go back to last room" stuff work
        //GlobalData.PreviousScenes.Pop();
        //GlobalData.PreviousScenes.Pop();
        var hallway = GetParent().GetNode<Node2D>("Hallway");
        Hallway hallwayAccess = hallway as Hallway;

        //Go to the room as based on the AdmissionManager's reference for the latest assigned room
        ExpNode2D room = AdmissionManagerAccess.GetLatestRoom();
        hallwayAccess.GoToRoom(room);
        Room roomRef = room as Room;
        
        //Hiding some more UI stuff.
        Hide();
        //Resetting the diagnosis box
        Diagnosis.ClearAllBoxes();

        //Saving the scene path, for RMB functionality
        GlobalData.PreviousScenes.Push(hallway.GetPath());
        GlobalData.PreviousScenes.Push(room.GetPath());
    }

    public override void OnRoomEnter(Node mainNode)
    {
        //GD.Print("Entering admission");

        UpdatePatientInterfaceUI();

        Hallway hallway = mainNode.GetNode<Hallway>("Hallway");
        hallway.UpdateHallwayUI();

        Inventory inv = mainNode.GetNode<Inventory>("Inventory");
        inv.InventoryActions();
    }

    public override void OnRoomExit()
    {
        //GD.Print("Exiting admission");
        HideSpeechBubble();
    }
}

