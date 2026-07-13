using Godot;
using System;
using System.Collections.Generic;

public partial class Main : Node
{
    Contents_O Office;
    Contents_C Computer;
    Contents_P_I PatientInterface;
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
        //The initial initialize gets called here.
        //Every other node then gets initialized from the chain starts right here.
        //Every single class with an initialize method should be able to track back
        //to these lines right here.
        Initialize();
	}

    private void GetNodes()
    {
        //Grabbing all the relevant nodes in the "main" scene of the game.
        Office = GetNode<Contents_O>("Office");
        Computer = GetNode<Contents_C>("Computer");
        PatientInterface = GetNode<Contents_P_I>("Patient_Interface");
        Bed = GetNode<Bed>("Bed");
        PauseMenu = GetNode<PauseMenu>("Pause");
        Inventory = GetNode<Inventory>("Inventory");
        Hallway = GetNode<Hallway>("Hallway");
        Treatment = Inventory.GetNode<TreatmentManager>("Treatment_Manager");
    }
    private void Initialize()
    {
        GetNodes();
        //always keep the office at the bottom of the previous scenes stack, so the reference on how to return to it is always there
        GlobalData.PreviousScenes.Push(GetNode("Office").GetPath());
        //Initialization chain [BELOW]
        InitializeChildren();
        GeneratePatientRooms(RoomControl);
    }

    private void InitializeChildren()
    {
        //This is where the children get initialized, the next step within the chain.
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
        //Generating the patient room scenes.
        //Grabbing the existing template to create the other rooms
        var patientRoom = (Node2D)GetNode("RoomTemplate");
        Random random = new Random();
        //Creating as many rooms as finalRoomCount specifies. Subject to change in the future.
        for (int i = 0; i < finalRoomCount; i++)
        {
            Node2D newRoom = (Node2D)patientRoom.Duplicate();
            newRoom.Hide();
            roomControl.AddChild(newRoom);
            RoomManager.RoomList.Add(newRoom);
            Room room = newRoom as Room;
            room.Initialize(Treatment.HideUI);
        }
    }
    public override void _UnhandledInput(InputEvent @event)
    {
        //This is the function that makes it so that right clicking takes you back to a previous room.
        if (@event is InputEventMouseButton eventKey)
        {
            //if a key is pressed and that key is the right mouse button, and if the pause menu and the office aren't visible
            if (eventKey.Pressed && eventKey.ButtonIndex == MouseButton.Right && PauseMenu.Visible == false && Office.Visible == false)
            {
                //pop a scene from the previous scenes stack, this is the scene currently in use
                var current_scene = (Node2D)GetNode(GlobalData.PreviousScenes.Pop().ToString());
                //hide it
                current_scene.Hide();
                GD.Print("exiting " + current_scene.Name);
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
                GD.Print("entering " + parent.Name);
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
                //Reset the PreviousScenes stack (except for the office) every time we go back to the office
                GlobalData.PreviousScenes.Clear();
                GlobalData.PreviousScenes.Push(GetNode("Office").GetPath());
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
            //enable the GiveMedicine buttons when entering the patient room if the lockout is disabled
            GiveMedicine1Button.Disabled = false;
            GiveMedicine2Button.Disabled = false;
            GiveMedicine3Button.Disabled = false;
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
}
