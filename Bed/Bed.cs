using Godot;
using System;


public partial class Bed : Node2D
{
    private RichTextLabel DaysCounter;
    private RichTextLabel MoneyEarned;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        

        Hide();
        // getting the information for the days from the DayManager
        var day_M = GetNode<DayManager>("/root/DayManager");
        //day_M.Player_Ingame_Days++;

        //setting the RichTextLabel up, heigth the font size 130 and big, therefore BbcodeEnabled needs to be true
        var DaysCounter = GetNode<RichTextLabel>("Day");
        DaysCounter.BbcodeEnabled = true;
        DaysCounter.Text = $"[b][font_size=130] {day_M.Player_Ingame_Days} days in containment [/font_size][/b]";

        var MoneyEarned = GetNode<RichTextLabel>("MoneyEarned");
        MoneyEarned.BbcodeEnabled = true;
        MoneyEarned.Text = "Today's earnings: " + GlobalData.DailyEarnings + "\n Passive income: " + GlobalData.PassiveIncome;
        

       



        // the treatment countdown gets his information from the Global autoload. It is different than from the Daymanager, because Global has the namespace global which is gets set in the upper code (almost the first thing) and then you can call the class Variable and the integer




        // setting the RichTextLabel up (needs new name or its getting confusing), to custommize it like up there before BbcodeEnabled needs to be true. The frontSize is 110, the text is big.
        var DaysCounters = GetNode<RichTextLabel>("TreatmentDays");
        DaysCounters.BbcodeEnabled = true;

        // here is the Text chaning a little bit, with the color (pink and red) and "Animation". the Text will shake and be a little wavy. The treatment Countdown decides which text will be showned to the player
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
            DaysCounters.Text = $"[b][font_size=110][shake rate=200][wave rate=20][color=red]{GlobalData.Countdown} days left without treatment [/color][/wave][/shake][/font_size][/b]";
        }
        else
        {
            //not implemented yet
            //GetTree().ChangeSceneToFile("res://DeathScreen/death_screen.tscn");
        }





    }

    private void _on_visibility_changed()
    {
        var day_M = GetNode<DayManager>("/root/DayManager");

        var DaysCounter = GetNode<RichTextLabel>("Day");
        DaysCounter.BbcodeEnabled = true;
        DaysCounter.Text = $"[b][font_size=130] {day_M.Player_Ingame_Days} days in containment [/font_size][/b]";

        var MoneyEarned = GetNode<RichTextLabel>("MoneyEarned");
        MoneyEarned.BbcodeEnabled = true;
        MoneyEarned.Text = "Today's earnings: " + GlobalData.DailyEarnings + "\n Passive income: " + GlobalData.PassiveIncome;

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
            DaysCounters.Text = $"[b][font_size=110][shake rate=200][wave rate=20][color=red]{GlobalData.Countdown} days left without treatment [/color][/wave][/shake][/font_size][/b]";
        }
        else
        {
            //Scene changed to the death Screen, The reasion can be also set in the Global autoload, so you can change the reasion for the death screen, depending on how the player died
            GlobalData.Reasion = "Your sickness killed you! Keep an eye on your treatment countdown";
            GetTree().ChangeSceneToFile("res://DeathScreen/death_screen.tscn");
        }

        if (GlobalData.MedicinePlayer >= 1)
        {
            GlobalData.Dialog_Dealer = true;
        }

    }

    
    


}
