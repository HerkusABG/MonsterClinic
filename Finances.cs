using Godot;
using System;
using System.Collections.Generic;


public partial class Finances : Node2D
{

	[Export] RichTextLabel BreakdownLabel;
	public void Initialize()
	{

	}


	public void DisplayBreakdown()
	{
        List<FinancePackage> packages = new List<FinancePackage>();
		packages = FinanceInfo.GetPackages();
		string output = "";

		foreach (FinancePackage package in packages)
		{
			GD.Print("LOOP");
			output += package.GetPackageInfo();
			output += "\n";
        }
        BreakdownLabel.Text = output;
    }
}
