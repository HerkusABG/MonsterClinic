using Godot;
using System;
using System.Collections.Generic;


public static class RoomManager
{
    public static List<Node2D> RoomList;

    public static void Initialize()
    {
        RoomList = new List<Node2D>();
    }

    public static Node2D FindEmptyRoom()
    {
        //Go through the list of empty rooms and find one.
        for (int i = 0; i < Upgrades.roomCount; i++)
        {
            Room room = RoomList[i] as Room;
            if(!room.HasPatient())
            {
                return room;
            }
        }
        return null;
    }

    public static int GetEmptyRoomCount()
    {
        //Find all empty rooms.
        int count = 0;
        for(int i = 0; i < Upgrades.roomCount; i++)
        {
            Room room = RoomList[i] as Room;
            if (!room.HasPatient())
            {
                count++;
            }
        }
        return count;
    }

    public static void NewDay()
    {
        //Make the actions in the different rooms renewable daily.
        foreach(Room room in RoomList)
        {
            room.SetAlreadyTreated(false);
            if (room.HasPatient())
            {
                room.Patient.TriggerDailyTags();
            }
        }
    }
}
