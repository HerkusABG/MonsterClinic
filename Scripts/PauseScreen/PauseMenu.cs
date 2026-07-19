using Godot;
using System;

public partial class PauseMenu : Node2D
{
	[Export] PackedScene option = ResourceLoader.Load<PackedScene>("res://Scenes/option_menu.tscn");

    //Added the cheat stuff in accordance with our post-overhaul approach, will probably overhaul the whole scene eventually too
    //Storing a reference to all the buttons, labels, etc., for easy reference in the methods
    Button CheatsButton;
	ColorRect CheatsBackground;
	Label MoneyDisplay;
	Button MoneyAdder;
	Button MoneySubtractor;
	Button CloseCheats;
	bool escBlock = false;
	bool escBlockSetup = false;
    
    public void Initialize()
	{
		SaveSystem.LoadFile_Settings();
        //grabs references to all the necessary nodes
        GetNodes();
		// hide the Control on startup
        var get = GetNode<Control>("Spawn_Options");
		get.Hide();

		Subscribe();
    }

	private void GetNodes()
	{
        //Basically just grabs all the nodes
        CheatsButton = GetNode("Player_Interactables_Menu").GetNode<Button>("Cheats");
		CheatsBackground = GetNode<ColorRect>("Cheats_Background");
        MoneyDisplay = CheatsBackground.GetNode<Label>("Money_Display");
		MoneyAdder = CheatsBackground.GetNode<Button>("Money_Adder");
		MoneySubtractor = CheatsBackground.GetNode<Button>("Money_Subtractor");
		CloseCheats = CheatsBackground.GetNode<Button>("Close");
    }

	private void Subscribe()
	{
        CheatsButton.Pressed += ShowCheats;
        MoneyAdder.Pressed += AddMoney;
        MoneySubtractor.Pressed += SubtractMoney;
        CloseCheats.Pressed += () => CloseParent(CloseCheats);
    }

	public override void _UnhandledInput(InputEvent @event)
	{
		//grabbing the reference to the vbox containing the 3 main menu buttons
        var settings = GetNode<Control>("Spawn_Options");
		if (@event is InputEventKey eventKey)
		{
			//if a key is pressed and that key is esc
			if (eventKey.Pressed && eventKey.Keycode == Key.Escape)
			{
				//hide cheat menu if it's visible
				if (CheatsBackground.Visible == true)
				{
					CheatsBackground.Hide();
				}
				//ottherwise toggle the pause menu
				else
				{
					Visible = !Visible;
				}
			}

		}
	}

	//when resume pressed, hide the pause menu
	private void _on_resume_pressed()
	{
		Hide();
	}

	//when settings pressed, hide the main section of the pause menu, and show the settings from the option menu
	private void _on_settings_pressed()
	{
		escBlock = true;

        var menu = (VBoxContainer)GetNode("Player_Interactables_Menu").GetNode("Menu_Box");
        

		// Control node gets shown and on this spawns the option menu with the settings
		var get = GetNode<Control>("Spawn_Options");
		get.Show();

		// Spawns the option menu on the Control node
        var optionMenu = option.Instantiate();
        get.AddChild(optionMenu);
		
        // Connects the Signalname from the Option menu to the new callable function OptionMenuClose
        optionMenu.Connect(OptionMenu.SignalName.OptionMenuClose, new Callable(this, nameof(OptionMenuClose)));

    }

	private void _on_save_game_pressed()
	{
        SaveSystem.Save_Days();
    }
	private void ShowCheats()
	{
		CheatsBackground.Show();
	}

    private void CloseParent(Button button)
    {
        var Parent = button.GetParent();
        if (Parent.GetClass() == "Label")
        {
            Label ParentLabel = (Label)Parent;
            ParentLabel.Hide();
        }
        if (Parent.GetClass() == "Control")
        {
            var ControlParent = (Control)Parent;
            ControlParent.Hide();
        }
        if (Parent.GetClass() == "ColorRect")
        {
            var ControlParent = (ColorRect)Parent;
            ControlParent.Hide();
        }
    }
	//add money, update display
    private void AddMoney()
    {
        DoctorInventory.Money += 10;
        MoneyDisplay.Text = DoctorInventory.Money.ToString();
    }
    //subtract money, update display
    private void SubtractMoney()
    {
        DoctorInventory.Money -= 10;
        MoneyDisplay.Text = DoctorInventory.Money.ToString();
    }
    //update display whenever the display's visibility changes
    private void _on_money_display_visibility_changed()
    {
		if (MoneyDisplay.Visible == true)
		{
			MoneyDisplay.Text = DoctorInventory.Money.ToString();
		}
    }

    //when exit pressed, quit the game
    private void _on_exit_pressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/main_menu.tscn");
	}

	public void OptionMenuClose(Boolean op_Close)
	{
		// if the op_close is true, then Spawn_options is hide
		if(op_Close == true)
		{

			// The Control gets hidden again
            var get = GetNode<Control>("Spawn_Options");
            get.Hide();
            escBlockSetup = true;
        }
		
    }

}
