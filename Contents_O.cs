using Godot;
using System;

public partial class Contents_O : Node2D
{

    private Timer sceneTimer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	private void _on_computer_a_pressed()
	{

		Hide();
		var ComputerScene = (Node2D)GetParent().GetNode("Computer");
		ComputerScene.Show();
	}
    private void _on_patient_i_a_pressed()
    {

        Hide();
        var PatientScene = (Node2D)GetParent().GetNode("Patient_Interface");
        PatientScene.Show();
    }

	private void _on_elevator_pressed()
	{
		Hide();
        var RoomScene = (Node2D)GetParent().GetNode("Room");
        RoomScene.Show();
    }
    private void _on_bed_pressed()
    {
        Hide();
        var day_M = GetNode<DayManager>("/root/DayManager");
        day_M.Player_Ingame_Days++;
        GlobalData.Money += GlobalData.DailyEarnings;
        GlobalData.Countdown--;
        var BedScene = (Node2D)GetParent().GetNode("Bed");
        BedScene.Show();

        // Timer from the scene
        var sceneTimer = GetNode<Timer>("ChangeToBed_Timer");
        sceneTimer.OneShot = true;

        // connect the signals
        sceneTimer.Timeout += OnSceneTimerTimeout;

        // timer is getting set to 3 seconds and starts
        sceneTimer.Start(3.0);
    }

    private void OnSceneTimerTimeout()
    {
        // switching scenes
        GlobalData.DailyEarnings = 0;
        var BedScene = (Node2D)GetParent().GetNode("Bed");
        BedScene.Hide();
        Show();
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
