using Godot;
using System;
using System.Collections.Generic;

public partial class MaladyCatalogSlotUi : Control
{
    // Called the Autoload script to extract the MaladyData, which contains the MaladyName, Description and Sympthoms
    public Malady_Autoload.MaladyData MaladyData;
    public List<Malady_Autoload.MaladyData> MaladyList;



    public override void _Ready()
    {
        // to get the indexChecker i needed to get the path from the Autoload, to change the ListMaladies, when they spawn
        var MaladyAutoload = GetNode<Malady_Autoload>("/root/MaladyAutoload");
        if (MaladyData != null)
        {
             MaladyData = MaladyAutoload.ListMaladies[MaladyAutoload.indexChecker];
             MaladyList = MaladyAutoload.ListMaladies;
            // set up the name, description and sympthoms of the malady in the Labels and RichtextLabels.
            GetNode<Label>("Name").Text = MaladyData.MaladyName;
            GetNode<RichTextLabel>("Description").Text = MaladyData.Description;
            GetNode<RichTextLabel>("Sympthoms").Text = string.Join("\n", MaladyData.Sympthoms);

            //if the current entry is the final one in the list, reset the index checker, so it doesn't go beyond the list size when it runs after new game or load game
            if (MaladyAutoload.indexChecker == MaladyList.Count - 1)
            {
                MaladyAutoload.indexChecker = -1;
            }
        }
    }
    


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}

