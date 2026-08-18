using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class MaladyCatalogSlotUi : Control
{
    // Called the Autoload script to extract the MaladyData, which contains the MaladyName, Description and Sympthoms
    public Malady_Autoload.MaladyData MaladyData;
    public List<Malady_Autoload.MaladyData> MaladyList;



    public void Initialize()
    {
        GetNode<Label>("Symptoms_label").Show();
        GetNode<Label>("CuredBy_label").Show();

        GetNode<RichTextLabel>("CuredBy").Show();
        GetNode<RichTextLabel>("Symptoms").Show();

        GetNode<Label>("Cost_label").Hide();
        GetNode<Label>("Behaviour_label").Hide();
    }
    
   

    public void BluePox()
    {
        /*// to get the indexchecker the maladyautoload is called, the indexchecker is set to 0, which is the first entry in the list of maladies
        var MaladyAutoload = GetNode<Malady_Autoload>("/root/MaladyAutoload");
        MaladyAutoload.indexChecker = 0;
        // the maladydata is set to the first entry in the list of maladies
        MaladyData = MaladyAutoload.ListMaladies[MaladyAutoload.indexChecker];
        // set up the name, description and sympthoms of the malady in the Labels and RichtextLabels.
        GetNode<Label>("Name").Text = MaladyData.MaladyName;
        GetNode<RichTextLabel>("Description").Text = MaladyData.Description;
        GetNode<RichTextLabel>("Sympthoms").Text = string.Join("\n", MaladyData.Sympthoms);*/
    }
    public void Injury()
    {
        /*// to get the indexchecker the maladyautoload is called, the indexchecker is set to 0, which is the first entry in the list of maladies
        var MaladyAutoload = GetNode<Malady_Autoload>("/root/MaladyAutoload");
        MaladyAutoload.indexChecker = 1;
        // the maladydata is set to the first entry in the list of maladies
        MaladyData = MaladyAutoload.ListMaladies[MaladyAutoload.indexChecker];
        // set up the name, description and sympthoms of the malady in the Labels and RichtextLabels.
        GetNode<Label>("Name").Text = MaladyData.MaladyName;
        GetNode<RichTextLabel>("Description").Text = MaladyData.Description;
        GetNode<RichTextLabel>("Sympthoms").Text = string.Join("\n", MaladyData.Sympthoms);*/
    }

    public void DisplayMaladyInfo(Malady malady)
    {
        // to get the indexchecker the maladyautoload is called, the indexchecker is set to 0, which is the first entry in the list of maladies
        // the maladydata is set to the first entry in the list of maladies
        // set up the name, description and sympthoms of the malady in the Labels and RichtextLabels.
        GetNode<Label>("Name").Text = malady.name;
        GetNode<RichTextLabel>("Description").Text = malady.description;
        List<string> cureNames = new List<string>();
        foreach (Medicine medicine in malady.cures)
        {
            cureNames.Add(medicine.name);
        }
        GetNode<RichTextLabel>("CuredBy").Text = string.Join("\n", cureNames);
        GetNode<RichTextLabel>("Symptoms").Text = string.Join("\n", malady.allSymptoms);
    }

    public void DisplayMaladyInfo(CatalogueInfoPackage package)
    {
        // to get the indexchecker the maladyautoload is called, the indexchecker is set to 0, which is the first entry in the list of maladies
        // the maladydata is set to the first entry in the list of maladies
        // set up the name, description and sympthoms of the malady in the Labels and RichtextLabels.
        GetNode<Label>("Name").Text = package.name;
        GetNode<RichTextLabel>("Description").Text = package.description;

        if (package.type == CatalogueInfoPackage.PackageType.Malady)
        {
            GetNode<Label>("Symptoms_label").Show();
            GetNode<Label>("CuredBy_label").Show();

            GetNode<RichTextLabel>("CuredBy").Show();
            GetNode<RichTextLabel>("Symptoms").Show();

            GetNode<Label>("Cost_label").Hide();
            GetNode<Label>("Behaviour_label").Hide();

            GetNode<RichTextLabel>("CuredBy").Text = string.Join("\n", package.cures);
            GetNode<RichTextLabel>("Symptoms").Text = string.Join("\n", package.symptoms);
        }
        else if (package.type == CatalogueInfoPackage.PackageType.Medicine)
        {
            GetNode<Label>("Cost_label").Show();

            GetNode<RichTextLabel>("CuredBy").Hide();
            //GetNode<RichTextLabel>("Symptoms").Hide();
            GetNode<Label>("Behaviour_label").Hide();
            GetNode<Label>("Symptoms_label").Hide();
            GetNode<Label>("CuredBy_label").Hide();

            GetNode<RichTextLabel>("Symptoms").Text = package.cost;
            //GetNode<Label>("Cost_label").Text = package.cost;
        }
        else if (package.type == CatalogueInfoPackage.PackageType.Tag)
        {
            GetNode<Label>("Behaviour_label").Show();

            GetNode<Label>("Cost_label").Hide();
            GetNode<RichTextLabel>("CuredBy").Hide();
            //GetNode<RichTextLabel>("Symptoms").Hide();
            GetNode<Label>("Symptoms_label").Hide();
            GetNode<Label>("CuredBy_label").Hide();

            GetNode<RichTextLabel>("Symptoms").Text = package.behaviour;

            //GetNode<Label>("Behaviour_label").Text = package.behaviour;
        }
        //List<string> cureNames = new List<string>();
        //GetNode<RichTextLabel>("Symptoms").Text = string.Join("\n", malady.allSymptoms);
    }
}

