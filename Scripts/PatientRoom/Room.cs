using Godot;
using System;
public partial class Room : ExpNode2D 
{
    //Storing a reference to all the buttons, labels, etc., for easy reference in the methods
    Button LeaveRoomButton;
    [Export] Control WholePatient;
    [Export] Sprite2D PatientDisplay;
    [Export] Sprite2D PatientHead;
    [Export] Control Corpse;
    [Export] Control PatientInfo;
    [Export] Control UIControl;
    [Export] Sprite2D MaladySprite;
    [Export] Sprite2D TopMaladySprite;
    [Export] Sprite2D ClothingSprite;
    [Export] PackedScene Transition = ResourceLoader.Load<PackedScene>("res://fade_animation.tscn");
    [Export] Button ClothingButton;

    //Is the room empty?
    private bool isEmpty = true;

    public bool curedInAbsence;

    //Pointer to the patient information
    public PatientStats Patient;


    //logic for return buton dsiplay 
    private Timer _inactivityTimer;
    private Timer _displayTimer;
    private bool _isButtonVisible = false;
    



    //boolean that checks whether you can treat the patient.
    private bool notYetTreated = true;
    private int timesTreated = 0;

    Inventory invy;
    [Export] public SpeechManager SpeechManagerAccess;
    [Export] PatientInfoManager PatientInfoScreen;
    public void Initialize(Action HideUIAction)
    {
        //grabs references to all the necessary nodes
        GetNodes();

        PatientInfoScreen.Initialize(this);

        //assigning methods to all the buttons
        LeaveRoomButton.MouseEntered += HoverOn;
        LeaveRoomButton.MouseExited += HoverOff;
        SetupTimers();
        //LeaveRoomButton.MouseEntered += HoverOn;
        //LeaveRoomButton.MouseExited += HoverOff;
        LeaveRoomButton.Pressed += LeaveRoom;
        LeaveRoomButton.Pressed += HideUIAction;

        ClothingButton.Pressed += ToggleClothing;

        curedInAbsence = false;
    }

    private void GetNodes()
    {
        //Grab references
        LeaveRoomButton = GetNode<Button>("Leave_Room");
    }

    //SETTING UP TIMMER FOR RETURN
    private void SetupTimers()
{
    if (LeaveRoomButton != null)
    {
        LeaveRoomButton.Hide();
    }

    _inactivityTimer = new Timer();
    _inactivityTimer.WaitTime = 5.0f;
    _inactivityTimer.OneShot = true;
    _inactivityTimer.Timeout += OnInactivityTimeout;
    AddChild(_inactivityTimer);

    _displayTimer = new Timer();
    _displayTimer.WaitTime = 5.0f;
    _displayTimer.OneShot = true;
    _displayTimer.Timeout += OnDisplayTimeout;
    AddChild(_displayTimer);

    _inactivityTimer.Start();
}

public override void _UnhandledInput(InputEvent @event)
{
    if (_inactivityTimer == null) return;

    if (!_isButtonVisible)
    {
        if (@event is InputEventMouseMotion mouseMotion)
        {
            if (mouseMotion.Relative.LengthSquared() > 1.0f)
            {
                _inactivityTimer.Start();
            }
        }
        else if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
        {
            _inactivityTimer.Start();
        }
    }
}

private void OnInactivityTimeout()
{
    _isButtonVisible = true;
    if (LeaveRoomButton != null)
    {
        LeaveRoomButton.Show();
    }

    _displayTimer.Start();
}

private void OnDisplayTimeout()
{
    if (LeaveRoomButton != null && LeaveRoomButton.IsHovered())
    {
        _displayTimer.Start();
        return;
    }

    _isButtonVisible = false;
    if (LeaveRoomButton != null)
    {
        LeaveRoomButton.Hide();
    }

    _inactivityTimer.Start();
}

private void HoverOn()
{
    var label = LeaveRoomButton?.GetNodeOrNull<Label>("Label");
    if (label != null)
    {
        label.Modulate = Colors.White;
    }
}

private void HoverOff()
{
    var label = LeaveRoomButton?.GetNodeOrNull<Label>("Label");
    if (label != null)
    {
        label.Modulate = new Color(1, 1, 1, 0.8f);
    }
}



