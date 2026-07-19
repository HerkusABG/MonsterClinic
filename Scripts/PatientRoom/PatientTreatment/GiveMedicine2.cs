using Godot;
using System;

//for all comments, just refer to GiveMedicine1, it's the same code, don't make me copy-paste all of it

public partial class GiveMedicine2 : Button
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Text = $"{MedicineManager.Database["Aspirin"].name} \n Owned: {MedicineManager.Database["Aspirin"].amount}";
        Pressed += ButtonPressed;
    }

    private void _on_room_visibility_changed()
    {
        Text = $"{MedicineManager.Database["Aspirin"].name} \n Owned: {MedicineManager.Database["Aspirin"].amount}";
    }

    private void ButtonPressed()
    {
        var patient = (Node2D)GetParent().GetParent().GetNode("Patient_Display");
        var No_Patient_Popup = (Label)GetParent().GetNode("No_Patient_Popup");
        var Wrong_Medicine_Popup = (Label)GetParent().GetNode("Wrong_Medicine_Popup");
        if (patient.Visible == false)
        {
            No_Patient_Popup.Show();

        }
        else if (MedicineManager.Database["Aspirin"].amount > 0)    
        {
            if (GlobalData.CurrentPatientMalady != "B")
            {
                Wrong_Medicine_Popup.Show();
                var med1 = (Button)GetParent().GetNode("Give_Medicine_1");
                var med3 = (Button)GetParent().GetNode("Give_Medicine_3");
                Disabled = true;
                med1.Disabled = true;
                med3.Disabled = true; 

            } 
            else 
            {
                GlobalData.CurrentPatientSeverity -= 1;
                MedicineManager.Database["Aspirin"].amount--;
                Text = $"{MedicineManager.Database["Aspirin"].name} \n Owned: {MedicineManager.Database["Aspirin"].amount}";
                if (GlobalData.CurrentPatientSeverity == 0)
                {
                    var cured = (Label)GetParent().GetNode("Patient_Cured_Popup");
                    cured.Show();
                    var med1 = (Button)GetParent().GetNode("Give_Medicine_1");
                    var med3 = (Button)GetParent().GetNode("Give_Medicine_3");
                    Disabled = true;
                    med1.Disabled = true;
                    med3.Disabled = true;
                    GlobalData.DailyEarnings += 70;
                }
            }
        }
    }
}

