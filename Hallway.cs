using Godot;
using System;
using System.Collections.Generic;

public partial class Hallway : ExpNode2D
{
    //Node that controls everything inside of the hallway
    Control HallwayControl;
    //Control node specifically for the doors.
    Control DoorControl;
    Button LeaveButton;
    List<BaseButton> Doors = new List<BaseButton>();
    [Export] Button LeaveRoomButton;
    [Export] PackedScene Transition = ResourceLoader.Load<PackedScene>("res://fade_animation.tscn");

    private Timer _inactivityTimer;
    private Timer _displayTimer;
    private bool _isButtonVisible = false;

    public void Initialize()
    {
        //Initializing the hallway, all the main methods.
        GetNodes();

        Subscribe();

        DoorInitialize();

        SetupTimers();
    }

    private void SetupTimers()
    {
        if (LeaveRoomButton != null)
        {
            LeaveRoomButton.Hide();
            var label = LeaveRoomButton.GetNodeOrNull<Label>("Label");
            if (label != null) label.Text = "RETURN";
        }

        // Timer 1: Waits for 5s of NO mouse movement before showing button
        _inactivityTimer = new Timer();
        _inactivityTimer.WaitTime = 5.0f;
        _inactivityTimer.OneShot = true;
        _inactivityTimer.Timeout += OnInactivityTimeout;
        AddChild(_inactivityTimer);

        // Timer 2: Keeps button visible on screen for 5s so player can move mouse and click
        _displayTimer = new Timer();
        _displayTimer.WaitTime = 5.0f;
        _displayTimer.OneShot = true;
        _displayTimer.Timeout += OnDisplayTimeout;
        AddChild(_displayTimer);

        _inactivityTimer.Start();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        // Only reset the inactivity countdown while the button is hidden
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
        // 5 seconds of inactivity reached: Show button and start 5s display window
        _isButtonVisible = true;
        if (LeaveRoomButton != null)
        {
            LeaveRoomButton.Show();
            var label = LeaveRoomButton.GetNodeOrNull<Label>("Label");
            if (label != null) label.Text = "RETURN";
        }

        _displayTimer.Start();
    }

    private void OnDisplayTimeout()
    {
        // If player is hovering over the button, extend visibility so it doesn't vanish mid-click
        if (LeaveRoomButton != null && LeaveRoomButton.IsHovered())
        {
            _displayTimer.Start();
            return;
        }

        // 5 seconds of display time elapsed: Hide button and wait for inactivity again
        _isButtonVisible = false;
        if (LeaveRoomButton != null)
        {
            LeaveRoomButton.Hide();
        }

        _inactivityTimer.Start();
    }

    private void GetNodes()
    {
        //Grabbing relevant nodes that will later be used in other parts of code.
        HallwayControl = GetNode<Control>("HallwayControl");
        //LeaveButton = HallwayControl.GetNode<Button>("Leave_Room");
        DoorControl = HallwayControl.GetNode<Control>("DoorControl");
        
    }

    private void Subscribe()
    {
        //Subscriptions. Basically assigning new methods to buttons.
        LeaveRoomButton.MouseEntered += HoverOn;
        LeaveRoomButton.MouseExited += HoverOff;
        LeaveRoomButton.Pressed += LeaveRoom;
    }

    private void DoorInitialize()
    {
        //Logic for generating door logic.
        Main main = GetParent() as Main;
        Inventory inv = GetParent().GetNode<Inventory>("Inventory");
        TreatmentManager treatment = inv.GetNode<TreatmentManager>("Treatment_Manager");
        //Doors
        int doorIndex = 0;
        foreach (Node child in DoorControl.GetChildren())
        {
            BaseButton childButton = child as BaseButton;
            if (childButton != null)
            {
                Doors.Add(childButton);
                Door doorButton = childButton as Door;
                doorButton.DoorId = doorIndex;
                doorIndex++;
                childButton.Pressed += () => GoToRoom(doorButton.DoorId);
                //childButton.Pressed += treatment.ShowUI;
                childButton.Disabled = false; // Enabled so player can click locked doors to see the "ruined room" prompt
            }
        }
    }

    private void GoToRoom(int index)
    {
        //CALLED WHEN ONE OF THE DOORS ARE PRESSED IN THE HALLWAY
        int unlockedRooms = Upgrades.IntUpgradeDatabase["Rooms"].incrementTarget;

        if (index < unlockedRooms)
        {
            // UNLOCKED: Hide any active door messages and enter room
            var tutorial = GetNodeOrNull<Tutorial>("Tutorial");
            if (tutorial != null)
            {
                tutorial.HideLockedDoorDialogue();
            }

            RoomTracker.EnterPatientRoom(index);
        }
        else
        {
            // LOCKED: Block room entry and show ruined room prompt
            var tutorial = GetNodeOrNull<Tutorial>("Tutorial");
            if (tutorial != null)
            {
                tutorial.ShowLockedDoorDialogue("The room is in ruins, I'll need to pay to make it usable.");
            }
        }
    }

    public void GoToRoom(ExpNode2D roomInput)
    {
        //CALLED WHEN "VISIT" BUTTON IS PRESSED IN THE ADMISSION
        RoomTracker.EnterPatientRoom(roomInput);
    }

    private void LeaveRoom()
    {
        //when leaving the room, hide it, show the office, and pop the room off the previous scenes stack, to not interfere with the right click functionality
        RoomTracker.GoBack();
    }

    public void ResetRoomUI()
    {
        Inventory inv = GetParent().GetNode<Inventory>("Inventory");
        TreatmentManager treatment = inv.GetNode<TreatmentManager>("Treatment_Manager");
    }

    public void UpdateHallwayUI()
    {
        // All doors remain click-enabled; GoToRoom handles unlocked vs locked logic
    }

    private void HoverOn()
    {
        var label = LeaveRoomButton?.GetNodeOrNull<Label>("Label");
        if (label != null)
        {
            label.Text = "RETURN";
            label.Modulate = Colors.White;
        }
    }

    private void HoverOff()
    {
        var label = LeaveRoomButton?.GetNodeOrNull<Label>("Label");
        if (label != null)
        {
            label.Text = "RETURN";
            label.Modulate = new Color(1, 1, 1, 0.8f);
        }
    }

    public override void OnRoomEnter(Node mainNode)
    {
        //GD.Print("Entering hallway");
        TriggerFading();
        UpdateHallwayUI();

        // Clear any old locked door messages when entering the hallway
        var tutorial = GetNodeOrNull<Tutorial>("Tutorial");
        if (tutorial != null)
        {
            tutorial.HideLockedDoorDialogue();
        }

        Inventory inv = mainNode.GetNode<Inventory>("Inventory");
        inv.InventoryActions();
        inv.Hide();

        // Start waiting for 5 seconds of inactivity upon entering the hallway
        _isButtonVisible = false;
        if (LeaveRoomButton != null) LeaveRoomButton.Hide();
        if (_inactivityTimer != null) _inactivityTimer.Start();
    }

    public override void OnRoomExit()
    {
        //GD.Print("Exiting hallway");
    }

    private void TriggerFading()
    {
        // instantiate the scene FadeAnimation
        var fading = Transition.Instantiate<FadeAnimation>();
        // add the scene FadeAnimation and call the Methode Fades
        AddChild(fading);
        fading.Fades();
    }
}