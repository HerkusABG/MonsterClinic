using Godot;
using System;

public partial class CatalogueButton : Button
{
	Malady malady;
	public void Initialize(Malady malady)
	{
		this.malady = malady;
    }
}
