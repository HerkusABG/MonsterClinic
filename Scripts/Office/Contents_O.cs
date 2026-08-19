using Godot;
using System;

public partial class Contents_O : ExpNode2D
{
    private Timer sceneTimer;
    private Timer financesTimer;
    [Export] PackedScene dealer_selftreatment_dialog = ResourceLoader.Load<PackedScene>("res://Scenes/dialog.tscn");
    private Label PopupLabel;
    private Button PopupOpen;
    private Button PopupClose;
    [Export] PackedScene Transition = ResourceLoader.Load<PackedScene>("res://fade_animation.tscn");
    [Export] TextureButton BedButton;
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
        BedButton.Pressed += Popup_Show;
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
        PopupLabel = GetNode<Label>("PopupLabel");
        PopupOpen = GetNode<Button>("PopupLabel/PopupOpen");
        PopupClose = GetNode<Button>("PopupLabel/PopupClose");
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

    }
    private void _on_bed_pressed()
    {
        //Popup_Show();
    }

    private void _on_popup_open_pressed()
    {
        Hide();
        var day_M = GetNode<DayManager>("/root/DayManager");
        day_M.Player_Ingame_Days++;
        GlobalData.Player_Ingame_Days++;
        //make the money from treating patients, and the passive income
        //GlobalData.PassiveIncome = GlobalData.patientCount * 20;
        GlobalData.PassiveIncome = RoomManager.GetAllPassivePayouts();

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
        inventory.Hide();


        //GlobalData.inPatientAdmission = false;
        RoomTracker.RoomTrack(ActiveRoom.Office);

        if (GlobalData.Countdown >= 0)
        {
            //push the scene we're entering to the previous scenes stack
            GlobalData.PreviousScenes.Push(BedScene.GetPath());


            // timer is getting set to 3 seconds and starts
            sceneTimer.Start(3.0);
            GlobalData.sleepState = SleepState.Days;
            if (GlobalData.Medicincavailability != 0)
            {
                GlobalData.Medicincavailability--;
            }
        }
        Popup_Hide();
    }

    private void _on_popup_close_pressed()
    {
        Popup_Hide();
    }

    private void Popup_Show()
    {
        GD.Print("popup");
        PopupLabel.Show();
        PopupOpen.Show();
        PopupClose.Show();
    }

    private void Popup_Hide()
    {
        PopupLabel.Hide();
        PopupOpen.Hide();
        PopupClose.Hide();
    }

    public override void _Input(InputEvent inputEvent)
    {
        if (inputEvent is InputEventMouseButton leftmouseBtn)
        {
            if (leftmouseBtn.ButtonIndex == MouseButton.Left && leftmouseBtn.Pressed && GlobalData.Bed == true && GlobalData.Countdown != -1)
            {
                if(GlobalData.sleepState == SleepState.Days)
                {
                    sceneTimer.Stop();
                    var BedScene = (Node2D)GetParent().GetNode("Bed");
                    BedScene.Hide();
                    //Show();
                    OnSceneTimerTimeout();
                }
                else if (GlobalData.sleepState == SleepState.Money)
                {
                    financesTimer.Stop();
                    var FinanceScene = (Node2D)GetParent().GetNode("Finances");
                    FinanceScene.Hide();
                    //Show();
                    on_finances_timer_timeout();
                }
            }
        }
    }

    private void OnSceneTimerTimeout()
    {
        GlobalData.sleepState = SleepState.Money;
        // get node bed scene
        var BedScene = (Node2D)GetParent().GetNode("Bed");

        // Daily earnings gets reseted
        GlobalData.DailyEarnings = 0;

        // condition for the Controled Spawn
        if (GlobalData.ControlSpawnFading == 2)
        {
            GlobalData.Bed = true;
            // Condition Changes
            GlobalData.Fading = true;
            GlobalData.Dialog_Dealer = false;
            TriggerFading();
        }

        if (GlobalData.MedicinePlayer == 0 && GlobalData.Countdown == -1)
        {
            
        }

        // switches scene
        BedScene.Hide();
        var FinancesScene = (Node2D)GetParent().GetNode("Finances");
        Finances finances = FinancesScene as Finances;
        finances.DisplayBreakdown();
        FinancesScene.Show();
        FinanceInfo.ClearPackages();
        Show();
        //push the scene we're entering to the previous scenes stack
        GlobalData.PreviousScenes.Pop();
        //DialogDealer(); <----- COMMENTED THIS OUT HERKUS
        financesTimer.Start(5.0);
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
        if (financesTimer.IsConnected("timeout", Callable.From(on_finances_timer_timeout)))
        {
            financesTimer.Timeout -= on_finances_timer_timeout;
        }
        GlobalData.sleepState = SleepState.None;

        Inventory inventory = GetParent().GetNode<Inventory>("Inventory");
        inventory.Show();
    }
    private void TriggerFading()
    {
        // instantiate the scene FadeAnimation
        var fading = Transition.Instantiate<FadeAnimation>();
        // add the scene FadeAnimation and call the Methode Fades
        AddChild(fading);
        fading.Fades();
    }



    public override void OnRoomEnter(Node mainNode)
    {
        //GD.Print("Entering office");
        TriggerFading();

        Inventory inv = mainNode.GetNode<Inventory>("Inventory");
        inv.InventoryActions();
        inv.Show();
    }

    public override void OnRoomExit()
    {
        //GD.Print("Exiting office");
    }
}
