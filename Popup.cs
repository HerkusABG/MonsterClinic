using Godot;
using System;

public partial class Popup : Control
{
	Label Label;
	public Button Close;
	public void Initialize()
	{
        Label = GetNode<Label>("PopupLabel");
		Close = GetNode<Button>("PopupClose");
        Close.Pressed += CloseParent;
    }

    public void DisplayPopup(string input)
    {
        Show();
        Label.Text = input;
    }

    private void CloseParent()
    {
        Hide();
        /*var Parent = button.GetParent();
        if (Parent.GetClass() == "Label")
        {
            Label ParentLabel = (Label)Parent;
            ParentLabel.Hide();
        }
        if (Parent.GetClass() == "Control")
        {
            var ControlParent = (Control)Parent;
            ControlParent.Hide();
        }


        //in this specific case, we also remove the patient and reset patient malady data
        if (button == ClosePatientCuredPopup)
        {
            Room.UpdateSprites();
            //GlobalData.CurrentPatientMalady = "none";
            //GlobalData.CurrentPatientSeverity = 0;
        }*/
    }

}
