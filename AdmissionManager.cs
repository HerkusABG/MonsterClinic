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

        // takes sprite to patient rooom
        GetTree().ChangeSceneToFile("res://Room.tscn"); 
    }
}