using Godot;
using System;
using System.Collections.Generic;

public partial class Hallway : Node2D
{
	Control HallwayControl;
	Control DoorControl;
    Button LeaveButton;
    List<Button> Doors =  new List<Button>();

    public int testInt = 69;
    [Export] Button LeaveRoomButton;
    public void HallwayInitialize()
	{
        HallwayControl = GetNode<Control>("HallwayControl");
        LeaveButton = HallwayControl.GetNode<Button>("Leave_Room");
        DoorControl = HallwayControl.GetNode<Control>("DoorControl");

        LeaveRoomButton.MouseEntered += HoverOn;
        LeaveRoomButton.MouseExited += HoverOff;
        LeaveRoomButton.Pressed += LeaveRoom;

        GD.Print(GetParent().Name);
        Main main = GetParent() as Main;

        Inventory inv = GetParent().GetNode<Inventory>("Inventory");
        TreatmentManager treatment = inv.GetNode<TreatmentManager>("Treatment_Manager");
        //Doors
        int doorIndex = 0;
        foreach (Node child in DoorControl.GetChildren())
        {
            if (child.GetClass() == "Button")
            {
                Button childButton = child as Button;
                Doors.Add(childButton);
                Door doorButton = childButton as Door;
                doorButton.doorId = doorIndex;
                doorIndex++;
                childButton.Pressed += () => GoToRoom(doorButton.doorId);
                childButton.Pressed += treatment.ShowUI;
                childButton.Pressed += main.InventoryPatientRoom;
                childButton.Disabled = true;
            }
        }
    }

    private void GoToRoom(int index)
    {
        Hide();
        GlobalData.inPatientRoom = true;
        //var RoomScene = (Node2D)GetParent().GetNode("Room");
        //GD.Print($"Room count: {RoomList.Count}.");
        var RoomScene = RoomManager.RoomList[index];
        RoomScene.Show();
        Room room = RoomScene as Room;
        room.UpdatePatientInfoLabel();
        Inventory inv = GetParent().GetNode<Inventory>("Inventory");
        //extra safeguards to ensure the medicine buttons are disabled if the room's patient has already been treated that day, since now they can also be treated from the map
        TextureButton GiveMedicine1Button = inv.GetNode("Open_Inventory").GetNode<TextureButton>("Give_Medicine_1");
        TextureButton GiveMedicine2Button = inv.GetNode("Open_Inventory").GetNode<TextureButton>("Give_Medicine_2");
        TextureButton GiveMedicine3Button = inv.GetNode("Open_Inventory").GetNode<TextureButton>("Give_Medicine_3");
        if (room.alreadyTreated)
        {
            GiveMedicine1Button.Disabled = true;
            GiveMedicine2Button.Disabled = true;
            GiveMedicine3Button.Disabled = true;
        }
        TreatmentManager treatment = inv.GetNode<TreatmentManager>("Treatment_Manager");
        treatment.SetTreatmentRoomReference(room);
        //push the scene we're entering to the previous scenes stack
        GlobalData.PreviousScenes.Push(RoomScene.GetPath());
    }

    public void GoToRoom(Node2D roomInput)
    {
        Hide();
        GlobalData.inPatientRoom = true;
        //var RoomScene = (Node2D)GetParent().GetNode("Room");
        //GD.Print($"Room count: {RoomList.Count}.");
        roomInput.Show();
        Room room = roomInput as Room;
        room.UpdatePatientInfoLabel();
        Inventory inv = GetParent().GetNode<Inventory>("Inventory");
        //extra safeguards to ensure the medicine buttons are disabled if the room's patient has already been treated that day, since now they can also be treated from the map
        TextureButton GiveMedicine1Button = inv.GetNode("Open_Inventory").GetNode<TextureButton>("Give_Medicine_1");
        TextureButton GiveMedicine2Button = inv.GetNode("Open_Inventory").GetNode<TextureButton>("Give_Medicine_2");
        TextureButton GiveMedicine3Button = inv.GetNode("Open_Inventory").GetNode<TextureButton>("Give_Medicine_3");
        if (room.alreadyTreated)
        {
            GiveMedicine1Button.Disabled = true;
            GiveMedicine2Button.Disabled = true;
            GiveMedicine3Button.Disabled = true;
        }
        TreatmentManager treatment = inv.GetNode<TreatmentManager>("Treatment_Manager");
        treatment.SetTreatmentRoomReference(room);
        //treatment.ShowUI();
        //push the scene we're entering to the previous scenes stack
        GlobalData.PreviousScenes.Push(roomInput.GetPath());
    }

    private void LeaveRoom()
    {
        //when leaving the room, hide it, show the office, and pop the room off the previous scenes stack, to not interfere with the right click functionality
        Hide();
        GlobalData.inPatientRoom = false;
        var OfficeScene = (Node2D)GetParent().GetNode("Office");
        OfficeScene.Show();
        if(GlobalData.PreviousScenes.Count != 0)
        {
            GlobalData.PreviousScenes.Pop();
        }
    }

    public void ResetRoomUI()
    {
        Inventory inv = GetParent().GetNode<Inventory>("Inventory");
        TreatmentManager treatment = inv.GetNode<TreatmentManager>("Treatment_Manager");
        treatment.ReenableMedicine();
        /* foreach(Room room in RoomManager.RoomList)
         {
            TreatmentManager treatment = room.GetNode<TreatmentManager>("Treatment_Manager");
            treatment.ReenableMedicine();

            room.UpdatePatientInfoLabel();
         }*/
    }

    public void UpdateHallwayUI()
    {
        for(int i = 0; i < Upgrades.roomCount; i++)
        {
            Doors[i].Disabled = false;
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

    
}
