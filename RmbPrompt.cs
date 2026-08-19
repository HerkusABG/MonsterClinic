using Godot;
using System;

public partial class RmbPrompt : MarginContainer
{
    [Export] private RichTextLabel _promptLabel;
    [Export] private Timer _displayTimer;

    public override void _Ready()
    {
        if (_promptLabel != null)
            _promptLabel.Hide();

        if (_displayTimer != null)
        {
            _displayTimer.WaitTime = 5.0f;
            _displayTimer.OneShot = true;
            _displayTimer.Timeout += OnTimerTimeout;
            _displayTimer.Start();
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
      
      //HIDE OR 5 SECOND TIMER RETSTARTS 
        if (@event is InputEventMouseMotion mouseMotion)
        {
            if (mouseMotion.Relative.LengthSquared() > 1.0f)
            {
                RestartTimer();
            }
        }
        
        
        else if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
        {
            RestartTimer();
        }
    }

    private void OnTimerTimeout()
    {
        if (_promptLabel != null)
            _promptLabel.Show();
    }

    public void HidePrompt()
    {
        if (_displayTimer != null)
            _displayTimer.Stop();

        if (_promptLabel != null)
            _promptLabel.Hide();
    }

    public void RestartTimer()
    {
        HidePrompt();
        if (_displayTimer != null)
            _displayTimer.Start();
    }
}
