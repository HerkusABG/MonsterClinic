using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class ScrollCatalog : ScrollContainer
{
    [Export] public PackedScene SlotUI;

    // loads the Malady Catalog Slot UI scene and the Folder scene
    [Export] PackedScene SlotUIMalady = ResourceLoader.Load<PackedScene>("res://Scenes/malady_catalog_slot_ui.tscn");
    [Export] PackedScene Folders = ResourceLoader.Load<PackedScene>("res://Cataloag_Malady/folder.tscn");
    // loads the Malady_Autoload script to access the list of maladies(ListMaladies)
    public Malady_Autoload MaladyData;
    // new array to store the categories of the maladies without duplicates
    public string[] maladyCategorie;
    
    public override void _Ready()
    {
        // Get the Malady_Autoload node to access the list of maladies
        MaladyData = GetNode<Malady_Autoload>("/root/MaladyAutoload");
        // get from the MaladyDate the list of maladies and select the categories, disticts filters the duplicates out
        maladyCategorie = MaladyData.ListMaladies.Select(m => m.Categorie).Distinct().ToArray();

        // Get the GridContainer node to add the folders to it
        var gridContainer = GetNode<GridContainer>("GridContainer");

        // get the length of the array of the maladyCategorie and instantiate(add) a folder for each categorie
        for(int i = 0; i < maladyCategorie.Length; i++) 
        {
            var slotfolder = Folders.Instantiate<Folder_MC>();
            gridContainer.AddChild(slotfolder);
        
        }



    }
}
