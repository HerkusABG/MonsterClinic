using Godot;
using System;

public partial class DealerButton : TextureButton
{
	public int index;
	Label label;
	Label info;
	string storeInfo;

	public void Initialize()
	{
		label = GetNode<Label>("DealerLabel");
		info = GetParent().GetParent().GetParent().GetNode<Label>("Purchase_Info");
    }

	public void ChangeText(string input)
	{
		label.Text = input;
	}

	public void StoreInfo(string input)
	{
		storeInfo = input;
	}

	public void UpdateInfo()
	{
        info.Text = storeInfo;
    }

	private void _on_pressed()
	{
		UpdateInfo();
	}
}
