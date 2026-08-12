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
            UpdateDoorVisuals();
        }
    }

    public override void _Ready()
    {
        UpdateDoorVisuals();
    }

    // Runs whenever this door button is clicked
    public override void _Pressed()
    {
        // Find the active Tutorial instance in the scene tree
        var tutorial = GetTree().Root.FindChild("Tutorial", recursive: true, owned: false) as Tutorial;

        if (!IsUnlocked)
        {
            // Triggers: "The room is in ruins, I'll need to pay to make it usable."
            tutorial?.ShowLockedDoorDialogue();
        }
        else
        {
            // (Optional) Add your unlocked door scene change logic here later
            GD.Print($"Door {DoorId} clicked (Unlocked)!");
        }
    }

    public void UpdateDoorVisuals()
    {
        // Keep the button active so Godot registers clicks even when locked
        Disabled = false;
        MouseFilter = MouseFilterEnum.Stop;

        if (IsUnlocked)
        {
            // Show door graphic and hide rubble
            if (UnlockedTexture != null)
                TextureNormal = UnlockedTexture;

            if (RubbleOverlay != null)
                RubbleOverlay.Visible = false;
        }
        else
        {
            // Hide unlocked graphic so only rubble is visible
            TextureNormal = null;

            if (RubbleOverlay != null)
                RubbleOverlay.Visible = true;
        }
    }
}