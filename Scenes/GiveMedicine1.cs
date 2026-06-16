using Godot;
using System;

public partial class GiveMedicine1 : Button
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

        Text = "Medicine 1 \n Owned: " + GlobalData.Medicine1Count.ToString();
        Pressed += ButtonPressed;
    }

    private void _on_room_visibility_changed()
    {
        //update the text whenever the scene's visibility changes. Is it kinda random? Yes. But I can't exactly send a signal from the dealer all the way here
        //whenever you buy something, and this is better than updating every frame, so this is what we're doing
        Text = "Medicine 1 \n Owned: " + GlobalData.Medicine1Count.ToString();
    }

    private void ButtonPressed()
    {
        var patient = (Node2D)GetParent().GetParent().GetNode("Patient_Display");
        var No_Patient_Popup = (Label)GetParent().GetNode("No_Patient_Popup");
        var Wrong_Medicine_Popup = (Label)GetParent().GetNode("Wrong_Medicine_Popup");

        //if there's no patient, show the appropriate popup
        if (patient.Visible == false)
        {
            No_Patient_Popup.Show();

        }
        else if (GlobalData.Medicine1Count > 0)
        {
            //if you try to use the medicine on the wrong malady, you get the appropriate popup, and the medicie buttons get disabled until you close it
            if (GlobalData.CurrentPatientMalady != "A")
            {
                Wrong_Medicine_Popup.Show();
                var med2 = (Button)GetParent().GetNode("Give_Medicine_2");
                var med3 = (Button)GetParent().GetNode("Give_Medicine_3");
                Disabled = true;
                med2.Disabled = true;
                med3.Disabled = true;

            }
            else
            {
                //the correct use of the medicine, severity goes down, you consume 1 medicine, the text gets updated
                GlobalData.CurrentPatientSeverity -= 1;
                GlobalData.Medicine1Count--;
                Text = "Medicine 1 \n Owned: " + GlobalData.Medicine1Count.ToString();
                //if you get the severity down to 0, the patient is cured, you get a popup, and you get paid
                if (GlobalData.CurrentPatientSeverity == 0)
                {
                    var cured = (Label)GetParent().GetNode("Patient_Cured_Popup");
                    cured.Show();
                    var med2 = (Button)GetParent().GetNode("Give_Medicine_2");
                    var med3 = (Button)GetParent().GetNode("Give_Medicine_3");
                    Disabled = true;
                    med2.Disabled = true;
                    med3.Disabled = true;
                    GlobalData.DailyEarnings += 40;
                }
            }
        }
    }
}
