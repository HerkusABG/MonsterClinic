using Godot;
using System;
using System.Collections.Generic;
using System.Linq;


public partial class Checkbox : Button
{
    //These are children of the diagnosis box.
    //here we handle more local logic that is related to the checkboxes.
    //That way diagnosis box only keeps the info it needs and only when needed.

    //isChecked is private, use GetCheckValue to grab the info.
    private bool isChecked;
    private string condition;
    Label label;
    public void Initialize(int input)
    {
        //malady = MaladyList.Database.ElementAt(rnd.Next(0, MaladyList.Database.Count)).Value;
       // MaladyList.Database.ElementAt(0).Value = input;
        //SymptomList.Database.ElementAt(0).Value.name;
        condition = SymptomList.Database.ElementAt(input).Value.name;
        isChecked = false;
        Pressed += ButtonPressed;
        label = GetNode<Label>("CheckboxLabel");
        label.Text = condition;
    }
    private void ButtonPressed()
    {
        if(!isChecked)
        {
            Text = "x";
            isChecked = true;
        }
        else
        {
            Text = " ";
            isChecked = false;
        }
    }

    public bool GetCheckValue()
    {
        return isChecked;
    }

    public string GetCondition()
    {
        return condition;
    }

    public void SetCheckboxStatus(bool status)
    {
        
        isChecked = status;
        if(status)
        {
            Text = "x";
        }
        else
        {
            Text = " ";
        }
    }
}
