using Godot;
using System;

public partial class TabInfo : TabBar
{
	Label label;
	public void Initialize()
	{
		label = GetNode<Label>("Text");
	}

	public void Write(string input)
	{
        label.Text = input;
    }
}
