using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class SelfTreatment : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        Pressed += ButtonPressed;
        Text = "Self Treatment \n (Price:" + GlobalData.MedicinePlayer + "\n \n Owned: " + GlobalData.MedicinePlayer.ToString() + " \n availability in: " + GlobalData.Medicincavailability.ToString();


    }
    private void ButtonPressed()

    {
        // Check if the player has the money and if the medicine is available before allowing him to purchase item
        if (DoctorInventory.Money >= GlobalData.MedicineCost && GlobalData.Medicincavailability <= 0)
        {
            // Money deduction, player gets the medicine and the cost of the medicine gets increased (probally needs balancing)
            DoctorInventory.Money -= GlobalData.MedicineCost;
            GlobalData.MedicinePlayer ++;
            GlobalData.MedicineCost = GlobalData.MedicineCost * 2; // Increase the cost for the next purchase
            Text = "Self Treatment \n (Price: " + GlobalData.MedicineCost + ") \n \n Owned: " + GlobalData.MedicinePlayer.ToString() + " \n availability in: " + GlobalData.Medicincavailability.ToString();
        
        }
        else if (DoctorInventory.Money < GlobalData.MedicineCost)
        {
            var Popup = (Label)GetParent().GetParent().GetNode("Insufficient_Funds");
            Popup.Show();
        }
        else
        {
            var Popup = (Label)GetParent().GetParent().GetNode("Insufficient_Availability");
            Popup.Show();
        }
    }

    private void _on_dealer_ph_visibility_changed()
    {
        Text = "SelfTreatment \n (Price: " + GlobalData.MedicineCost + ") \n \n Owned: " + GlobalData.MedicinePlayer.ToString() + " \n availability in: " + GlobalData.Medicincavailability.ToString();
    }


 // Called every frame. 'delta' is the elapsed time since the previous frame.
 public override void _Process(double delta)
	{
        if (GlobalData.Medicincavailability > 0)
        {
            Text = "Self Treatment \n (Price: " + GlobalData.MedicineCost + ") \n \n Owned: " + GlobalData.MedicinePlayer.ToString() + " \n availability in: " + GlobalData.Medicincavailability.ToString();
        }
        else
        {
            Text = "Self Treatment \n (Price: " + GlobalData.MedicineCost + ") \n \n Owned: " + GlobalData.MedicinePlayer.ToString() + " \n availability in: 0";

        }
      
    }
}
