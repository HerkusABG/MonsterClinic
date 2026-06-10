using Godot;

public partial class Room : Node2D 
{
    [Export] public Sprite2D PatientDisplay;

    
    public override void _EnterTree()
    {

        // VisibilityChanged += OnVisibilityChanged;
        Hide();
    }

    private void _on_leave_room_pressed()
    {
        //when leaving the room, hide it, show the office, and pop the room off the previous scenes stack, to not interfere with the right click functionality
        Hide();
        var OfficeScene = (Node2D)GetParent().GetNode("Office");
        OfficeScene.Show();
        GlobalData.PreviousScenes.Pop();
    }

    private void OnVisibilityChanged()
    {
        
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