using Godot;
using System;

public partial class Main : Node
{
    Node2D Office;
    Node2D Computer;
    Node2D PatientInterface;
    Node2D PatientRoom;
    Node2D Bed;
    Node2D Pause;
    Node2D Inventory;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        GetNodes();

        //always keep the office at the bottom of the previous scenes stack, so the reference on how to return to it is always there
        GlobalData.PreviousScenes.Push(GetNode("Office").GetPath());
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
        if (Office.Visible == true)
        {
            Inventory.Show();
        } else
        {
            Inventory.Hide();
        }
    }

    private void _on_patient_interface_visibility_changed()
    {
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
        var GiveMedicine1Button = GetNode("Inventory").GetNode("Open_Inventory").GetNode<Button>("Give_Medicine_1");
        var GiveMedicine2Button = GetNode("Inventory").GetNode("Open_Inventory").GetNode<Button>("Give_Medicine_2");
        var GiveMedicine3Button = GetNode("Inventory").GetNode("Open_Inventory").GetNode<Button>("Give_Medicine_3");
        if (PatientRoom.Visible == true)
        {
            Inventory.Show();
            if (GlobalData.DailyLockout == false)
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
