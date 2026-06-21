using Godot;
using System;
using System.Collections.Generic;


public static class RoomManager
{
    public static List<Node2D> RoomList = new List<Node2D>();

    public static Node2D FindEmptyRoom()
    {
        foreach(Node2D roomListEntry in RoomList)
        {
            Room room = roomListEntry as Room;
            if(room.isEmpty)
            {
                room.isEmpty = false;
                return room;
            }
        }
        return null;
    }

    public static int GetEmptyRoomCount()
    {
        int count = 0;
        foreach (Node2D roomListEntry in RoomList)
        {
            Room room = roomListEntry as Room;
            if (room.isEmpty)
            {
                count++;
            }
        }
        return count;
    }
}
