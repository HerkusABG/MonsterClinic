using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

public partial class Folder_MC : Control
{
    Button Malady_Button;


    public Malady_Autoload.MaladyData MaladyData;
	//[Export] private Control MaladyCatalogSlotUi;
	Texture2D folderopen = (Texture2D)ResourceLoader.Load("res://Cataloag_Malady/Folder_Open.png");
    Texture2D folderclose = (Texture2D)ResourceLoader.Load("res://Cataloag_Malady/Folder_Idle.png");
    public Boolean doubleclick = false;
    public List<Malady_Autoload.MaladyData> MaladyList;

    //
    //[Export] PackedScene Maladydescription = ResourceLoader.Load<PackedScene>("res://Cataloag_Malady/malady_catalog_slot_ui.tscn");
   
    // set the Button name for the buttons so it is easier for the MaladyCatalogSlotUi to later find the correct information
    public string setbuttonname = "";

    // create a varable to hold the MaladyCatalogSlotUi
    public MaladyCatalogSlotUi MCSU;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{

        var MaladyAutoload = GetNode<Malady_Autoload>("/root/MaladyAutoload");
        
        // get the MaladyData from the MaladyAutoload using the indexChecker to set the tag and text for the button
        MaladyData = MaladyAutoload.ListMaladies[MaladyAutoload.indexChecker];
        GetNode<Label>("Tag").Text = MaladyData.Categorie;
        GetNode<Button>("FolderButton/VBoxContainer/Malady_Button").Text = MaladyData.MaladyName;
       
        // set the button name from the MaladyData and the index gets higher to get the next data
        if (MaladyAutoload.indexChecker == 0)
        {
            setbuttonname = MaladyData.MaladyName;
        }
        MaladyAutoload.indexChecker++;

        // get VBoxContainer from the scene and hides the container which contains the buttons
        var container_Button = GetNode<VBoxContainer>("FolderButton/VBoxContainer");
        container_Button.Hide();
        // get the Malady_Button from the scene
        Malady_Button = GetNode<Button>("FolderButton/VBoxContainer/Malady_Button");
        // connects every button in the VBoxContainer to the _on_malady_button_pressed function, when the button is pressed it will call the function and pass the button as a parameter
        foreach (var child in container_Button.GetChildren())
        {
            // checks if the child is a button, if it connected to the _on_malady_button_pressed funtion the button as a parameter gets transfered to the function
            if (child is Button btn)
            {
                btn.Pressed += () => _on_malady_button_pressed(btn);
            }
        }

    }


    public void _on_folder_button_pressed()
    {
        // get FolderSprite and VBoxContainer from the scene
        var FolderSprite = GetNode<Sprite2D>("FolderIdle");
        var container_Button = GetNode<VBoxContainer>("FolderButton/VBoxContainer");

        // function is for double clicking the folder to open and close, if false the folder opens, if its true the folder closes
        if (doubleclick == false)
        {
            // set the folder sprite to the open folder texture, sets doubleclick to true, and shows the VboxContainer (which contains the buttons)
            FolderSprite.Texture = folderopen;
            doubleclick = true;
            container_Button.Show();
        }
        else
        {
            // set the folder sprite to the closed folder texture, sets doubleclick to false, and hides the VBoxContainer (which contains the buttons)
            FolderSprite.Texture = folderclose;
            doubleclick = false;
            container_Button.Hide();
        }
        
    }

    public void _on_malady_button_pressed(Button btn)
	{
        // looking for the MaladyCatalogSlotUi in the scene tree (Computer) and assigning it to the MCSU variable, because the folder scene doesnt have the MCSU
        MCSU = GetTree().Root.FindChild("MaladyCatalogSlotUi", true, false) as MaladyCatalogSlotUi;

        // get the button name from the button
        string getButtonName = btn.Text;
        
        // check the button name and call the appropriate function in MCSU(MaladyCatalogSlotUi)
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
