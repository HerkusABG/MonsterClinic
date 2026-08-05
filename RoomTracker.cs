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
        // SAFEGUARD: Guard against invalid Main reference before attempting node retrieval
        if (Main != null)
        {
            CurrentScene = Main.GetNodeOrNull<ExpNode2D>("Office");
        }
        EnterRoom(ActiveRoom.Office);
    }

    public static void GoBack()
    {
        // SAFEGUARD: Prevent null reference error if CurrentScene is unassigned during back navigation
        if (CurrentScene != null)
        {
            CurrentScene.Hide();
            CurrentScene.OnRoomExit();
        }

        if (GlobalData.PreviousScenes != null && GlobalData.PreviousScenes.Count > 0)
        {
            GlobalData.PreviousScenes.Pop();
        }

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
        if (CurrentScene != null)
        {
            CurrentScene.Hide();
            CurrentScene.OnRoomExit();
        }

        // SAFEGUARD: Ensure Main node reference exists before attempting to fetch child scenes
        if (Main == null)
        {
            GD.PrintErr("RoomTracker Error: Main node reference is null in EnterRoom.");
            return;
        }

        ExpNode2D targetScene = null;

        if (input == ActiveRoom.Office)
        {
            RoomTrack(ActiveRoom.Office);
            // SAFEGUARD: Replaced GetNode with GetNodeOrNull to avoid unhandled native exceptions
            targetScene = Main.GetNodeOrNull<ExpNode2D>("Office");
        }
        else if (input == ActiveRoom.Hallway)
        {
            RoomTrack(ActiveRoom.Hallway);
            targetScene = Main.GetNodeOrNull<ExpNode2D>("Hallway");
        }
        else if (input == ActiveRoom.Admission)
        {
            RoomTrack(ActiveRoom.Admission);
            targetScene = Main.GetNodeOrNull<ExpNode2D>("Patient_Interface");
            //PatientScene.Show();
        }
        else if(input == ActiveRoom.Computer)
        {
            RoomTrack(ActiveRoom.Computer);
            targetScene = Main.GetNodeOrNull<ExpNode2D>("Computer");
            //ComputerScene.Show();
        }

        // SAFEGUARD: Validate retrieved target scene exists before calling operations on it (Fixes line 84 NRE)
        if (targetScene == null)
        {
            GD.PrintErr($"RoomTracker Error: Could not find node for room state '{input}' under Main.");
            return;
        }

        CurrentScene = targetScene;
        CurrentScene.OnRoomEnter(Main);
        CurrentScene.Show();

        if (GlobalData.PreviousScenes != null)
        {
            GlobalData.PreviousScenes.Push(CurrentScene.GetPath());
        }
    }

    public static void EnterPatientRoom(int index)
    {
        if (CurrentScene != null)
        {
            CurrentScene.Hide();
            CurrentScene.OnRoomExit();
        }

        RoomTrack(ActiveRoom.PatientRoom);

        // SAFEGUARD: Check if RoomList exists and index is within valid range
        if (RoomManager.RoomList == null || index < 0 || index >= RoomManager.RoomList.Count)
        {
            GD.PrintErr($"RoomTracker Error: Invalid room index {index}.");
            return;
        }

        ExpNode2D RoomScene = RoomManager.RoomList[index];
        //RoomScene.Show();

        if (RoomScene == null)
        {
            GD.PrintErr($"RoomTracker Error: Room at index {index} is null.");
            return;
        }

        CurrentScene = RoomScene;
        RoomScene.OnRoomEnter();

        // SAFEGUARD: Safe retrieval of Inventory and TreatmentManager to avoid crashes on missing UI nodes
        if (Main != null)
        {
            Inventory inv = Main.GetNodeOrNull<Inventory>("Inventory");
            if (inv != null)
            {
                inv.InventoryActions();

                TreatmentManager treatment = inv.GetNodeOrNull<TreatmentManager>("Treatment_Manager");
                if (treatment != null)
                {
                    Room room = RoomScene as Room;
                    treatment.SetTreatmentRoomReference(room);
                }
            }
        }

        CurrentScene.Show();

        if (GlobalData.PreviousScenes != null)
        {
            GlobalData.PreviousScenes.Push(CurrentScene.GetPath());
        }
    }

    public static void EnterPatientRoom(ExpNode2D roomInput)
    {
        if (CurrentScene != null)
        {
            CurrentScene.Hide();
            CurrentScene.OnRoomExit();
        }

        // SAFEGUARD: Verify passed room node is not null
        if (roomInput == null)
        {
            GD.PrintErr("RoomTracker Error: roomInput passed to EnterPatientRoom is null.");
            return;
        }

        RoomTrack(ActiveRoom.PatientRoom);
        ExpNode2D RoomScene = roomInput;
        //RoomScene.Show();
        CurrentScene = RoomScene;

        RoomScene.OnRoomEnter();

        if (Main != null)
        {
            Inventory inv = Main.GetNodeOrNull<Inventory>("Inventory");
            if (inv != null)
            {
                inv.InventoryActions();

                TreatmentManager treatment = inv.GetNodeOrNull<TreatmentManager>("Treatment_Manager");
                if (treatment != null)
                {
                    Room room = RoomScene as Room;
                    treatment.SetTreatmentRoomReference(room);
                }
            }
        }

        CurrentScene.Show();

        if (GlobalData.PreviousScenes != null)
        {
            GlobalData.PreviousScenes.Push(RoomScene.GetPath());
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