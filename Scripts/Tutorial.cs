using Godot;

public partial class Tutorial : CanvasLayer
{
    
    [Export] public Control LockedDoorMessage { get; set; }
    [Export] public Label Label { get; set; }

    [Export] public bool EnableIdleHint { get; set; } = false;
    [Export] public string IdleHintMessage { get; set; } = "Click computer for supplies and upgrades!";
    [Export] public float IdleTimeLimit { get; set; } = 10.0f;

   //5.0 seconds for message on screen
    [Export] public float AutoHideDuration { get; set; } = 5.0f;

    private const string DefaultRuinedMessage = "The room is in ruins, I'll need to pay to make it usable.";
    private float _idleTimer = 0.0f;
    private float _autoHideTimer = 0.0f;
    private bool _hasShownIdleHint = false;

    public override void _Ready()
    {
        
        HideLockedDoorDialogue();
    }

    public override void _Process(double delta)
    {
        
        if (EnableIdleHint && !_hasShownIdleHint)
        {
            _idleTimer += (float)delta;

            if (_idleTimer >= IdleTimeLimit)
            {
                ShowLockedDoorDialogue(IdleHintMessage);
                _hasShownIdleHint = true;
            }
        }

        //Message only shows for 5 seconds (just incase player hasnt moved, dont need the messge on the screen too long))
        if (LockedDoorMessage != null && LockedDoorMessage.Visible)
        {
            _autoHideTimer -= (float)delta;
            if (_autoHideTimer <= 0.0f)
            {
                HideLockedDoorDialogue();
            }
        }
    }

    // Timer will reset when player make mouse movement or click
    public override void _Input(InputEvent @event)
    {
        if (@event.IsPressed() || @event is InputEventMouseMotion)
        {
            _idleTimer = 0.0f;
        }
    }

   
    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
        {
            if (LockedDoorMessage != null && LockedDoorMessage.Visible)
            {
                HideLockedDoorDialogue();
            }
        }
    }

        public void ShowLockedDoorDialogue(string message = DefaultRuinedMessage) //The message will dissappear after 5 seconds or click is made
    {
        if (Label != null)
        {
            Label.Text = message;
        }

        if (LockedDoorMessage != null)
        {
            LockedDoorMessage.Visible = true;
            _autoHideTimer = AutoHideDuration; 
        }
    }

   
    public void HideLockedDoorDialogue()
    {
        if (LockedDoorMessage != null)
        {
            LockedDoorMessage.Visible = false;
        }
    }
}