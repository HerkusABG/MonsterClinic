using Godot;
using System;
using System.Collections.Generic;

public partial class Main : Node
{
    Node2D Office;
    Node2D Computer;
    Node2D PatientInterface;
    Node2D PatientRoom;
    Node2D Bed;
    Node2D Pause;
    Node2D Inventory;
    TreatmentManager Treatment;
// Called when the node enters the scene tree for the first time.
    [Export] Control RoomControl;
    int finalRoomCount = 6;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        InitializeAllData();
        GetNodes();

        //uncomment after the related issue in MapUI is resolved
        //Upgrades.NewGameRoomReset();

        //always keep the office at the bottom of the previous scenes stack, so the reference on how to return to it is always there
        GlobalData.PreviousScenes.Push(GetNode("Office").GetPath());
        GeneratePatientRooms(RoomControl);
        
	}

    private void GetNodes()
    {
        Office = GetNode<Node2D>("Office");
        Computer = GetNode<Node2D>("Computer");
        PatientInterface = GetNode<Node2D>("Patient_Interface");
        PatientRoom = GetNode<Node2D>("Room");
        Bed = GetNode<Node2D>("Bed");
        Pause = GetNode<Node2D>("Pause");
        Inventory = GetNode<Node2D>("Inventory");
        Inventory inv = Inventory as Inventory;
        inv.Initialize();
        Treatment = inv.GetNode<TreatmentManager>("Treatment_Manager");
    }
    private void InitializeAllData()
    {
        GD.Print("init!");
        RoomManager.Initialize();
        Hallway hallway = GetNode<Node2D>("Hallway") as Hallway;
        hallway.HallwayInitialize();
        //GD.Print("Line 44 in Main.cs");
    }

    private void GeneratePatientRooms(Control roomControl)
    {
        var patientRoom = (Node2D)GetNode("Room");
        Random random = new Random();
        for (int i = 0; i < finalRoomCount; i++)
        {
            Node2D newRoom = (Node2D)patientRoom.Duplicate();
            newRoom.Hide();
            //newRoom.Modulate = new Color((float)(random.NextDouble()), (float)(random.NextDouble()), 0, 1);
            roomControl.AddChild(newRoom);
            RoomManager.RoomList.Add(newRoom);
            Room room = newRoom as Room;
            room.myIndex = i;
            room.Initialize(Treatment.HideUI);
        }
    }
    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton eventKey)
        {
            //if a key is pressed and that key is the right mouse button, and if the pause menu and the office aren't visible
            if (eventKey.Pressed && eventKey.ButtonIndex == MouseButton.Right && Pause.Visible == false && Office.Visible == false)
            {
                //pop a scene from the previous scenes stack, this is the scene currently in use
                var current_scene = (Node2D)GetNode(GlobalData.PreviousScenes.Pop().ToString());
                //hide it
                current_scene.Hide();
                Room room = current_scene as Room;
                if(room != null)
                {
                    Treatment.HideUI();
                }
                Contents_P_I patientInterface = current_scene as Contents_P_I;
                if (patientInterface != null)
                {
                    patientInterface.HideSpeechBubble();
                }
                //pop a scene again, this is the scene we were previously in
                var parent = (Node2D)GetNode(GlobalData.PreviousScenes.Peek().ToString());
                //show it
                parent.Show();
            }
        }
    }

    //this, and the next 2 methods are for showing the inventory in the scenes it's meant to be accessible, and hiding it otherwise
    private void _on_office_visibility_changed()
    {
        if(Office != null)
        {
            if (Office.Visible == true)
            {
                Inventory.Show();
            }
            else
            {
                Inventory.Hide();
            }
        }
    }

    private void _on_patient_interface_visibility_changed()
    {
        if (PatientInterface == null) return;
        if (PatientInterface.Visible == true)
        {
            Inventory.Show();
        }
        else
        {
            Inventory.Hide();
        }
    }

    private void _on_room_visibility_changed()
    {
        if (Treatment == null) return;
        var GiveMedicine1Button = GetNode("Inventory").GetNode("Open_Inventory").GetNode<TextureButton>("Give_Medicine_1");
        var GiveMedicine2Button = GetNode("Inventory").GetNode("Open_Inventory").GetNode<TextureButton>("Give_Medicine_2");
        var GiveMedicine3Button = GetNode("Inventory").GetNode("Open_Inventory").GetNode<TextureButton>("Give_Medicine_3");
        if (GlobalData.inPatientRoom)
        {
            Inventory.Show();
            //if (GlobalData.DailyLockout == false)
            if (Treatment.GetRoom().alreadyTreated == false)
            {
                //enable the GiveMedicine buttons when entering the patient room if the lockout is disabled
                GiveMedicine1Button.Disabled = false;
                GiveMedicine2Button.Disabled = false;
                GiveMedicine3Button.Disabled = false;
            }
        }   
        else
        {
            Inventory.Hide();
            GiveMedicine1Button.Disabled = true;
            GiveMedicine2Button.Disabled = true;
            GiveMedicine3Button.Disabled = true;

        }
    }

    private void _on_computer_visibility_changed()
    {
        if (Computer.Visible)
        {
            Inventory.Hide();
        }
    }

    public void InventoryPatientRoom()
    {
        var GiveMedicine1Button = GetNode("Inventory").GetNode("Open_Inventory").GetNode<TextureButton>("Give_Medicine_1");
        var GiveMedicine2Button = GetNode("Inventory").GetNode("Open_Inventory").GetNode<TextureButton>("Give_Medicine_2");
        var GiveMedicine3Button = GetNode("Inventory").GetNode("Open_Inventory").GetNode<TextureButton>("Give_Medicine_3");
        if (GlobalData.inPatientRoom)
        {
            Inventory.Show();
            //if (GlobalData.DailyLockout == false)
            if (Treatment.GetRoom().alreadyTreated == false)
            {
                //enable the GiveMedicine buttons when entering the patient room if the lockout is disabled
                GiveMedicine1Button.Disabled = false;
                GiveMedicine2Button.Disabled = false;
                GiveMedicine3Button.Disabled = false;
            }
        }
        else
        {
            Inventory.Hide();
            GiveMedicine1Button.Disabled = true;
            GiveMedicine2Button.Disabled = true;
            GiveMedicine3Button.Disabled = true;

        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
