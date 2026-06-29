using Godot;
using System;
using System.Collections.Generic;

public partial class Main : Node
{
    Contents_O Office;
    Contents_C Computer;
    Contents_P_I PatientInterface;
    //Node2D PatientRoom;
    Bed Bed;
    PauseMenu PauseMenu;
    Inventory Inventory;
    Hallway Hallway;
    TreatmentManager Treatment;
    [Export] Control RoomControl;
    int finalRoomCount = 6;

    //Since Main is the the main node, everything should be initialized here.
    //Methods shouldn't use their individual "_Ready()" methods, unless
    //it is absolutely necessary. This is to insure a very clear flow of actions.
    //Otherwise a _Ready method of one class can be executed before its parent and cause some issues.
	public override void _Ready()
	{
        Initialize();
	}

    private void GetNodes()
    {
        Office = GetNode<Contents_O>("Office");
        Computer = GetNode<Contents_C>("Computer");
        PatientInterface = GetNode<Contents_P_I>("Patient_Interface");
        //PatientRoom = GetNode<Node2D>("Room");
        Bed = GetNode<Bed>("Bed");
        PauseMenu = GetNode<PauseMenu>("Pause");
        Inventory = GetNode<Inventory>("Inventory");
        Hallway = GetNode<Hallway>("Hallway");
        Treatment = Inventory.GetNode<TreatmentManager>("Treatment_Manager");
    }
    private void Initialize()
    {
        GD.Print("Initialize Main");
        GetNodes();
        //always keep the office at the bottom of the previous scenes stack, so the reference on how to return to it is always there
        GlobalData.PreviousScenes.Push(GetNode("Office").GetPath());
        InitializeChildren();
        GeneratePatientRooms(RoomControl);
    }

    private void InitializeChildren()
    {
        RoomManager.Initialize();
        Office.Initialize();
        Computer.Initialize();
        PatientInterface.Initialize();
        Hallway.Initialize();
        Bed.Initialize();
        PauseMenu.Initialize();
        Inventory.Initialize();
    }

    private void GeneratePatientRooms(Control roomControl)
    {
        var patientRoom = (Node2D)GetNode("RoomTemplate");
        Random random = new Random();
        for (int i = 0; i < finalRoomCount; i++)
        {
            Node2D newRoom = (Node2D)patientRoom.Duplicate();
            newRoom.Hide();
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
            if (eventKey.Pressed && eventKey.ButtonIndex == MouseButton.Right && PauseMenu.Visible == false && Office.Visible == false)
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
}
