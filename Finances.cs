using Godot;
using System;
using System.Collections.Generic;


public partial class Finances : Node2D
{

	[Export] RichTextLabel BreakdownLabel;
	[Export] Label FinalLabel;
	public void Initialize()
	{

	}


	public void DisplayBreakdown()
	{
        List<FinancePackage> packages = new List<FinancePackage>();
		packages = FinanceInfo.GetPackages();
		string output = "";

		int sum = 0;

		Room[] rooms = RoomManager.GetAllDeadPatients();
		int deadpatients = rooms.Length;


        output += $"{GlobalData.patientCount - deadpatients} patient(s) staying overnight, earned {GlobalData.PassiveIncome} credits.";
        output += "\n";

        foreach (FinancePackage package in packages)
		{
			output += package.GetPackageInfo();
			output += "\n";
			sum += package.GetEarnings();
        }
        BreakdownLabel.Text = output;
		sum += GlobalData.PassiveIncome;


        FinalLabel.Text = $"Total earnings: {sum} credits.";
    }
}
