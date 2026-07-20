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
        for (int i = 0; i < Upgrades.IntUpgradeDatabase["Rooms"].incrementTarget; i++)
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
        for(int i = 0; i < Upgrades.IntUpgradeDatabase["Rooms"].incrementTarget; i++)
        {
            Room room = RoomList[i] as Room;
            if (!room.HasPatient())
            {
                count++;
            }
        }
        return count;
    }

    public static int GetDeadPatientCount()
    {
        //Find all empty rooms.
        int count = 0;
        for (int i = 0; i < Upgrades.IntUpgradeDatabase["Rooms"].incrementTarget; i++)
        {
            Room room = RoomList[i] as Room;
            if (room.HasPatient())
            {
                if (!room.Patient.IsPatientAlive())
                {
                    count++;
                }
            }
        }
        return count;
    }

    public static Room[] GetAllDeadPatients()
    {
        int count = 0;
        int length = GetDeadPatientCount();
        Room[] array = new Room[length];
        for (int i = 0; i < Upgrades.IntUpgradeDatabase["Rooms"].incrementTarget; i++)
        {
            Room room = RoomList[i] as Room;
            if (room.HasPatient())
            {
                if (!room.Patient.IsPatientAlive())
                {
                    array[count] = room;
                    count++;
                }
            }
        }
        return array;
    }

    public static Room FindDeadPatient()
    {
        //Go through the list of empty rooms and find one.
        for (int i = 0; i < Upgrades.IntUpgradeDatabase["Rooms"].incrementTarget; i++)
        {
            Room room = RoomList[i] as Room;
            if (room.HasPatient())
            {
                if (!room.Patient.IsPatientAlive())
                {
                    GD.Print("Return room");
                    return room;
                }
            }
        }
        GD.Print("Return null");
        return null;
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
