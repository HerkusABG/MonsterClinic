using Godot;
using System;
using System.Collections.Generic;

public partial class Tutorial : CanvasLayer
{
[Export] public Timer InactivityTimer { get; set; }
[Export] public PanelContainer PopupPanel { get; set; }
[Export] public Label HintLabel { get; set; }

    private bool _hasTriggered = false;

    public override void _Ready()
    {
        if (PopupPanel != null)
            PopupPanel.Visible = false;

        // Set the initial directive text
        if (HintLabel != null)
            HintLabel.Text = "Click Computer to view Patients & Supplies!";

        if (InactivityTimer != null)
        {
            InactivityTimer.Timeout += OnTimerTimeout;
            InactivityTimer.Start();
        }
    }

    public override void _Input(InputEvent @event)
    {
        // On any mouse click, dismiss the popup and stop the tutorial permanently
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
        {
            if (PopupPanel != null && PopupPanel.Visible)
            {
                PopupPanel.Visible = false;
                
                // Stop the timer so this tutorial NEVER triggers again
                InactivityTimer?.Stop();
            }
            else if (!_hasTriggered)
            {
                // Reset the 10-second timer as long as the tutorial hasn't shown up yet
                InactivityTimer?.Start();
            }
        }
    }

    private void OnTimerTimeout()
    {
        // Trigger the tutorial popup only once
        if (!_hasTriggered)
        {
            _hasTriggered = true;

            if (PopupPanel != null)
            {
                PopupPanel.Visible = true;
            }

            // Stop the timer from running in the background
            InactivityTimer?.Stop();
        }
    }
}