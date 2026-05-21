using Godot;

public partial class Room : Node2D 
{
    //creates a slot in the sinpector for new 2dsprite
    [Export] public Sprite2D PatientDisplay;

    // _Ready is called the exact moment the Room scene finishes loading
    public override void _Ready()
    {
        // check if a patient was sent over the bridge
        if (GlobalData.AdmittedPatientTexture != null)
        {
            //  apply the saved texture to the room's sprite
            PatientDisplay.Texture = GlobalData.AdmittedPatientTexture;
            
            
            PatientDisplay.Show(); 
        }
        else
        {
            // if someone opens the room without admitting a patient, hide the sprite
            PatientDisplay.Hide(); 
        }
    }
 }