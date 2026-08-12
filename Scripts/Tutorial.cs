using Godot;
using System;
using System.Collections.Generic;

public partial class Tutorial : CanvasLayer
{
    [Export] public Timer InactivityTimer { get; set; }
    [Export] public PanelContainer PopupPanel { get; set; }
    [Export] public Label HintLabel { get; set; }

    
    [Export] public bool IsOfficeScene { get; set; } = false;

    private bool _hasTriggered = false;

    public override void _Ready()
    {
        if (PopupPanel != null)
            PopupPanel.Visible = false;

        if (IsOfficeScene)
        {
            // --- OFFICE LOGIC ---
            if (HintLabel != null)
                HintLabel.Text = "Click Computer to view Patients & Supplies!";

            if (InactivityTimer != null)
            {
                InactivityTimer.Timeout += OnTimerTimeout;
                InactivityTimer.Start();
            }
        }
        else
        {
            // --- HALLWAY LOGIC ---
            // Finds all doors and listens for clicks on their rubble overlays
            HookUpDoorRubbleClicks();
        }
    }

    private void HookUpDoorRubbleClicks()
    {
        // Finds every node of type Door in the current scene
        foreach (Node node in GetTree().CurrentScene.FindChildren("*", "Door", recursive: true, owned: false))
        {
            if (node is Door doorNode && doorNode.RubbleOverlay != null)
            {
                // Ensure the rubble receives click events
                doorNode.RubbleOverlay.MouseFilter = Control.MouseFilterEnum.Stop;
                
                // Connect click event to trigger dialogue
                doorNode.RubbleOverlay.GuiInput += (inputEvent) => OnRubbleClicked(inputEvent, doorNode);
            }
        }
    }

    private void OnRubbleClicked(InputEvent @event, Door door)
    {
        // Only trigger message if the door is currently locked (showing rubble)
        if (!door.IsUnlocked && @event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
        {
            ShowDoorDialogue("The room is in ruins, I'll need to pay to make it usable.");
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (!IsOfficeScene) return;

        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
        {
            if (PopupPanel != null && PopupPanel.Visible)
            {
                PopupPanel.Visible = false;
                InactivityTimer?.Stop();
            }
            else if (!_hasTriggered)
            {
                InactivityTimer?.Start();
            }
        }
    }

    private void OnTimerTimeout()
    {
        if (IsOfficeScene && !_hasTriggered)
        {
            _hasTriggered = true;

            if (PopupPanel != null)
                PopupPanel.Visible = true;

            InactivityTimer?.Stop();
        }
    }

    public async void ShowDoorDialogue(string message)
    {
        if (HintLabel != null)
            HintLabel.Text = message;

        if (PopupPanel != null)
            PopupPanel.Visible = true;

        InactivityTimer?.Stop();

        // Auto-hide popup after 2.5 seconds
        await ToSignal(GetTree().CreateTimer(2.5f), SceneTreeTimer.SignalName.Timeout);

        if (PopupPanel != null)
            PopupPanel.Visible = false;
    }
}