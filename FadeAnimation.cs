using Godot;
using System;
using System.Runtime.InteropServices.JavaScript;


public partial class FadeAnimation : Node2D
{
    private Tween tw_fade;

    [Export] RichTextLabel Day;
    [Export] RichTextLabel TreatmentDays;
    [Export] RichTextLabel MoneyEarnedDay;
    public override void _Ready()
    {
        GetNodes();
        HideText();
    }

    private void GetNodes()
    {
        Day = GetNode<RichTextLabel>("Day");
        TreatmentDays = GetNode<RichTextLabel>("TreatmentDays");
        MoneyEarnedDay = GetNode<RichTextLabel>("MoneyEarned");
    }

    public void Fades()
    {
        SetUpText();
        // creates a Tween
        tw_fade = GetTree().CreateTween().SetParallel();

        // get Timer
        var deleteselfTimer = GetNode<Timer>("Delete_Timer");
        deleteselfTimer.OneShot = true;



        // condition for the animation
        if (GlobalData.Fading == false)
        {
            // get the ColorRect and set the new Color invisible
            var Colorrect_visibility = GetNode<ColorRect>("Fade");
            Colorrect_visibility.Color = new Color(Colorrect_visibility.Color.R, Colorrect_visibility.Color.G, Colorrect_visibility.Color.B, 0);


            // Tween affects the color rect, 1f -> from invisible to visible, 1f -> animation speed
            tw_fade.TweenProperty(Colorrect_visibility, "color:a", 1f, 1f);

            if(GlobalData.Bed == true)
            {
                FadeText();
                ShowText();
            }
            else
            {
                HideText();
            }

            // smooth animation for the Tween
            tw_fade.SetTrans(Tween.TransitionType.Sine);
            tw_fade.SetEase(Tween.EaseType.Out);

            
            // Condition changes 
            GlobalData.Fading = true;


            // Timer get set to 3 sec, so long is the bed scene. Timer starts
            //deleteselfTimer.SetWaitTime(3.0);
            deleteselfTimer.Start(3.0);

        }
        else
        {
            // get the ColorRect and set the new Color invisible
            var Colorrect_visibility = GetNode<ColorRect>("Fade");
            Colorrect_visibility.Color = new Color(Colorrect_visibility.Color.R, Colorrect_visibility.Color.G, Colorrect_visibility.Color.B, 1);

            
            // Tween affects the color rect, 0f -> from invisible to visible, 1f -> animation speed
            tw_fade.TweenProperty(Colorrect_visibility, "color:a", 0f, 0.4f);

            if (GlobalData.Bed == true)
            {
                FadeText();
            }
            else
            {
                HideText();
            }

            // smooth animation for the Tween
            tw_fade.SetTrans(Tween.TransitionType.Sine);
            tw_fade.SetEase(Tween.EaseType.Out);

            

            // Timer get set to 1 sec, so long is that the player isnt stuck. Timer starts
           //deleteselfTimer.SetWaitTime(1.0);
            deleteselfTimer.Start(0.4f);

        }
        // Timer gets connected to the function, when the timer is done, the function gets called
        deleteselfTimer.Timeout += _on_delete_timer_timeout;
        

    }

