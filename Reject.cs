using Godot;
using System;

public partial class Reject : Button
{
    
    [Export] public Sprite2D PortraitSprite;

    public override void _Ready()
    {
        Pressed += ChangeColor;
    }

    private void ChangeColor()
    {
    
        Random random = new Random();

        // Declare 'newTint' so the code knows what it is
        Color newTint = new Color( 
            (float)random.NextDouble(), 
            (float)random.NextDouble(), 
            (float)random.NextDouble() 
        ); 

        // Apply it to the sprite
        PortraitSprite.Modulate = newTint;
    }
}
   