using Godot;
using System;

public partial class MainMenu : Control
{
    // Called when the node enters the scene tree for the first time.
    [Export] PackedScene option = ResourceLoader.Load<PackedScene>("res://Scenes/option_menu.tscn");
    [Signal] public delegate void DeleteSaveSystemEventHandler(bool deleteSafe);

    public override void _Ready()
    {
        var ColorRecthide = GetNode<ColorRect>("ColorRect");
        ColorRecthide.Hide();
        SaveSystem.LoadFile_Settings();

        // Player indicator for not having any save files
        var LockColor = GetNode<ColorRect>("Lock_Color");
        if (FileAccess.FileExists("user://Days.Json") || SaveManager.SaveFileExists())
        {
            LockColor.Hide();
        }
        else
        {
            LockColor.Show();
        }

        // Safely check ContinueButton if it exists in scene
        if (HasNode("ContinueButton"))
        {
            GetNode<Button>("ContinueButton").Disabled = !SaveManager.SaveFileExists();
        }

        // Configure Load / Continue Game button
        if (HasNode("Load_Game_Button"))
        {
            var loadBtn = GetNode<Button>("Load_Game_Button");
            loadBtn.Disabled = !SaveManager.SaveFileExists() && !FileAccess.FileExists("user://Days.Json");
            loadBtn.Pressed += OnContinuePressed;
        }

        // Wire New Game and Delete buttons
        if (HasNode("New_Game_Button"))
        {
            GetNode<Button>("New_Game_Button").Pressed += OnNewGamePressed;
        }

        if (HasNode("Delete_Save"))
        {
            GetNode<Button>("Delete_Save").Pressed += _on_delete_save_pressed;
        }
    }

    private void _on_new_game_button_pressed()
    {
        OnNewGamePressed();
    }

    private void _on_load_game_button_button_down()
    {
        if (FileAccess.FileExists("user://Days.Json"))
        {
            // loads the days and treatment countdown for the player
            SaveSystem.Load_Days();
            GetTree().ChangeSceneToFile("res://Scenes/Main.tscn");
        }
    }

    private void _on_options_button_pressed()
    {
        // spawns the option menu
        var optionMenu = option.Instantiate();
        AddChild(optionMenu);
    }

    private void _on_credits_button_pressed()
    {
        var ColorRecthide = GetNode<ColorRect>("ColorRect");
        ColorRecthide.Show();
        var TextRTL = GetNode<RichTextLabel>("ColorRect/RichTextLabel");
        TextRTL.Text = "Credits arent currently available, try again in the full version :)";
    }

    private void _on_delete_save_pressed()
    {
        var LockColor = GetNode<ColorRect>("Lock_Color");
        LockColor.Show();

        // checks if File exists
        if (FileAccess.FileExists("user://Days.Json"))
        {
            // deletes the save json
            SaveSystem.Delete_Days();
        }

        // Delete SaveManager file
        SaveManager.DeleteSave();

        // Update button status
        if (HasNode("Load_Game_Button"))
        {
            GetNode<Button>("Load_Game_Button").Disabled = true;
        }
        if (HasNode("ContinueButton"))
        {
            GetNode<Button>("ContinueButton").Disabled = true;
        }
    }

    private void _on_exit_button_pressed()
    {
        // closes the game
        GetTree().Quit();
    }

    private void _on_close_pressed()
    {
        var ColorRecthide = GetNode<ColorRect>("ColorRect");
        ColorRecthide.Hide();
    }

    private void OnContinuePressed()
    {
        if (SaveManager.SaveFileExists())
        {
            SaveManager.LoadGame();
            GetTree().ChangeSceneToFile("res://Scenes/Office.tscn");
        }
    }

    private void OnNewGamePressed()
{
    SceneTree tree = GetTree();
    if (tree == null) return;

    tree.Paused = false;

    // Delete saved data files
    SaveManager.DeleteSave();

    // Reset currency and starting statistics
    DoctorInventory.Money = 100;
    GlobalData.Player_Ingame_Days = 1;
    GlobalData.Countdown = 4;
    GlobalData.DailyEarnings = 0;
    GlobalData.Dialog_Dealer = false;

    // Clear medicine stock
    if (MedicineManager.Database != null)
    {
        foreach (var medicine in MedicineManager.Database.Values)
        {
            if (medicine != null) medicine.amount = 0;
        }
    }

    // Change scene safely
    tree.ChangeSceneToFile("res://Scenes/Main.tscn");
}
}