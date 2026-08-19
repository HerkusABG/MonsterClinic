using Godot;
using System;

public partial class CatalogueIcon : Control
{
	private Button iconButton;
	private Label iconText;
	private MaladyCatalogSlotUi MCSU;
	CatalogueInfoPackage iconPackage;
    public void Initialize(CatalogueInfoPackage package)
	{
		Show();
		iconText = GetNode<Label>("IconText");

		iconButton = GetNode<Button>("Button");

        MCSU = GetTree().Root.FindChild("MaladyCatalogSlotUi", true, false) as MaladyCatalogSlotUi;

		iconPackage = package;

        iconText.Text = package.name;
		iconButton.Pressed += OnIconClick;
    }

	private void OnIconClick()
	{
		MCSU.DisplayMaladyInfo(iconPackage);
    }
}
