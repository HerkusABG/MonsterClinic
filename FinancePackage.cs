using Godot;
using System;

public class FinancePackage
{
    int amount;
    string malady;
    int patients;
    int individualCost;

    public FinancePackage(int inputAmount, string inputMalady, int inputPatients)
    {
        amount = inputAmount;
        individualCost = amount;
        malady = inputMalady;
        patients = inputPatients;
    }

    public string GetPackageInfo()
    {
        string output = "";
        output = $"Cured {malady} {patients} time(s), earned {amount} credits.";
        return output;
    }

    public int GetEarnings()
    {
        return amount;
    }

    public bool IsMyMalady(string inputMalady)
    {
        if(inputMalady == malady)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SameMalady()
    {
        amount += individualCost;
        patients++;
    }
}
