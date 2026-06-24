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

        for (int i = 0; i < Upgrades.roomCount; i++)
        {
            Room room = RoomList[i] as Room;
            if(room.isEmpty)
            {
                room.isEmpty = false;
                return room;
            }
        }
        /*foreach(Node2D roomListEntry in RoomList)
        {
            Room room = roomListEntry as Room;
            if(room.isEmpty)
            {
                room.isEmpty = false;
                return room;
            }
        }*/
        return null;
    }

    public static int GetEmptyRoomCount()
    {
        int count = 0;
        for(int i = 0; i < Upgrades.roomCount; i++)
        {
            Room room = RoomList[i] as Room;
            if (room.isEmpty)
            {
                count++;
            }
        }
        /*foreach (Node2D roomListEntry in RoomList)
        {
            Room room = roomListEntry as Room;
            if (room.isEmpty)
            {
                count++;
            }
        }*/
        return count;
    }

    public static void NewDay()
    {
        foreach(Room room in RoomList)
        {
            room.alreadyTreated = false;
        }
    }
}
