using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;

public partial class Diagnosis_Box : Label
{
    //Depending on the amount of boxes in the checklist, it will add an
    //Appropriate amount of "SymptomChecks", which in this case store whether a particular
    //Checkbox value is TRUE or FALSE (Checked or unchecked)
    //Basically just imagine a list of booleans which expand according to how many checkboxes
    //there are in the patient interface.
    //IMPORTANT: Any new checkboxes that are added must be the children of the diagnosis box, otherwise it won't work!
    private List<Checkbox> CheckboxList = new List<Checkbox>();
    private List<bool> SymptomChecks = new List<bool>();
    public override void _Ready()
	{
        int count = 0;
        //Looking through all the children to find all the checkboxes.
        foreach (Node child in GetChildren())
        {
            if (child.GetClass() == "Button")
            {
                //We add all checkboxes to the list so we can reference them later.
                Button childButton = (Button)child;
                CheckboxList.Add((Checkbox)child);

                CheckboxList[count].Initialize(count + 1);

                //Make sure we check all the boxes whenever one of them will be pressed
                childButton.Pressed += CheckCheckboxes;

                //We add a symptomChecks value to that list accordingly.
                SymptomChecks.Add(false);
                count++;
            }
        }
    }
    public void SetAllCheckboxStatus(bool status)
    {
        foreach (Checkbox checkbox in CheckboxList)
        {
            checkbox.Disabled = !status;
        }
    }
    public void ClearAllBoxes()
    {
        foreach(Checkbox checkbox in CheckboxList)
        {
            checkbox.SetCheckboxStatus(false);
        }
    }

    private void CheckCheckboxes()
    {
        List<string> currentSymptoms = new List<string>();
        List<string> finalSymptoms = new List<string>();
        string possibleMaladies = "Could be: ";
        foreach(Checkbox checkbox in CheckboxList)
        {
            if(checkbox.GetCheckValue())
            {
                currentSymptoms.Add(checkbox.GetCondition());
            }
        }
        if(currentSymptoms.Count <= 0)
        {
            Text = "What could it be?";
            return;
        }
        for (int i = 0; i < currentSymptoms.Count; i++)
        {
            for (int j = 0; j < MaladyList.Database.Count; j++)
            {
                if(MaladyMatch(j, currentSymptoms))
                {
                    if (!finalSymptoms.Contains(MaladyList.Database.ElementAt(j).Value.name))
                    {
                        finalSymptoms.Add(MaladyList.Database.ElementAt(j).Value.name);
                    }
                }
            }
        }
        if (finalSymptoms.Count <= 0)
        {
            Text = "I'm not sure what this could possibly mean...";
            return;
        }
        possibleMaladies += $"{finalSymptoms[0]}";
        for (int k = 1; k < finalSymptoms.Count; k++)
        {
            possibleMaladies += " or ";
            possibleMaladies += $"{finalSymptoms[k]}";
        }
        Text = possibleMaladies;
    }

    private bool MaladyMatch(int index, List<string> symptoms)
    {
        foreach(string symptom in symptoms)
        {
            if (!MaladyList.Database.ElementAt(index).Value.allSymptoms.Contains(symptom))
            {
                return false;
            }
        }
        return true;
    }
}
