using Godot;
using GodotPlugins.Game;
using System;
using System.Collections.Generic;

public static class RoomTracker
{
    public static ActiveRoom activeRoom;
    private static Node Main;
    private static ExpNode2D CurrentScene;

    public static Dictionary<string, string> Database = new()
    {


    };

    public static void Initialize(Node main)
    {
        Main = main;
        CurrentScene = Main.GetNode("Office") as ExpNode2D;
        GD.Print(CurrentScene.Name);
        GD.Print(CurrentScene.Visible);
        EnterRoom(ActiveRoom.Office);
    }

    public static void GoBack()
    {
        CurrentScene.Hide();
        CurrentScene.OnRoomExit();
        GlobalData.PreviousScenes.Pop();

        Room room = CurrentScene as Room;
        if (room != null)
        {
            EnterRoom(ActiveRoom.Hallway);
            return;
        }
        Contents_P_I patientInterface = CurrentScene as Contents_P_I;
        if (patientInterface != null)
        {
            EnterRoom(ActiveRoom.Office);
            return;
        }
        Contents_C computer = CurrentScene as Contents_C;
        if (computer != null)
        {
            EnterRoom(ActiveRoom.Office);
            return;
        }
        Hallway hallway = CurrentScene as Hallway;
        if (hallway != null)
        {
            EnterRoom(ActiveRoom.Office);
            return;
        }
    }

    public static void EnterRoom(ActiveRoom input)
    {
        CurrentScene.Hide();
        CurrentScene.OnRoomExit();
        if(input == ActiveRoom.Office)
        {
            RoomTrack(ActiveRoom.Office);
            ExpNode2D OfficeScene = Main.GetNode("Office") as ExpNode2D;
            //OfficeScene.Show();
            CurrentScene = OfficeScene;

            OfficeScene.OnRoomEnter(Main);
        }
        else if (input == ActiveRoom.Hallway)
        {
            RoomTrack(ActiveRoom.Hallway);
            ExpNode2D HallwayScene = Main.GetNode("Hallway") as ExpNode2D;
            //HallwayScene.Show();
            CurrentScene = HallwayScene;

            HallwayScene.OnRoomEnter(Main);
        }
        else if(input == ActiveRoom.Admission)
        {
            RoomTrack(ActiveRoom.Admission);
            ExpNode2D PatientScene = Main.GetNode("Patient_Interface") as ExpNode2D;
            //PatientScene.Show();
            CurrentScene = PatientScene;

            PatientScene.OnRoomEnter(Main);
        }
        else if(input == ActiveRoom.Computer)
        {
            RoomTrack(ActiveRoom.Computer);
            ExpNode2D ComputerScene = Main.GetNode("Computer") as ExpNode2D;
            //ComputerScene.Show();
            CurrentScene = ComputerScene;

            ComputerScene.OnRoomEnter(Main);
        }
        CurrentScene.Show();
        GlobalData.PreviousScenes.Push(CurrentScene.GetPath());
    }

    public static void EnterPatientRoom(int index)
    {
        CurrentScene.Hide();
        CurrentScene.OnRoomExit();
        RoomTrack(ActiveRoom.PatientRoom);
        ExpNode2D RoomScene = RoomManager.RoomList[index];
        //RoomScene.Show();
        CurrentScene = RoomScene;

        RoomScene.OnRoomEnter();

        Inventory inv = Main.GetNode<Inventory>("Inventory");
        inv.InventoryActions();

        TreatmentManager treatment = inv.GetNode<TreatmentManager>("Treatment_Manager");
        Room room = RoomScene as Room;
        treatment.SetTreatmentRoomReference(room);

        CurrentScene.Show();
        GlobalData.PreviousScenes.Push(CurrentScene.GetPath());
    }

    public static void EnterPatientRoom(ExpNode2D roomInput)
    {
        CurrentScene.Hide();
        CurrentScene.OnRoomExit();
        RoomTrack(ActiveRoom.PatientRoom);
        ExpNode2D RoomScene = roomInput;
        //RoomScene.Show();
        CurrentScene = RoomScene;

        RoomScene.OnRoomEnter();

        Inventory inv = Main.GetNode<Inventory>("Inventory");
        inv.InventoryActions();

        TreatmentManager treatment = inv.GetNode<TreatmentManager>("Treatment_Manager");
        Room room = RoomScene as Room;
        treatment.SetTreatmentRoomReference(room);

        CurrentScene.Show();
        GlobalData.PreviousScenes.Push(RoomScene.GetPath());
    }
    public static void RoomTrack(ActiveRoom input)
    {
        activeRoom = input;
    }

    public static ActiveRoom GetRoomTrack()
    {
        return activeRoom;
    }

    public static bool IsInRoom(ActiveRoom input)
    {
        if(activeRoom == input)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

public enum ActiveRoom
{
    Office,
    Admission,
    Computer,
    PatientRoom,
    Hallway
}