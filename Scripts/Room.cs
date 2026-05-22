using Godot;

public partial class Room : Node2D 
{
    [Export] public Sprite2D PatientDisplay;

    
    public override void _EnterTree()
    {
        
        VisibilityChanged += OnVisibilityChanged;
    }

    private void _on_back_pressed()
    {
        Hide();
        var OfficeScene = (Node2D)GetParent().GetNode("Office");
        OfficeScene.Show();
    }

    private void OnVisibilityChanged()
    {
        
        if (IsVisibleInTree())
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
        }
    }
}