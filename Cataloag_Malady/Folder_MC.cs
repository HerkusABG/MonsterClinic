using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

public partial class Folder_MC : Control
{
    Button Malady_Button;

    [Export] Button folderButton;
    [Export] Control Dropdown;
    public Malady_Autoload.MaladyData MaladyData;
	//[Export] private Control MaladyCatalogSlotUi;
	Texture2D folderopen = (Texture2D)ResourceLoader.Load("res://Cataloag_Malady/folder1.png");
    Texture2D folderclose = (Texture2D)ResourceLoader.Load("res://Cataloag_Malady/folder2.png");
    public Boolean doubleclick = false;
    //public List<Malady_Autoload.MaladyData> MaladyList;

    //
    //[Export] PackedScene Maladydescription = ResourceLoader.Load<PackedScene>("res://Cataloag_Malady/malady_catalog_slot_ui.tscn");
   
    // set the Button name for the buttons so it is easier for the MaladyCatalogSlotUi to later find the correct information
    public string setbuttonname = "";

    // create a varable to hold the MaladyCatalogSlotUi
    public MaladyCatalogSlotUi MCSU;

    // Called when the node enters the scene tree for the first time.
    public void Initialize(MaladyCategory category, Action action)
	{
        //folderButton.Pressed += action;
        folderButton.Pressed += FolderAction;
        //var MaladyAutoload = GetNode<Malady_Autoload>("/root/MaladyAutoload");

        // get the MaladyData from the MaladyAutoload using the indexChecker to set the tag and text for the button
        var container_Button = GetNode<VBoxContainer>("FolderButton/Dropdown/Panel/VBoxContainer");
        MCSU = GetTree().Root.FindChild("MaladyCatalogSlotUi", true, false) as MaladyCatalogSlotUi;
        GetNode<Label>("Tag").Text = category.PluralName;

        List<Malady> SortedMaladies = new List<Malady>();
        SortedMaladies = MaladyList.GetAllMaladiesOfType(category);
        int index = 0;
        foreach (Button button in container_Button.GetChildren())
        {
            if(index < SortedMaladies.Count)
            {
                Malady malady = SortedMaladies[index];
                button.Text = SortedMaladies[index].name;
                button.Pressed += () => MCSU.DisplayMaladyInfo(malady);
                GD.Print($"Button z is {button.ZIndex}");
                index++;
            }
            else
            {
                button.Hide();
            }
        }
        foreach (var child in container_Button.GetChildren())
        {
            // checks if the child is a button, if it connected to the _on_malady_button_pressed funtion the button as a parameter gets transfered to the function
            if (child is Button btn)
            {
                //btn.Pressed += () => _on_malady_button_pressed(btn);
            }
        }

    }


    public void _on_folder_button_pressed()
    {
        //FolderAction();
    }

    private void FolderAction()
    {
        // get FolderSprite and VBoxContainer from the scene
        var FolderSprite = GetNode<Sprite2D>("FolderIdle");
        //var container_Button = GetNode<VBoxContainer>("FolderButton/VBoxContainer");

        // function is for double clicking the folder to open and close, if false the folder opens, if its true the folder closes
        if (doubleclick == false)
        {
            // set the folder sprite to the open folder texture, sets doubleclick to true, and shows the VboxContainer (which contains the buttons)
            FolderSprite.Texture = folderopen;
            doubleclick = true;
            //container_Button.Show();
            Dropdown.Show();
        }
        else
        {
            // set the folder sprite to the closed folder texture, sets doubleclick to false, and hides the VBoxContainer (which contains the buttons)
            FolderSprite.Texture = folderclose;
            doubleclick = false;
            //container_Button.Hide();
            Dropdown.Hide();
        }
    }

    public void CloseFolder()
    {
        var FolderSprite = GetNode<Sprite2D>("FolderIdle");
        //var container_Button = GetNode<VBoxContainer>("FolderButton/VBoxContainer");

        FolderSprite.Texture = folderclose;
        doubleclick = false;
        //container_Button.Hide();
        Dropdown.Hide();
    }
    public void _on_malady_button_pressed(Button btn)
	{
        // looking for the MaladyCatalogSlotUi in the scene tree (Computer) and assigning it to the MCSU variable, because the folder scene doesnt have the MCSU
        MCSU = GetTree().Root.FindChild("MaladyCatalogSlotUi", true, false) as MaladyCatalogSlotUi;

        // get the button name from the button
        string getButtonName = btn.Text;
        
        // check the button name and call the appropriate function in MCSU(MaladyCatalogSlotUi)
        if (getButtonName == "Blue Pox")
        {
            MCSU.BluePox();

        }
        else if (getButtonName == "Accident")
        {
            MCSU.Injury();
        }
    }


	
}