    public void FadeText()
    {
        if (GlobalData.Fading == false)
        {
            tw_fade = GetTree().CreateTween().SetParallel();
            Day.SelfModulate = new Color(Day.SelfModulate.R, Day.SelfModulate.G, Day.SelfModulate.B, 0);
            TreatmentDays.SelfModulate = new Color(TreatmentDays.SelfModulate.R, TreatmentDays.SelfModulate.G, TreatmentDays.SelfModulate.B, 0);
            MoneyEarnedDay.SelfModulate = new Color(MoneyEarnedDay.SelfModulate.R, MoneyEarnedDay.SelfModulate.G, MoneyEarnedDay.SelfModulate.B, 0);

            tw_fade.TweenProperty(Day, "self_modulate:a", 1f, 1f);
            tw_fade.TweenProperty(TreatmentDays, "self_modulate:a", 1f, 1f);
            tw_fade.TweenProperty(MoneyEarnedDay, "self_modulate:a", 1f, 1f);
        }
        /*else
        {
            Day.SelfModulate = new Color(Day.SelfModulate.R, Day.SelfModulate.G, Day.SelfModulate.B, 1);
            TreatmentDays.SelfModulate = new Color(TreatmentDays.SelfModulate.R, TreatmentDays.SelfModulate.G, TreatmentDays.SelfModulate.B, 1);
            MoneyEarnedDay.SelfModulate = new Color(MoneyEarnedDay.SelfModulate.R, MoneyEarnedDay.SelfModulate.G, MoneyEarnedDay.SelfModulate.B, 1);
            
            tw_fade.TweenProperty(Day, "self_modulate:a", 0f, 1f);
            tw_fade.TweenProperty(TreatmentDays, "self_modulate:a", 0f, 1f);
            tw_fade.TweenProperty(MoneyEarnedDay, "self_modulate:a", 0f, 1f);
        }*/

    }

    private void _on_delete_timer_timeout()
    {
        // delets itself
        QueueFree();
    }

    private void HideText()
    {
        Day.Hide();
        TreatmentDays.Hide();
        MoneyEarnedDay.Hide();
    }

    private void ShowText()
    {
        Day.Show();
        TreatmentDays.Show();
        MoneyEarnedDay.Show();
    }


    private void SetUpText()
    {
        var day_M = GetNode<DayManager>("/root/DayManager");

        Day.BbcodeEnabled = true;
        Day.Text = $"[b][font_size=130] {day_M.Player_Ingame_Days} days in containment [/font_size][/b]";

        var MoneyEarned = GetNode<RichTextLabel>("MoneyEarned");
        MoneyEarned.BbcodeEnabled = true;
        MoneyEarned.Text = "Today's earnings: " + GlobalData.DailyEarnings;

        var DaysCounters = GetNode<RichTextLabel>("TreatmentDays");
        DaysCounters.BbcodeEnabled = true;

        if (GlobalData.Countdown >= 3)
        {
            DaysCounters.Text = $"[b][font_size=110]{GlobalData.Countdown} days left without treatment[/font_size][/b]";
        }
        else if (GlobalData.Countdown >= 1 && GlobalData.Countdown < 3)
        {
            DaysCounters.Text = $"[b][font_size=110][shake rate=50][color=DEEP_PINK]{GlobalData.Countdown} days left without treatment [/color][/shake][/font_size][/b]";
        }

        else if (GlobalData.Countdown == 0)
        {
            DaysCounters.Text = $"[b][font_size=110][shake rate=50][color=DEEP_PINK]{GlobalData.Countdown} days left without treatment [/color][/shake][/font_size][/b]";

        }
        // idk why but the Countdown is weird, it only works if its -1 or -2 for the death
        else if (GlobalData.MedicinePlayer >= 1 && GlobalData.Countdown == -1)
        {
            DaysCounters.Text = $"[b][font_size=110][shake rate=200][wave rate=20][color=green] Treatment is comming [/color][/wave][/shake][/font_size][/b]";
        }
        else if (GlobalData.MedicinePlayer == 0 && GlobalData.Countdown == -2 && GlobalData.Dialog_Dealer == false)
        {
            //Scene changed to the death Screen, The reasion can be also set in the Global autoload, so you can change the reasion for the death screen, depending on how the player died
            GlobalData.Reasion = "Your sickness killed you! Keep an eye on your treatment countdown";
            GetTree().ChangeSceneToFile("res://DeathScreen/death_screen.tscn");
        }
        else if (GlobalData.Countdown == -1 && GlobalData.MedicinePlayer == 0)
        {
            DaysCounters.Text = $"[b][font_size=110][shake rate=200][wave rate=20][color=red] Death is waiting [/color][/wave][/shake][/font_size][/b]";
        }


        if (GlobalData.MedicinePlayer >= 1)
        {
            //GlobalData.Dialog_Dealer = true;
            //GlobalData.MedicinePlayer--;
        }
        else
        {
            GlobalData.Dialog_Dealer = false;
        }

    }

}
