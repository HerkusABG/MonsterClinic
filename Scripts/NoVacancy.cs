using Godot;
using System;

public partial class NoVacancy : Panel
{
   
    private Label _vacancyLabel; 
    
    [Export] public float FlashSpeed = 25.0f; // Fast speed for an "error" flicker , WILL CHNAGE LATER ACCORDINLY TO WHAT TILDE HAS MIND FOR LATER
    [Export] public float FlickerDuration = 1.5f; // FLASH DURATION, WILL CHANGE LATER ACCORDINGLY TO WHAT TILDE HAS MIND FOR LATER

    private float _flickerTimer = 0.0f;
    private float _timePassed = 0.0f;

    public override void _Ready()
    {
        this.Visible = false;

        
	    _vacancyLabel = GetParent().GetNode<Label>("PatientsLeftParent").GetNode<Label>("PatientsLeftLabel");    
    }

    public override void _Process(double delta)
    {
        if (_flickerTimer > 0.0f)
        {
            _flickerTimer -= (float)delta;
            _timePassed += (float)delta * FlashSpeed;

            // Rapidly turn visibility on and off to create a sharp flicker effect
            this.Visible = Mathf.Sin(_timePassed) > 0.0f;

            if (_flickerTimer <= 0.0f)
            {
                this.Visible = false;
                if (_vacancyLabel != null)
                {
                    _vacancyLabel.Text = ""; // Clear out the alert text when finished
                }
            }
        }
    }

    // Call this method from the Admission Manager/Interface when Admit fails
    public void TriggerFullWarning()
    {
        _flickerTimer = FlickerDuration;
        _timePassed = 0.0f;
        this.Visible = true;

        if (_vacancyLabel != null)
        {
            _vacancyLabel.Text = "NO VACANCY";
        }
    }
}