using Godot;
using System;
using System.Collections.Generic;

public partial class Hallway : Node2D
{
	Control HallwayControl;
	Control DoorControl;
    Button LeaveButton;
    List<Button> Doors =  new List<Button>();

    public void HallwayInitialize()
	{
        HallwayControl = GetNode<Control>("HallwayControl");
        LeaveButton = HallwayControl.GetNode<Button>("Leave_Room");
        DoorControl = HallwayControl.GetNode<Control>("DoorControl");

        LeaveButton.Pressed += LeaveRoom;

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
            }
        }
    }

    private void GoToRoom(int index)
    {
        Hide();

        //var RoomScene = (Node2D)GetParent().GetNode("Room");
        //GD.Print($"Room count: {RoomList.Count}.");
        var RoomScene = RoomManager.RoomList[index];
        RoomScene.Show();
        Room room = RoomScene as Room;
        room.UpdatePatientInfoLabel();

        //push the scene we're entering to the previous scenes stack
        GlobalData.PreviousScenes.Push(RoomScene.GetPath());
    }

    private void LeaveRoom()
    {
        //when leaving the room, hide it, show the office, and pop the room off the previous scenes stack, to not interfere with the right click functionality
        Hide();
        var OfficeScene = (Node2D)GetParent().GetNode("Office");
        OfficeScene.Show();
        if(GlobalData.PreviousScenes.Count != 0)
        {
            GlobalData.PreviousScenes.Pop();
        }
    }

    public void ResetRoomUI()
    {
        foreach(Room room in RoomManager.RoomList)
        {
           TreatmentManager treatment = room.GetNode<TreatmentManager>("Treatment_Manager");
           treatment.ReenableMedicine();

            room.UpdatePatientInfoLabel();
        }
    }
}
