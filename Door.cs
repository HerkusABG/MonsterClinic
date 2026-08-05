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

    public void UpdateDoorVisuals()
{
    if (IsUnlocked)
    {
        // Show door graphic and enable interaction
        if (UnlockedTexture != null)
            TextureNormal = UnlockedTexture;

        Disabled = false;
        MouseFilter = MouseFilterEnum.Stop;

        // Hide rubble
        if (RubbleOverlay != null)
            RubbleOverlay.Visible = false;
    }
    else
    {
        // Hide door graphic so only rubble is visible
        TextureNormal = null;

        Disabled = true;
        MouseFilter = MouseFilterEnum.Ignore;

        // Show rubble
        if (RubbleOverlay != null)
            RubbleOverlay.Visible = true;
    }
}
}