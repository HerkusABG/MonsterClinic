using Godot;
using System;
using System.Threading.Tasks;

public partial class Contents_O : Node2D
{
    private Timer sceneTimer;
    [Export] PackedScene dealer_selftreatment_dialog = ResourceLoader.Load<PackedScene>("res://dialog.tscn");
    [Export] PackedScene Transition = ResourceLoader.Load<PackedScene>("res://fade_animation.tscn");
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{

    }

    private void _on_computer_a_pressed()
	{

		Hide();
		var ComputerScene = (Node2D)GetParent().GetNode("Computer");
		ComputerScene.Show();
        //push the scene we're entering to the previous scenes stack
        GlobalData.PreviousScenes.Push(ComputerScene.GetPath());
	}
    private void _on_patient_i_a_pressed()
    {

        Hide();
        var PatientScene = (Node2D)GetParent().GetNode("Patient_Interface");
        PatientScene.Show();
        //push the scene we're entering to the previous scenes stack
        GlobalData.PreviousScenes.Push(PatientScene.GetPath());
    }

	private void _on_elevator_pressed()
	{
		Hide();
        var RoomScene = (Node2D)GetParent().GetNode("Room");
        RoomScene.Show();
        //push the scene we're entering to the previous scenes stack
        GlobalData.PreviousScenes.Push(RoomScene.GetPath());
    }
    private void _on_bed_pressed()
    {
        GlobalData.ControlSpawnFading = 1;
        Hide();
        
        var day_M = GetNode<DayManager>("/root/DayManager");
        day_M.Player_Ingame_Days++;
        DoctorInventory.Money += GlobalData.DailyEarnings;
        var BedScene = (Node2D)GetParent().GetNode("Bed");
        BedScene.Show();
        GlobalData.Fading = false;
        //push the scene we're entering to the previous scenes stack
        GlobalData.PreviousScenes.Push(BedScene.GetPath());

        // Timer from the scene
        var sceneTimer = GetNode<Timer>("ChangeToBed_Timer");
        sceneTimer.OneShot = true;

        // connect the signals
        sceneTimer.Timeout += OnSceneTimerTimeout;

        // timer is getting set to 3 seconds and starts
        sceneTimer.Start(3.0);
        if(GlobalData.Medicincavailability != 0)
        {
            GlobalData.Medicincavailability--;
        }

        
    }
    
    private void OnSceneTimerTimeout()
    {
        // instantiate the scene FadeAnimation
        var fading = Transition.Instantiate<FadeAnimation>();

        // Daily earnings gets reseted
        GlobalData.DailyEarnings = 0;

        // get node bed scene
        var BedScene = (Node2D)GetParent().GetNode("Bed");

        

        // condition for the Controled Spawn
        if (GlobalData.ControlSpawnFading == 2)
        {
            // Condition Changes
            GlobalData.Fading = true;
            // add the scene FadeAnimation and call the Methode Fades
            AddChild(fading);
            fading.Fades();
            

        }

        if (GlobalData.Dialog_Dealer == true)
        {
            var DialogForDealer = (Control)GetParent().GetNode("Dialog");
            DialogForDealer.Show();
            Dialog.currentIndex = 0;
            
        }
        

        // switches scene
        BedScene.Hide();
        Show();
        //push the scene we're entering to the previous scenes stack
        GlobalData.PreviousScenes.Pop();
 
    }
    public override void _Process(double delta)
    {
        if (GlobalData.Dialog_Dealer == false)
        {
            var DialogForDealer = (Control)GetParent().GetNode("Dialog");
            DialogForDealer.Hide();
            
        }
    }

    }
