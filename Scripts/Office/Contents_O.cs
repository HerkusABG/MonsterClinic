using Godot;
using System;

public partial class Contents_O : ExpNode2D
{
    private Timer sceneTimer;
    private Timer financesTimer;
    [Export] PackedScene dealer_selftreatment_dialog = ResourceLoader.Load<PackedScene>("res://Scenes/dialog.tscn");
    [Export] PackedScene Transition = ResourceLoader.Load<PackedScene>("res://fade_animation.tscn");
    // Called when the node enters the scene tree for the first time.
    public void Initialize()
	{
        GetNodes();
        // Timer from the scene
        sceneTimer.OneShot = true;

        // connect the signals
        sceneTimer.Timeout += OnSceneTimerTimeout;

        Subscribe();

        InitializeChildren();
    }

    private void Subscribe()
    {

    }

    private void InitializeChildren()
    {
        Control control = GetNode<Control>("Player_Interactables_O");
        Mirror mirror = control.GetNode<Mirror>("Mirror");
        mirror.Initialize();
    }
    private void GetNodes()
    {
        sceneTimer = GetNode<Timer>("ChangeToBed_Timer");
        financesTimer = GetNode<Timer>("Finances_Timer");
    }

    private void _on_computer_a_pressed()
	{
        GlobalData.Bed = false;
        RoomTracker.EnterRoom(ActiveRoom.Computer);
    }
    private void _on_patient_i_a_pressed()
    {
        GlobalData.Bed = false;
        RoomTracker.EnterRoom(ActiveRoom.Admission);
		var DialogScene = (Control)GetParent().GetNode("Dialog");
        DialogScene.Hide();
    }

	private void _on_elevator_pressed()
	{
        GlobalData.Bed = false;
        RoomTracker.EnterRoom(ActiveRoom.Hallway);
		var DialogScene = (Control)GetParent().GetNode("Dialog");
        DialogScene.Hide();

    }
    private void _on_bed_pressed()
    {
        Hide();
        var day_M = GetNode<DayManager>("/root/DayManager");
        day_M.Player_Ingame_Days++;
        GlobalData.Player_Ingame_Days++;
        //make the money from treating patients, and the passive income
        GlobalData.PassiveIncome = GlobalData.patientCount * 20;
        DoctorInventory.Money += GlobalData.DailyEarnings + GlobalData.PassiveIncome;
        GlobalData.Countdown--;

        GlobalData.ControlSpawnFading = 1;

        var BedScene = (Node2D)GetParent().GetNode("Bed");
        BedScene.Show();

        //unlock the ability to enable the GiveMedicine buttons by entering the patient room
        //GlobalData.DailyLockout = false;
        RoomManager.NewDay();

        Hallway hallway = GetParent().GetNode<Hallway>("Hallway");
        hallway.ResetRoomUI();

        Contents_P_I patientInterface = GetParent().GetNode<Contents_P_I>("Patient_Interface");
        patientInterface.NewDay();

        Inventory inventory = GetParent().GetNode<Inventory>("Inventory");
        inventory.InventoryActions();

        //GlobalData.inPatientAdmission = false;
        RoomTracker.RoomTrack(ActiveRoom.Office);

        if (GlobalData.Countdown >= 0)
        {
            //push the scene we're entering to the previous scenes stack
            GlobalData.PreviousScenes.Push(BedScene.GetPath());

            // timer is getting set to 3 seconds and starts
            sceneTimer.Start(3.0);
            if (GlobalData.Medicincavailability != 0)
            {
                GlobalData.Medicincavailability--;
            }
            //DialogDealer();
        }
    }

    private void OnSceneTimerTimeout()
    {
        // get node bed scene
        var BedScene = (Node2D)GetParent().GetNode("Bed");

        // switches scene
        BedScene.Hide();
        var FinancesScene = (Node2D)GetParent().GetNode("Finances");
        FinancesScene.Show();
        Show();
        //push the scene we're entering to the previous scenes stack
        GlobalData.PreviousScenes.Pop();
        DialogDealer();
        financesTimer.Start(3.0);
        financesTimer.OneShot = true;
        financesTimer.Timeout += on_finances_timer_timeout;






        /* // switching scenes
         GlobalData.DailyEarnings = 0;
         var BedScene = (Node2D)GetParent().GetNode("Bed");
         BedScene.Hide();
         Show();
         if(GlobalData.ControlSpawnFading == 2)
         {

             Bed bed = GetParent().GetNode<Bed>("Bed");
             bed.FadeQuickFix();
             /*var spawn = GetNode<GridContainer>("Spawn");
             var fading = Transition.Instantiate<FadeAnimation>();
             spawn.AddChild(fading);
             fading.Fades();
          }
        //push the scene we're entering to the previous scenes stack
        GlobalData.PreviousScenes.Pop();
        */

    }

    private void on_finances_timer_timeout()
    {
        Dialog dialog = GetParent().GetNode<Dialog>("Dialog");
        //dialog.Show();
        GD.Print("Dialog");


        // Daily earnings gets reseted
        GlobalData.DailyEarnings = 0;

        // get node bed scene
        var BedScene = (Node2D)GetParent().GetNode("Bed");

        // condition for the Controled Spawn
        if (GlobalData.ControlSpawnFading == 2)
        {
            GlobalData.Bed = true;
            // Condition Changes
            GlobalData.Fading = true;
            TriggerFading();
        }
        if (GlobalData.Dialog_Dealer == true)
        {
            var DialogForDealer = (Control)GetParent().GetNode("Dialog");
            DialogForDealer.Show();
            Dialog.currentIndex = 0;

        }
        var FinancesScene = (Node2D)GetParent().GetNode("Finances");
        FinancesScene.Hide();
        financesTimer.Timeout -= on_finances_timer_timeout;

    }
    private void TriggerFading()
    {
        // instantiate the scene FadeAnimation
        var fading = Transition.Instantiate<FadeAnimation>();
        // add the scene FadeAnimation and call the Methode Fades
        AddChild(fading);
        fading.Fades();
    }


    private void DialogDealer()
    {
        // Dialog Dealer checks if the dialog should spawn again and the dealer control is so that the code isnt spammened in the process
        if (GlobalData.Dialog_Dealer == true && GlobalData.Dialog_Dealer_Control == true)
        {
            var DialogScene = (Control)GetParent().GetNode("Dialog");
            DialogScene.Show();

            // the dialog for the dealer is set to the 0, because he is the first one in the two dimensional array
            Dialog.currentNPC = 0;
            // Dealer Control checks if the dialog should spawn again
            GlobalData.Dialog_Dealer_Control = false;
            // The medicine need to decrease for the player
            GlobalData.MedicinePlayer--;
        }
    }

    public override void OnRoomEnter(Node mainNode)
    {
        //GD.Print("Entering office");
        TriggerFading();
        Inventory inv = mainNode.GetNode<Inventory>("Inventory");
        inv.InventoryActions();
    }

    public override void OnRoomExit()
    {
        //GD.Print("Exiting office");
    }
}
