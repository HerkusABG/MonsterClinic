using Godot;
using GodotPlugins.Game;
using System;
using System.Collections.Generic;

public static class RoomTracker
{
    public static ActiveRoom activeRoom;
    private static Node Main;
    private static Node2D CurrentScene;

    public static Dictionary<string, string> Database = new()
    {


    };

    public static void Initialize(Node main)
    {
        Main = main;
        CurrentScene = Main.GetNode("Office") as Node2D;
        EnterRoom(ActiveRoom.Office);
    }

    public static void GoBack()
    {
        CurrentScene.Hide();
        GlobalData.PreviousScenes.Pop();

        Room room = CurrentScene as Room;
        if (room != null)
        {
            EnterRoom(ActiveRoom.Hallway);
        }
        Contents_P_I patientInterface = CurrentScene as Contents_P_I;
        if (patientInterface != null)
        {
            EnterRoom(ActiveRoom.Office);
            patientInterface.HideSpeechBubble();
        }
        //pop a scene again, this is the scene we were previously in
        //var parent = (Node2D)GetNode(GlobalData.PreviousScenes.Peek().ToString
        Node2D parent = (Node2D)Main.GetNode(GlobalData.PreviousScenes.Peek().ToString()) as Node2D;
        //GD.Print(GlobalData.PreviousScenes.Peek().ToString());
        //Node2D parent = Main.GetNode("Office") as Node2D;
        //GD.Print(parent.Name);
        //show it
        parent.Show();
        //GD.Print("entering " + parent.Name);
    }

    public static void EnterRoom(ActiveRoom input)
    {
        CurrentScene.Hide();
        if(input == ActiveRoom.Office)
        {
            RoomTrack(ActiveRoom.Office);
            Node2D OfficeScene = Main.GetNode("Office") as Node2D;
            OfficeScene.Show();
            CurrentScene = OfficeScene;
            //GlobalData.PreviousScenes.Pop();

            Inventory inv = Main.GetNode<Inventory>("Inventory");
            inv.InventoryActions();

            GlobalData.PreviousScenes.Push(OfficeScene.GetPath());
        }
        else if (input == ActiveRoom.Hallway)
        {
            RoomTrack(ActiveRoom.Hallway);
            Node2D HallwayScene = Main.GetNode("Hallway") as Node2D;
            HallwayScene.Show();
            CurrentScene = HallwayScene;

            Hallway hallway = HallwayScene as Hallway;
            hallway.UpdateHallwayUI();

            Inventory inv = Main.GetNode<Inventory>("Inventory");
            inv.InventoryActions();

            GlobalData.PreviousScenes.Push(HallwayScene.GetPath());
        }
        else if(input == ActiveRoom.Admission)
        {
            RoomTrack(ActiveRoom.Admission);
            Node2D PatientScene = Main.GetNode("Patient_Interface") as Node2D;
            PatientScene.Show();
            CurrentScene = PatientScene;

            Contents_P_I PatientInterface = PatientScene as Contents_P_I;
            PatientInterface.UpdatePatientInterfaceUI();

            Hallway hallway = Main.GetNode<Hallway>("Hallway");
            hallway.UpdateHallwayUI();

            Inventory inv = Main.GetNode<Inventory>("Inventory");
            inv.InventoryActions();

            //push the scene we're entering to the previous scenes stack
            GlobalData.PreviousScenes.Push(PatientScene.GetPath());
        }
        else if(input == ActiveRoom.Computer)
        {
            RoomTrack(ActiveRoom.Computer);
            Node2D ComputerScene = Main.GetNode("Computer") as Node2D;
            ComputerScene.Show();
            CurrentScene = ComputerScene;

            //push the scene we're entering to the previous scenes stack
            GlobalData.PreviousScenes.Push(ComputerScene.GetPath());
        }
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