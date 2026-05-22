using Godot;
using System;
//using global;

public partial class Deadline_Countdown : Label
{

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Hide();
    }

    private void _on_treatment_d_pressed()
    {
        Show();
        if (GlobalData.Countdown > 1)
        {
            Text = String.Format("If you don't get treatment in {0} days, \n you'll be in real trouble", GlobalData.Countdown);
        }
        else if (GlobalData.Countdown == 1)
        {
            Text = "If you don't get treatment within the next day, \n you'll be in real trouble";
        }
        else
        {
            Text = "Time's up! If this wasn't a prototype, \n you'd already be gone...";
        }
    }

    private void _on_close_pressed()
    {
        Hide();
    }

    private void _on_time_passed()
    {
        if (GlobalData.Countdown > 0)
        {
            GlobalData.Countdown--;
        }
    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}