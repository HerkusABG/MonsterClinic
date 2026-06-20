using Godot;
using System;

public partial class Checkbox : Button
{
    //These are children of the diagnosis box.
    //here we handle more local logic that is related to the checkboxes.
    //That way diagnosis box only keeps the info it needs and only when needed.

    //isChecked is private, use GetCheckValue to grab the info.
    private bool isChecked;
	public override void _Ready()
	{
        isChecked = false;
        Pressed += ButtonPressed;
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
}
