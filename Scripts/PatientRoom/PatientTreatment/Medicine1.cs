using Godot;
using System;

public partial class Medicine1 : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        //basic setup stuff, it's clickable, and it displays the product, price, and how many you already own
        Pressed += ButtonPressed;
        Text = "Medicine 1 \n (Price: 10) \n \n Owned: " + GlobalData.Medicine1Count.ToString();
    }
    private void ButtonPressed()
    {
        //if you can afford it, subtract the price from your money, add it to your inventory, and update the text
        if (DoctorInventory.Money >= 10)
        {
            DoctorInventory.Money -= 10;
            GlobalData.Medicine1Count += 1;
            Text = "Medicine 1 \n (Price: 10) \n \n Owned: " + GlobalData.Medicine1Count.ToString();
        //if you can't afford it, give em the poor idiot popup
        } else
        {
            var Popup = (Label)GetParent().GetParent().GetNode("Insufficient_Funds");
            Popup.Show();
        }
    }

    private void _on_dealer_ph_visibility_changed()
    {
        //refresh the text every time the dealer window's visibility changes
        Text = "Medicine 1 \n (Price: 10) \n \n Owned: " + GlobalData.Medicine1Count.ToString();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
