using Godot;
using System;


public partial class Bed : Node2D
{
    private RichTextLabel DaysCounter;
    private RichTextLabel MoneyEarned;

    [Export] PackedScene Transition = ResourceLoader.Load<PackedScene>("res://fade_animation.tscn");
    // Called when the node enters the scene tree for the first time.
    public void Initialize()
    {
        Hide();
        DoctorInventory.Money += GlobalData.DailyEarnings;
        GlobalData.DailyEarnings = 0;
    }

    private void _on_visibility_changed()
    {
        //GD.Print("On vibility changed in bed.cs");
        // i use a int, some how it worked better than a boolean
        if (GlobalData.ControlSpawnFading == 1)
        {
            //GD.Print("True");
            GlobalData.Fading = false;

            // get the GridContainer, so the text dont get covered from the FadeAnimation. FadeAnimation gets added to the GridContainer
            var spawn = GetNode<GridContainer>("Spawn");
            var fading = Transition.Instantiate<FadeAnimation>();
            spawn.AddChild(fading);

            // calls the Methode Fades from the FadeAnimation
            fading.Fades();

            // Control for the spawn FadeAnimation
            GlobalData.ControlSpawnFading = 2;
        }
    }

    public void FadeQuickFix()
    {
        GD.Print("Fadequickfix");
        if (GlobalData.ControlSpawnFading == 1) return;

        GlobalData.Fading = true;


        // get the GridContainer, so the text dont get covered from the FadeAnimation. FadeAnimation gets added to the GridContainer
        var spawn = GetNode<GridContainer>("Spawn");
        var fading = Transition.Instantiate<FadeAnimation>();
        spawn.AddChild(fading);

        // calls the Methode Fades from the FadeAnimation
        fading.Fades();

        // Control for the spawn FadeAnimation
        GlobalData.ControlSpawnFading = 1;
    }
}





