using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;

public partial class Diagnosis_Box : Label
{
    private List<Checkbox> CheckboxList = new List<Checkbox>();
    private List<bool> SymptomChecks = new List<bool>();
    public override void _Ready()
	{
        SymptomChecks.Add(false);
        SymptomChecks.Add(false);
        SymptomChecks.Add(false);
        SymptomChecks.Add(false);

        foreach (Node child in GetChildren())
        {
            if (child.GetClass() == "Button")
            {
                Button childButton = (Button)child;
                childButton.Pressed += CheckCheckboxes;
                CheckboxList.Add((Checkbox)child);
            }
        }
    }

    private void CheckCheckboxes()
    {
        for(int i = 0; i < CheckboxList.Count; i++)
        {
            SymptomChecks[i] = CheckboxList[i].GetCheckValue();
        }

        if (SymptomChecks[0] == true && SymptomChecks[1] == false && SymptomChecks[2] == false && SymptomChecks[3] == false)
        {
            Text = "It could be this, \n mhm but it also could be some other things \n what other symptoms are there?";
        }
        else if (SymptomChecks[1] == true && SymptomChecks[0] == true && SymptomChecks[2] == false && SymptomChecks[3] == false)
        {
            Text = "this narrows it down to x and y \n is that all?";
        }
        else if (SymptomChecks[2] == true && SymptomChecks[1] == true && SymptomChecks[0] == true && SymptomChecks[3] == false)
        {
            Text = "this narrows it down to y \n is that all?";
        }
        else
        {
            Text = "What do these Symptoms tell us??";
        }
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
        

    }
}
