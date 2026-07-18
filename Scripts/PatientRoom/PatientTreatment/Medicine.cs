using Godot;
using System;
using System.Xml.Linq;

public class Medicine
{
    //This is a class that is used for defining what sort of information
    //Each individual type of medicine should include. It is subject to change
    //based on the information given by the GD team.


    //Name of the medicine
	public string name { get; set; }
    //Cost of the medicine.
	public int cost { get; set; } = 10;
    //Amount of medicine bought
    public int amount { get; set; } = 0;

    public bool buyable = true;

    public bool unlocked = true;

    public Medicine Clone()
    {
        return new Medicine
        {
            name = name,
            cost = cost,
            amount = amount
        };
    }
}
