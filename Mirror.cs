using Godot;
using System;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;

public partial class Mirror : Button
{

    //All the necessary mirror references
    Button MirrorButton;
    Button Close;
    Label MirrorLabel;

    Label MirrorMessage;
    int severity = 0;
    string[] messages =
    {

    "Message 0",
    "Message 1",
    "Message 2",
    "Message 3",
    "Message 4",
    "Message 5",
    "Message 6",
    "Message 7",
    "Message 8",
    "Message 9"

    };
    public override void _Ready()
    {
        // Grabbing all the mirror references
        // MirrorButton reference is unecessary and should just be replaced with "this"
        MirrorButton = this;
        MirrorLabel = GetNode<Label>("MirrorLabel");
        Close = MirrorLabel.GetNode<Button>("Close");

        MirrorMessage = GetNode<Label>("MirrorLabel");

        //Hiding the mirror since we only want to see it once clicked.
        HideMirrorCloseUp();

        //Subscribing to the relevant events
        MirrorButton.Pressed += ShowMirrorCloseUp;
        Close.Pressed += HideMirrorCloseUp;

        MirrorButton.Pressed += ChangeMirrorMessage;
    }


    //Showing and hiding the mirror, pretty simple.
    private void ShowMirrorCloseUp()
    {
        MirrorLabel.Show();
    }

    private void HideMirrorCloseUp()
    {
        MirrorLabel.Hide();
    }

    private void ChangeMirrorMessage()
    {

        //Random severity 0-9
        severity = GD.RandRange(0, 9);

        //Pick random message
        int randomMessage = GD.RandRange(0, messages.Length - 1);

        //Change mirror text
        MirrorMessage.Text = messages[randomMessage];

        GD.Print("Severity: " + severity);
    }

}