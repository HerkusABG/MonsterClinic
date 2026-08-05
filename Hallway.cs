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

    public void Initialize()
    {
        //Initializing the hallway, all the main methods.
        GetNodes();

        Subscribe();

        DoorInitialize();
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
        int doorIndex = 0;
        foreach (Node child in DoorControl.GetChildren())
        {
            if (child is Door doorButton)
            {
                Doors.Add(doorButton);
                doorButton.DoorId = doorIndex;
                doorButton.IsUnlocked = (doorIndex == 0);

                // Pass the door index directly to your team's existing GoToRoom method
                int index = doorIndex;
                doorButton.Pressed += () => GoToRoom(index);

                doorIndex++;
            }
        }
    }

    private void GoToRoom(int index)
    {
        //CALLED WHEN ONE OF THE DOORS ARE PRESSED IN THE HALLWAY
        RoomTracker.EnterPatientRoom(index);
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
        // show Dialog in the office, if the dialog didnt ended.
        var DialogScene = (Control)GetParent().GetNode("Dialog");
        if(GlobalData.Dialog_Dealer == true)
        {
            DialogScene.Show();
        }
        else
        {
            DialogScene.Hide();
        }
    }

    public void ResetRoomUI()
    {
        Inventory inv = GetParent().GetNode<Inventory>("Inventory");
        TreatmentManager treatment = inv.GetNode<TreatmentManager>("Treatment_Manager");
    }

    public void UpdateHallwayUI()
    {
        for(int i = 0; i < Upgrades.IntUpgradeDatabase["Rooms"].incrementTarget; i++)
        {
            if (Doors[i] is Door door)
            {
                door.IsUnlocked = true;
            }
        }
    }

    private void HoverOn()
    {
        //makes the text show up when hovering over the button
        LeaveRoomButton.Text = "Leave";
    }

    private void HoverOff()
    {
        //makes the text disappear when you stop hovering
        LeaveRoomButton.Text = "";
    }

    public override void OnRoomEnter(Node mainNode)
    {
        //GD.Print("Entering hallway");

        UpdateHallwayUI();

        Inventory inv = mainNode.GetNode<Inventory>("Inventory");
        inv.InventoryActions();
    }

    public override void OnRoomExit()
    {
        //GD.Print("Exiting hallway");
    }
}