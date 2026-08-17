using Godot;
using System;

public class CatalogueInfoPackage
{
	public string name;
    public string description;

	

	public CatalogueInfoPackage(Malady malady)
	{
		name = malady.name;
		description = malady.description;
	}
}