    public override void OnRoomEnter()
    {
        //Piece of logic that gets executed whenever you enter the room.
        TriggerFading();
        UpdateSprites();
        ShowSpeechDialogue();

        _isButtonVisible = false;  // Reset button visibility state
        if (LeaveRoomButton != null) LeaveRoomButton.Hide();
        if (_inactivityTimer != null) _inactivityTimer.Start(); 
    }

    public override void OnRoomExit()
    {
        //when leaving the room, hide it, show the office, and pop the room off the previous scenes stack, to not interfere with the right click functionality
        
        SpeechManagerAccess.SetBubbleStatus(false);
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
            WholePatient.Hide();
        }
    }

    private void SetPatientUIStatus(bool status, bool alive)
    {
        if(status)
        {
            if(alive)
            {
                WholePatient.Show();
                AssignPatientTextures();
                Corpse.Hide();
                //PatientInfoScreen.Show();
                UIControl.Show();
            }
            else
            {
                WholePatient.Hide();
                Corpse.Show();
                //PatientInfoScreen.Show();
                UIControl.Show();
            }
        }
        else
        {
            WholePatient.Hide();
            //PatientInfoScreen.Hide();
            UIControl.Hide();
        }
    }

    private void AssignPatientTextures()
    {
        TextureUnit unit = Patient.GetPatientTextures();
        PatientDisplay.Texture = unit.BodySet.sitting;
        PatientHead.Texture = unit.HeadSet.sitting;
        if (unit.unitType == TextureType.Normal)
        {
            if (Patient.malady.severity < 4) return;
            TopMaladySprite.Texture = null;
            MaladySprite.Texture = unit.MaladySet.sitting;
        }
        else if (unit.unitType == TextureType.Top)
        {
            if (Patient.malady.severity < 4) return;
            MaladySprite.Texture = null;
            TopMaladySprite.Texture = unit.MaladySet.sitting;
        }
    }

    private void SetPatientRoomText()
    {
        /*string input;
        if(Patient.IsPatientAlive())
        {
            input = "Alive";
        }
        else
        {
            input = "Dead";
        }
        string mainText = $"Malady: {Patient.malady.name}" +
            $" \n Age: {Patient.age}" +
            $" \n Severity: {Patient.malady.severity} " +
            $"\n Status: {input}";*/
        PatientInfoScreen.UpdateText(TabType.General, true);
        //PatientInfoScreen.Write(mainText, TabType.General, true);
        /*PatientInfo.Text = $"Malady: {Patient.malady.name}" +
            $" \n Age: {Patient.age}" +
            $" \n Severity: {Patient.malady.severity} " +
            $"\n Status: {input}";*/
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
        PatientDisplay.Texture = null;
        PatientHead.Texture = null;
        MaladySprite.Texture = null;
        TopMaladySprite.Texture = null;
        UpdateSprites();
    }

    public void AssignPatient(PatientStats patient)
    {
        
        Patient = patient;
        Patient.AssignRoom(this);
        PatientInfoScreen.SetPatient(Patient);
        isEmpty = false;
    }

    public void NewDay()
    {
        
        UpdateSprites();
    }


    public void PatientCuredInAbsence()
    {
        curedInAbsence = true;
        //DeletePatient();
    }

    public void SetAlreadyTreated(bool input)
    {
        notYetTreated = !input;
    }

    public bool GetAlreadyTreated()
    {
        return !notYetTreated;
    }
    public void IncrementTreated()
    {
        timesTreated++;
    }
    public int GetTimesTreated()
    {
        return timesTreated;
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

    private void TriggerFading()
    {
        // instantiate the scene FadeAnimation
        var fading = Transition.Instantiate<FadeAnimation>();
        // add the scene FadeAnimation and call the Methode Fades
        AddChild(fading);
        fading.Fades();
    }

    private void ToggleClothing()
    {
        if(ClothingSprite.IsVisibleInTree())
        {
            ClothingSprite.Hide();
        }
        else
        {
            ClothingSprite.Show();
        }
    }

    private void ShowClothing()
    {
        ClothingSprite.Show();
    }

    private void HideClothing()
    {
        ClothingSprite.Hide();
    }

}