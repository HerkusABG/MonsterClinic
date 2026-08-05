using Godot;
using System;

public partial class TabInfo : TabBar
{
	RichTextLabel label;
	public void Initialize()
	{
		label = GetNode<RichTextLabel>("Text");
	}

	public void Write(string input)
	{
        label.Text = input;
    }
}
