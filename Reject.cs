using Godot;
using System;

public partial class Reject : Button
{
    
    [Export] public Sprite2D PortraitSprite;
	[Export] public Label PatientLabel; 
    [Export] public Label AgeLabel;

	private PatientStats _currentPatient;
    public override void _Ready()
    {
        Pressed += ChangeColor;
		//this immediately changes the color of the sprite when the scene is loaded, this is just for testing, to make sure the randomization works
		ChangeColor();
    }

    private void ChangeColor()
    {
		Random random = new Random();
		PatientStats newPatient = new PatientStats();

		
    
       
		string randomId = random.Next(1, 1000).ToString("D3");
        int randomAge = random.Next(18, 90);
       
	    //this chnages the color of the sprite to a random color
        Color newTint = new Color( 
            (float)random.NextDouble(), 
            (float)random.NextDouble(), 
            (float)random.NextDouble() 
        ); 
		if (PatientLabel != null)
            PatientLabel.Text = $"Patient: {randomId}";

        if (AgeLabel != null)
            AgeLabel.Text = $"Age: {randomAge}";

		// Apply it to the sprite
        if (PortraitSprite != null)
        {
            PortraitSprite.Modulate = newTint;
        }
    }
}
   