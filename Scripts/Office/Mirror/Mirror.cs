using Godot;
using System;

public partial class Mirror : Button
{

    //All the necessary mirror references
    Button MirrorButton;
    Button Close;
    Label MirrorLabel;
    public override void _Ready()
    {
        // Grabbing all the mirror references
        // MirrorButton reference is unecessary and should just be replaced with "this"
        MirrorButton = this;
        MirrorLabel = GetNode<Label>("MirrorLabel");
        Close = MirrorLabel.GetNode<Button>("Close");

        //Hiding the mirror since we only want to see it once clicked.
        HideMirrorCloseUp();

        //Subscribing to the relevant events
        MirrorButton.Pressed += ShowMirrorCloseUp;
        Close.Pressed += HideMirrorCloseUp;
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
}
