using Godot;
using System;

public partial class MaladyManager : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//all of our Maladies into an array so that we may assign them to patients deliberately 

		MaladyList MaladyListInstance = new MaladyList();

		Malady[] AllMaladies = { MaladyListInstance.Headache, MaladyListInstance.Parasite, MaladyListInstance.SkinWarts };

        //give the newly admitted patient a random malady at a random severity
        Random rnd = new Random();
        GlobalData.PatientMalady = AllMaladies [rnd.Next(0, 2)];

        GlobalData.PatientMalady.severity = rnd.Next(2, 5);

		// the Control code, if everything runs smoothly this should show in the COmpiler 
		GD.Print(GlobalData.PatientMalady.name);
		GD.Print(GlobalData.PatientMalady.severity.ToString());
    }
}
