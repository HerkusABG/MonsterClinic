using Godot;

public partial class Door : TextureButton
{
    [Export] public int DoorId { get; set; }
    [Export] public Texture2D UnlockedTexture { get; set; }
    [Export] public Control RubbleOverlay { get; set; }

    private bool _isUnlocked;

    [Export]
    public bool IsUnlocked
    {
        get => _isUnlocked;
        set
        {
            _isUnlocked = value;

            // Only run visual updates if the node is fully loaded in the scene
            if (IsNodeReady())
            {
                UpdateDoorVisuals();
            }
        }
    }

    public override void _Ready()
    {
        UpdateDoorVisuals();
    }

    // Called when the player clicks the door
    public override void _Pressed()
    {
        // Find the active Tutorial scene in the tree
        var tutorial = GetTree()?.Root?.FindChild("Tutorial", recursive: true, owned: false) as Tutorial;

        if (!IsUnlocked)
        {
            // Triggers: "The room is in ruins, I'll need to pay to make it usable."
            if (GodotObject.IsInstanceValid(tutorial))
            {
                tutorial.ShowLockedDoorDialogue();
            }
        }
        else
        {
            // UNLOCKED: Close dialogue box immediately if it was open
            if (GodotObject.IsInstanceValid(tutorial))
            {
                tutorial.HideLockedDoorDialogue();
            }
        }
    }

    public void UpdateDoorVisuals()
    {
        // Keep the button active so Godot registers clicks in both states
        Disabled = false;
        MouseFilter = MouseFilterEnum.Stop;

        if (IsUnlocked)
        {
            // UNLOCKED: Show door texture, hide rubble
            if (UnlockedTexture != null)
            {
                TextureNormal = UnlockedTexture;
            }

            if (GodotObject.IsInstanceValid(RubbleOverlay))
            {
                RubbleOverlay.Visible = false;
            }
        }
        else
        {
            // LOCKED: Hide door texture, show rubble
            TextureNormal = null;

            if (GodotObject.IsInstanceValid(RubbleOverlay))
            {
                RubbleOverlay.Visible = true;
                
                // Allow mouse clicks to pass through RubbleOverlay to this TextureButton
                RubbleOverlay.MouseFilter = MouseFilterEnum.Ignore;
            }
        }
    }
}