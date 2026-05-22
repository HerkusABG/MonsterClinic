using Godot;

public partial class AdmissionManager : Node
{
    [Export] public Sprite2D PatientSpriteDisplay; 

    public void _on_admit_pressed()
    {
        // saves patient sprite
        if (PatientSpriteDisplay != null)
        {
            GlobalData.AdmittedPatientTexture = PatientSpriteDisplay.Texture;
        }

       
        Node mainNode = GetParent().GetParent(); 
        
        
        var roomNode = mainNode.GetNode<Node2D>("Room");
        var patientInterface = mainNode.GetNode<Node2D>("Patient_Interface");

       
        roomNode.Show();
        patientInterface.Hide();
    }
}