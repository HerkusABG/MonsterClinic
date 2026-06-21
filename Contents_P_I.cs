using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.JavaScript;

public partial class Contents_P_I : Node2D
{
    //Storing a reference to all the buttons as well as the speech manager, which is responsible for the dialogue inside the P.A.
    //Since the inventory currently is a container, I also store a reference to that so i don't have to show and hide both of the buttons individually.
    // Patient stats stores patient symptoms and other relevant info. Currently I just added it into the scene but later we'll have it instantiated.

    PatientStats PatientStats;
    [Export] SpeechManager SpeechManagerAccess;

    Button ReturnButton;
    Button DialogueButton;
    Button ZoomButton;
    Button PulseButton;
    Button RejectButton;
    Button AdmitButton;
    Button DiagnosisButton;
    VBoxContainer InventoryContainer;

    private Timer DiagnosisTimer;


    //References to the "DECEASED" sprites which show up when you kill the patient
    [Export] Sprite2D DeceasedSprite1, DeceasedSprite2;

    public override void _Ready()
	{
        Hide();
        //Grabbing the references to all the buttons
        GetAllButtons();
        //Grabbing the patient stats.
        PatientStats = GetNode<PatientStats>("PatientStats");
        if (PatientStats == null)
        {
            GD.Print("Error patient stats not found!!");
        }

        // Initializing patient stats, this will use a constructor later. Kinda ugly to do it like this.
        PatientStats.PatientInitalize();

        //Assigning functionality to each of the buttons.
        ReturnButton.Pressed += ReturnToOffice;
        DialogueButton.Pressed += ShowSpeechDialogue;
        ZoomButton.Pressed += ShowSpeechZoom;
        PulseButton.Pressed += ShowSpeechHeartrate;
        RejectButton.Pressed += ShowSpeechReject;
        //InventoryButton.Pressed += ToggleInventory;
        DiagnosisButton.Pressed += ShowSpeechDiagnosis;

        //Hiding the "DECEASED" sprites which show up when patient is killed.
        DeceasedSprite1.Hide();
        DeceasedSprite2.Hide();
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

        //Going one step deeper for the inventory buttons.
        InventoryContainer = control.GetNode<VBoxContainer>("InventoryContainer");
        DiagnosisButton = InventoryContainer.GetNode<Button>("Diagnosis");
    }

    //All the show speech methods are just calling the speech manager and
    //displaying different information pulled from the PatientStats class.
    //In the future this could probably be done in a more sleek way, but for now it's functional.

    private void ShowSpeechDialogue()
    {
        SpeechManagerAccess.SpeechText(PatientStats.dialogue);
    }
    private void ShowSpeechReject()
    {
        SpeechManagerAccess.SpeechText("Patient has left");
    }

    private void ShowSpeechZoom()
    {
        SpeechManagerAccess.SpeechText("Skin status is: " + PatientStats.skinStatus);
    }

    private void ShowSpeechHeartrate()
    {
        SpeechManagerAccess.SpeechText("Heart rate is: " + PatientStats.heartRate);
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
            PatientStats.isAlive = false;
            //also disable the admit button while we're at it, you're not admitting a dead man
            AdmitButton.Disabled = true;
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
}
