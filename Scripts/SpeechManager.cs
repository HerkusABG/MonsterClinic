using Godot;
using System;

public partial class SpeechManager : Node
{

    //The speech manager is incredibly simple. 
    //All we're doing is storing a label and then the Contents_P_I class
    //Changes the text when needed. 
	[Export] Label SpeechBubble;

    public override void _Ready()
    {
        SpeechBubble.Hide();
    }

    public void SpeechText(string text)
	{
        SpeechBubble.Show();
        SpeechBubble.Text = text;
    }
}
