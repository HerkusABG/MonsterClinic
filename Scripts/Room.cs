using Godot;
using static System.Net.Mime.MediaTypeNames;

public partial class Room : Node2D 
{
    //Storing a reference to all the buttons, labels, etc., for easy reference in the methods

    //part of Princess's old stuff, keeping it around just in case
    //[Export] public Sprite2D PatientDisplay;
    Button LeaveRoomButton;
    Sprite2D PatientDisplay;
    Label PatientInfo;
    Button GiveMedicine1Button;
    Button GiveMedicine2Button;
    Button GiveMedicine3Button;
    Label WrongMedicinePopup;
    Button CloseWrongMedicinePopup;
    Label NoPatientPopup;
    Button CloseNoPatientPopup;
    Label PatientCuredPopup;
    Button ClosePatientCuredPopup;
    Label CorrectMedicinePopup;
    Button CloseCorrectMedicinePopup;

    //part of Princess's old stuff, keeping it around just in case
    /*public override void _EnterTree()
    {

        // VisibilityChanged += OnVisibilityChanged;
        Hide();
    }*/

    public override void _Ready()
    {
        //grabs references to all the necessary nodes
        GetNodes();

        //assigning methods to all the buttons
        LeaveRoomButton.MouseEntered += HoverOn;
        LeaveRoomButton.MouseExited += HoverOff;
        LeaveRoomButton.Pressed += LeaveRoom;

        //yes this looks kinda wacky, but apparently that's how I gotta write it if I want to have methods that take arguments
        CloseWrongMedicinePopup.Pressed += () => CloseParent(CloseWrongMedicinePopup);
        CloseNoPatientPopup.Pressed += () => CloseParent(CloseNoPatientPopup);
        ClosePatientCuredPopup.Pressed += () => CloseParent(ClosePatientCuredPopup);
        CloseCorrectMedicinePopup.Pressed += () => CloseParent(CloseCorrectMedicinePopup);




    }

    private void GetNodes()
    {
        //Basically just grabbing all the nodes
        LeaveRoomButton = GetNode<Button>("Leave_Room");
        PatientDisplay = GetNode<Sprite2D>("Patient_Display");
        PatientInfo = GetNode<Label>("Patient_Info");

        GiveMedicine1Button = GetParent().GetNode("Inventory").GetNode("Open_Inventory").GetNode<Button>("Give_Medicine_1");
        GiveMedicine2Button = GetParent().GetNode("Inventory").GetNode("Open_Inventory").GetNode<Button>("Give_Medicine_2");
        GiveMedicine3Button = GetParent().GetNode("Inventory").GetNode("Open_Inventory").GetNode<Button>("Give_Medicine_3");
        WrongMedicinePopup = GetNode<Label>("Wrong_Medicine_Popup");
        CloseWrongMedicinePopup = WrongMedicinePopup.GetNode<Button>("Close_Wrong_medicine_Popup");
        NoPatientPopup = GetNode<Label>("No_Patient_Popup");
        CloseNoPatientPopup = NoPatientPopup.GetNode<Button>("Close");
        PatientCuredPopup = GetNode<Label>("Patient_Cured_Popup");
        ClosePatientCuredPopup = PatientCuredPopup.GetNode<Button>("Close_Patient_Cured_Popup");
        CorrectMedicinePopup = GetNode<Label>("Correct_Medicine_Popup");
        CloseCorrectMedicinePopup = CorrectMedicinePopup.GetNode<Button>("Close_Correct_medicine_Popup");
    }

    private void HoverOn()
    {
        //makes the text show up when hovering over the button
        LeaveRoomButton.Text = "Leave room";
    }

    private void HoverOff()
    {
        //makes the text disappear when you stop hovering
        LeaveRoomButton.Text = "";
    }

    private void LeaveRoom()
    {
        //when leaving the room, hide it, show the office, and pop the room off the previous scenes stack, to not interfere with the right click functionality
        Hide();
        var OfficeScene = (Node2D)GetParent().GetNode("Office");
        OfficeScene.Show();
        GlobalData.PreviousScenes.Pop();
    }

    //universal method for closing a node's parent, used for all the x's in the top right of popups
    private void CloseParent(Button button)
    {
        var Parent = button.GetParent();
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
            PatientDisplay.Hide();
            PatientInfo.Hide();

            GlobalData.CurrentPatientMalady = "none";
            GlobalData.CurrentPatientSeverity = 0;
        }
    }

    //generally updating all the text that might've changed while the scene was hidden, whenever the visibility is changed
    //it could activate whenever the scene's visibility changes,but there's a (probably) harmless error that happens then, so I instead use a node who's visibility always matches the scene
    private void _on_patient_room_background_visibility_changed()
    {
        PatientInfo.Text = "Patient info: \n Malady: Malady " + GlobalData.CurrentPatientMalady + "\n Severity: " + GlobalData.CurrentPatientSeverity;

        GiveMedicine1Button.Text = $"{MedicineManager.Database["Morphine"].name} \n Owned: {MedicineManager.Database["Morphine"].amount}";
        GiveMedicine2Button.Text = $"{MedicineManager.Database["Aspirin"].name} \n Owned: {MedicineManager.Database["Aspirin"].amount}";
        GiveMedicine3Button.Text = $"{MedicineManager.Database["Ozempic"].name} \n Owned: {MedicineManager.Database["Ozempic"].amount}";

        WrongMedicinePopup.Hide();
        NoPatientPopup.Hide();
        CorrectMedicinePopup.Hide();

        //part of Princess's old stuff, keeping it around just in case
        /*if (IsVisibleInTree())
        {
            if (GlobalData.AdmittedPatientTexture != null)
            {
                PatientDisplay.Texture = GlobalData.AdmittedPatientTexture;
                PatientDisplay.Show();
            }
            else
            {
                PatientDisplay.Hide();
            }
        }*/
    }
}