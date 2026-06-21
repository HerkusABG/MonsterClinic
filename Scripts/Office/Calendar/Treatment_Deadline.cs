using Godot;
using System;

public partial class Treatment_Deadline : Button
{

    // Three node references used for referencing all the elements within the deadline window.
    Button Deadline;
	Button Close;
	Label DeadlineLabel;
    public override void _Ready()
	{
        // This script is attached to the button itself, so the button stores itself basically.
        // This reference is unecessary, I didn't think about it when writing.
        Deadline = this;
        DeadlineLabel = GetNode<Label>("DeadlineLabel");
        Close = DeadlineLabel.GetNode<Button>("Close");

        // Hiding the deadline window. It shouldn't be visible until clicked on.
        HideDeadline();

        // Subscribing to the relevant events.
        Deadline.Pressed += ShowDeadline;
        Close.Pressed += HideDeadline;
    }
    
    private void ShowDeadline()
    {
        //Updating the label with the relevant info.
        DeadlineLabel.Show();
        if (GlobalData.Countdown > 1)
        {
            DeadlineLabel.Text = String.Format("If you don't get treatment in {0} days, \n you'll be in real trouble", GlobalData.Countdown);
        }
        else if (GlobalData.Countdown == 1)
        {
            DeadlineLabel.Text = "If you don't get treatment within the next day, \n you'll be in real trouble";
        }
        else
        {
            DeadlineLabel.Text = "Time's up! If this wasn't a prototype, \n you'd already be gone...";
        }
    }

    private void HideDeadline()
	{
        // Hiding the window, which in this case is a label.
		DeadlineLabel.Hide();
    }
}
