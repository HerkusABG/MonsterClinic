using Godot;
using System;
using System.Collections.Generic;

public static class FinanceInfo
{
    public static List<FinancePackage> packages = new List<FinancePackage>();

    public static void Initialize()
    {
        ClearPackages();
    }
	public static void SaveFinanceInfo(int inputAmount, string inputMalady, int inputPatients)
	{
        foreach(FinancePackage loopPackage in packages)
        {
            if(loopPackage.IsMyMalady(inputMalady))
            {
                loopPackage.SameMalady();
            }
        }
        FinancePackage package = new FinancePackage(inputAmount, inputMalady, inputPatients);
        packages.Add(package);
    }

    public static List<FinancePackage> GetPackages()
    {
        return packages;
    }

    public static void ClearPackages()
    {
        packages.Clear();
    }
}