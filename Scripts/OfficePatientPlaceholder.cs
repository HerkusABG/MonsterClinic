using Godot;
using System;

public partial class OfficePatientPlaceholder : Sprite2D
{
    public override void _Ready()
    {
        // Force an immediate evaluation right when entering the scene tree
        UpdateSpriteVisibility();
    }

    public override void _Process(double delta)
    {
        // Keep it updated in real-time
        UpdateSpriteVisibility();
    }

    private void UpdateSpriteVisibility()
    {
        // If IsPatientInWindow is false, Visible becomes false
        this.Visible = GlobalData.IsPatientInWindow;
    }
}