using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;

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
        //Looking through all the children to find all the checkboxes.
        foreach (Node child in GetChildren())
        {
            if (child.GetClass() == "Button")
            {
                //We add all checkboxes to the list so we can reference them later.
                Button childButton = (Button)child;
                CheckboxList.Add((Checkbox)child);

                //Make sure we check all the boxes whenever one of them will be pressed
                childButton.Pressed += CheckCheckboxes;

                //We add a symptomChecks value to that list accordingly.
                SymptomChecks.Add(false);
            }
        }
    }

    private void CheckCheckboxes()
    {
        //Whenever any of the checkboxes are pressed we check whether we should update the text
        //in the diagnosis box accordingly. For this we also use the SymptomChecks, which tell us
        //whether each individual checkbox is ticked or not.
        for(int i = 0; i < CheckboxList.Count; i++)
        {
            SymptomChecks[i] = CheckboxList[i].GetCheckValue();
        }
        //After checking we simply update the text!

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
}
