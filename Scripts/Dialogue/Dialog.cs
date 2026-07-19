using Godot;
using System;
using System.Security.Cryptography.X509Certificates;
using static System.Net.Mime.MediaTypeNames;

public partial class Dialog : Control
{
	private Tween tw_dialog;
	private float doubles = 0f;

    // look out this is a multidimensional array, so you can have multiple dialogues for different characters. They are sometimes really difficult to understand
    public static string[][] dialogues;
	// the index for the multidimensional array, so the dialog goes to different lines
	public static int currentIndex = 0;
	[Export] private Button _button;

	public override void _Ready()
	{
        var dialog = GetNode<RichTextLabel>("DialogText");
		dialog.VisibleRatio = 0;

        currentIndex = 0;
		//one dialog text is initiated for the dealer dialog. This dialog can be used provally for the patients too, but need than modifications
		dialogues = new string[1][];

		// making the medicine availability random for the player, so he cant spam the self treatment
		var randomavalibility = new Random();
		GlobalData.Medicincavailability = randomavalibility.Next(2, 5);


		// the dialog for the dealer has the index 0, for other dialogs use 0 + 1, so the second dialog has the index 1 and so on.
		// for an out put or input for the array use the second index, so dialogues[0][0] is the first line of the dealer dialog, dialogues[0][1] is the second line and so on. For the second dialog you need to put dialogues[1][0] and so on.
		dialogues[0] = new string[]
		{
			"...",
			"I have some medicine for you",
			"you get a new delievery of medicine in " + GlobalData.Medicincavailability + " days, but the price is higher than before.",
			"We see us next time <3"
		};



		_on_button_pressed();


	}

	private void _on_button_pressed()
	{
        var dialog = GetNode<RichTextLabel>("DialogText");
		var Name_label = GetNode<Label>("ColorRect/Name_Label");
        if (tw_dialog != null && tw_dialog.IsRunning())
        {
            tw_dialog.Kill();
			
        }


        if (currentIndex < dialogues[0].Length)
		{
			// RichTextLabel is used to display the dialog, for that BbcodeEnabled needs to be true
			dialog.BbcodeEnabled = true;

			// Label declares which persons speaks
			Name_label.Text = "Dealer";

            

			// This is the RichTextLabel Visible Ratio, means the text visible of the RichTextLabel text. Zero is nothing is visiable and 1 is all are visible
			dialog.VisibleRatio = 0;
			// duration is the speed of the animation
			float duration = 2f;
			// this is for the visible ratio for the RichTextLabel
            doubles = 1f;
			// creates a Tween
			tw_dialog = GetTree().CreateTween();
			// Tween gets set up, the visible ratio get set up from 0 to 1, the animation speed is the last thing you add there
			tw_dialog.TweenProperty(dialog, "visible_ratio", dialog.VisibleRatio + doubles, duration);
			// both applies smooth and organic animation curve and consistancy.
			tw_dialog.SetTrans(Tween.TransitionType.Sine);
			tw_dialog.SetEase(Tween.EaseType.Out);

			// dialog text (from the array) gets set to the RichtextLabel
            dialog.Text = string.Join("\n", dialogues[0][currentIndex]);
			// Index needs to be increased, so the dialog gets further
			currentIndex++;
		
            
			


        }   
        else
		{
			// when the dialog is finished, the medicine availability is added to the countdown, so it is more balanced for the player
			GlobalData.Countdown = GlobalData.Countdown + GlobalData.Medicincavailability;
			// the dialog is set to false, so it can only be spawned once
			GlobalData.Dialog_Dealer = false;
			// the control for the dealer dialog, so it isnt spammed
			GlobalData.Dialog_Dealer_Control = true;
			// the dialog self destructs itself
			QueueFree();
		}

        
    }



}

