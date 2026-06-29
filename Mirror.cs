using Godot;
using System;

public partial class Mirror : TextureButton
{

    //All the necessary mirror references
    TextureButton MirrorButton;
    Button Close;
    Label MirrorLabel;
    Button Deadline;
    Label DeadlineLabel;

    TextureRect MirrorImage;

    Texture2D[] mirrorImages =
    {
    // These are the place holder images stored in the godot files for the office
    GD.Load<Texture2D>("res://Assets/2D Art/Office/MirrorImage/stickman1.png"),
    GD.Load<Texture2D>("res://Assets/2D Art/Office/MirrorImage/stickman2.png"),
    GD.Load<Texture2D>("res://Assets/2D Art/Office/MirrorImage/stickman3.png"),
    GD.Load<Texture2D>("res://Assets/2D Art/Office/MirrorImage/stickman4.png")
    };
    public void Initialize()
    {
        // Grabbing all the mirror references
        // MirrorButton reference is unecessary and should just be replaced with "this"
        MirrorButton = this;
        MirrorLabel = GetNode<Label>("MirrorLabel");
        Close = MirrorLabel.GetNode<Button>("Close");

        MirrorImage = MirrorLabel.GetNode<TextureRect>("MirrorImage");


       

        //Subscribing to the relevant events
        MirrorButton.Pressed += ShowMirrorCloseUp;
        Close.Pressed += HideMirrorCloseUp;
        MirrorButton.Pressed += ChangeMirrorImage;

        DeadlineLabel = GetNode<Label>("DeadlineLabel");

        // Hiding the deadline window. It shouldn't be visible until clicked on.
        HideDeadline();

        // Subscribing to the relevant events.
        Close.Pressed += HideDeadline;

        //Hiding the mirror since we only want to see it once clicked.
        HideMirrorCloseUp();
    }


    //Showing and hiding the mirror, pretty simple.
    private void ShowMirrorCloseUp()
    {
        MirrorLabel.Show();
        ShowDeadline();
    }

    private void HideMirrorCloseUp()
    {
        MirrorLabel.Hide();
        HideDeadline();
    }

    // Sorting out the images so that they change color according to the days left
    private void ChangeMirrorImage()
    {
        if (GlobalData.Countdown >= 4)
        {
            // Green (4 days)
            MirrorImage.Texture = mirrorImages[3];
        }

        else if (GlobalData.Countdown == 3)
        {
            // Yellow (3 days)
            MirrorImage.Texture = mirrorImages[2];
        }

        else if (GlobalData.Countdown == 2)
        {
            // orange (2 days)
            MirrorImage.Texture = mirrorImages[1];
        }

        else if (GlobalData.Countdown <= 1)
        {
            // Red (1 day)
            MirrorImage.Texture = mirrorImages[0];
        }

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
