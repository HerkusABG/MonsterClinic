using Godot;
using System;

//for all comments, just refer to GiveMedicine1, it's the same code, don't make me copy-paste all of it

public partial class GiveMedicine3 : Button
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Text = $"{MedicineManager.Database["Ozempic"].name} \n Owned: {MedicineManager.Database["Ozempic"].amount}";
        Pressed += ButtonPressed;
    }

    private void _on_room_visibility_changed()
    {
        Text = $"{MedicineManager.Database["Ozempic"].name} \n Owned: {MedicineManager.Database["Ozempic"].amount}";
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
        else if (MedicineManager.Database["Ozempic"].amount > 0)
        {
            if (GlobalData.CurrentPatientMalady != "C")
            {
                Wrong_Medicine_Popup.Show();
                var med1 = (Button)GetParent().GetNode("Give_Medicine_1");
                var med2 = (Button)GetParent().GetNode("Give_Medicine_2");
                Disabled = true;
                med1.Disabled = true;
                med2.Disabled = true;

            }
            else
            {
                GlobalData.CurrentPatientSeverity -= 1;
                MedicineManager.Database["Ozempic"].amount--;
                Text = $"{MedicineManager.Database["Ozempic"].name} \n Owned: {MedicineManager.Database["Ozempic"].amount}";
                if (GlobalData.CurrentPatientSeverity == 0)
                {
                    var cured = (Label)GetParent().GetNode("Patient_Cured_Popup");
                    cured.Show();
                    var med1 = (Button)GetParent().GetNode("Give_Medicine_1");
                    var med2 = (Button)GetParent().GetNode("Give_Medicine_2");
                    Disabled = true;
                    med1.Disabled = true;
                    med2.Disabled = true;
                    GlobalData.DailyEarnings += 100;
                }
            }
        }
    }
}

