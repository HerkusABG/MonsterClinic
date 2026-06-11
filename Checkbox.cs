using Godot;
using System;

public partial class Checkbox : Button
{
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
