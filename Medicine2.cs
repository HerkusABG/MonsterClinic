using Godot;
using System;

public partial class Medicine2 : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        //basic setup stuff, it's clickable, and it displays the product, price, and how many you already own
        Pressed += ButtonPressed;
        Text = "Medicine 2 \n (Price: 20) \n \n Owned: " + GlobalData.Medicine2Count.ToString();
    }
    private void ButtonPressed()
    {
        //if you can afford it, subtract the price from your money, add it to your inventory, and update the text
        if (DoctorInventory.Money >= 20)
        {
            DoctorInventory.Money -= 20;
            GlobalData.Medicine2Count += 1;
            Text = "Medicine 2 \n (Price: 20) \n \n Owned: " + GlobalData.Medicine2Count.ToString();
        }
        //if you can't afford it, give em the poor idiot popup
        else
        {
            var Popup = (Label)GetParent().GetParent().GetNode("Insufficient_Funds");
            Popup.Show();
        }
    }

    private void _on_dealer_ph_visibility_changed()
    {
        Text = "Medicine 2 \n (Price: 20) \n \n Owned: " + GlobalData.Medicine2Count.ToString();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
