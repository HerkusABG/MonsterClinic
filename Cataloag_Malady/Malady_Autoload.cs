using Godot;
using System.Collections.Generic;
public partial class Malady_Autoload : Node
{
    // This List can hold all MaladyData
    public List<MaladyData> ListMaladies = new List<MaladyData>();

    // the indexChecker is used to go through the list of Maladies, so is spawns the different Maladies
    public int indexChecker = 0;


    public override void _Ready()
    {

        // to create a new Maladie, The ListMaladies needs to be added and a new MaladyData needs to be created. All the varables should be defined there.
        ListMaladies.Add(new MaladyData
        {
            MaladyName = "Maladie 1",
            Sympthoms = new string[] { "Sympthom 1", "Sympthom 2", "Sympthom 3" },
            Description = "Description of the Maladie 1"

        });
        ListMaladies.Add(new MaladyData
        {
            MaladyName = "Maladie 2",
            Sympthoms = new string[] { "Sympthom 4", "Sympthom 5", "Sympthom 6" },
            Description = "Description of the Maladie 2"

        });
        ListMaladies.Add(new MaladyData
        {
            MaladyName = "Maladie 3",
            Sympthoms = new string[] { "Sympthom 7", "Sympthom 8", "Sympthom 9" },
            Description = "Description of the Maladie 3"

        });
        ListMaladies.Add(new MaladyData
        {
            MaladyName = "Maladie 4",
            Sympthoms = new string[] { "Sympthom 10", "Sympthom 11", "Sympthom 12" },
            Description = "Description of the Maladie 4"

        });
        ListMaladies.Add(new MaladyData
        {
            MaladyName = "Maladie 5",
            Sympthoms = new string[] { "Sympthom 13", "Sympthom 14", "Sympthom 15" },
            Description = "Description of the Maladie 5"

        });
        ListMaladies.Add(new MaladyData
        {
            MaladyName = "Maladie 6",
            Sympthoms = new string[] { "Sympthom 16", "Sympthom 17", "Sympthom 18" },
            Description = "Description of the Maladie 6"

        });
        ListMaladies.Add(new MaladyData
        {
            MaladyName = "Maladie 7",
            Sympthoms = new string[] { "Sympthom 19", "Sympthom 20", "Sympthom 21" },
            Description = "Description of the Maladie 7"

        });
        
    }



    public class MaladyData
    {
        //here are the variables to create the Maladies, consisting of name, description and sympthoms. The Sympthoms are an Array so there can be placed multiple strings in it.
        public string MaladyName;
        public string[] Sympthoms;
        public string Description;
    }


}
