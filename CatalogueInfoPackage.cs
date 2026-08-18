using Godot;
using System;
using System.Collections.Generic;
public class CatalogueInfoPackage
{
	public string name;
    public string description;

    public string cost;
    public string behaviour;


    public List<string> symptoms = new List<string>();
    public List<string> cures = new List<string>();

    public PackageType type;



    /*public CatalogueInfoPackage<T>(Dictionary<string, T> dictionary)
	{

	}*/

    public CatalogueInfoPackage(Malady malady)
	{
        type = PackageType.Malady;
        name = malady.name;
		description = malady.description;
        symptoms = malady.allSymptoms;
        foreach (Medicine medicine in malady.cures)
        {
            this.cures.Add(medicine.name);
        }
    }

    public CatalogueInfoPackage(Tag tag)
    {
        type = PackageType.Tag;
        name = tag.name;
        description = tag.description;
        behaviour = tag.behaviour;
    }
    public CatalogueInfoPackage(Medicine medicine)
    {
        type = PackageType.Medicine;
        name = medicine.name;
        description = medicine.description;
        cost = medicine.cost.ToString();
    }
    public enum PackageType
    {
        Malady,
        Medicine,
        Tag
    }
}
