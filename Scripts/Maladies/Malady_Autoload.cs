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
            MaladyName = "Blue Pox",
            Sympthoms = new string[] { "Sneezing", "Headache", "Fever" },
            Description = "A highly infectious virus that presents with cold-like symptoms and purplish spots in areas on the patient´s skin. Some Doctors noticed that Aspirin, Antibiotics, fany Antibiotics and God medicine has affects on the patient, on how bad or good these are isnt noted",
            Categorie = "virus"
        });
        ListMaladies.Add(new MaladyData
        {
            MaladyName = "Injury",
            Sympthoms = new string[] { "headache", "heartrate", "body pain" },
            Description = "A physical injury that leaves the patient weak while they recover from the damage. To your knowledge you know that Bandages would definitly help the patient, but the dealer has also in his shop fancy bandages and the god medicine as an option for dealing with them. There is the risk of further damaging them, do you take it?",
            Categorie = "injury"
        });
        ListMaladies.Add(new MaladyData
        {
            MaladyName = "Green Pox",
            Sympthoms = new string[] { "Sneezing", "Headache", "Fever" },
            Description = "A cousin strain of the Blue Pox, well known for ravaging communities in [REDACTED], luckily most cases can be treated with Antibiotics.",
            Categorie = "virus"
        });
        ListMaladies.Add(new MaladyData
        {
            MaladyName = "Sthyricoids",
            Sympthoms = new string[] { "Vertigo", "Paralysis", "Heart Problems" },
            Description = "A condition caught by people who have breathed in low-quality air for an extended period of time, making bloodflow to the limbs and brain difficult. Can be treated with Aptomitol and Morphine.",
            Categorie = "injury"
        });
        ListMaladies.Add(new MaladyData
        {
            MaladyName = "Soldier's gut",
            Sympthoms = new string[] { "Vertigo", "Vomitting"},
            Description = "Named after the soldier who were among the first to be afflicted by it, soldier's gut refers to an airborne virus which shuts down the digestive system in mere hours. Antibiotics usually help, but the virus is known to be unpredictable.",
            Categorie = "virus"
        });
        ListMaladies.Add(new MaladyData
        {
            MaladyName = "Slithic",
            Sympthoms = new string[] { "Vertigo", "Headache", "Fever" },
            Description = "A rare and incredibly deadly virus which can do irreperable damage to a person's body in mere days. Usually treated with Aptomitol and Phranax",
            Categorie = "virus"
        });

    }



    public class MaladyData
    {
        //here are the variables to create the Maladies, consisting of name, description and sympthoms. The Sympthoms are an Array so there can be placed multiple strings in it.
        public string MaladyName;
        public string[] Sympthoms;
        public string Description;
        public string Categorie;
    }


}
