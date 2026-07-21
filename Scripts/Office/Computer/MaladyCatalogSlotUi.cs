using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class MaladyCatalogSlotUi : Control
{
    // Called the Autoload script to extract the MaladyData, which contains the MaladyName, Description and Sympthoms
    public Malady_Autoload.MaladyData MaladyData;
    public List<Malady_Autoload.MaladyData> MaladyList;



    public override void _Ready()
    {
    
    }
    
   

    public void BluePox()
    {
        // to get the indexchecker the maladyautoload is called, the indexchecker is set to 0, which is the first entry in the list of maladies
        var MaladyAutoload = GetNode<Malady_Autoload>("/root/MaladyAutoload");
        MaladyAutoload.indexChecker = 0;
        // the maladydata is set to the first entry in the list of maladies
        MaladyData = MaladyAutoload.ListMaladies[MaladyAutoload.indexChecker];
        // set up the name, description and sympthoms of the malady in the Labels and RichtextLabels.
        GetNode<Label>("Name").Text = MaladyData.MaladyName;
        GetNode<RichTextLabel>("Description").Text = MaladyData.Description;
        GetNode<RichTextLabel>("Sympthoms").Text = string.Join("\n", MaladyData.Sympthoms);
    }
    public void Injury()
    {
        // to get the indexchecker the maladyautoload is called, the indexchecker is set to 0, which is the first entry in the list of maladies
        var MaladyAutoload = GetNode<Malady_Autoload>("/root/MaladyAutoload");
        MaladyAutoload.indexChecker = 1;
        // the maladydata is set to the first entry in the list of maladies
        MaladyData = MaladyAutoload.ListMaladies[MaladyAutoload.indexChecker];
        // set up the name, description and sympthoms of the malady in the Labels and RichtextLabels.
        GetNode<Label>("Name").Text = MaladyData.MaladyName;
        GetNode<RichTextLabel>("Description").Text = MaladyData.Description;
        GetNode<RichTextLabel>("Sympthoms").Text = string.Join("\n", MaladyData.Sympthoms);
    }

  
}

