using Godot;
using System;

public partial class DealerButton : Button
{
	public int index;
	Label label;

	public void Initialize()
	{
		label = GetNode<Label>("DealerLabel");
    }

	public void ChangeText(string input)
	{
		label.Text = input;
	}
}
