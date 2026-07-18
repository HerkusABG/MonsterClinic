using Godot;
using System;

public partial class Computer : TextureButton
{
    private AnimationPlayer _AnimationPlayer;
    private TextureRect _ScreenGlow;

    public override void _Ready()
    {
        _AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        _ScreenGlow = GetNode<TextureRect>("ScreenGlow");

        if (_ScreenGlow != null)
        {
            _ScreenGlow.Visible = false;
        }

        StartNotificationGlow();
    }

    public void StartNotificationGlow()
    {
        if (_ScreenGlow != null && _AnimationPlayer != null)
        {
            _ScreenGlow.Visible = true;
            _AnimationPlayer.Play("flash");
        }
    }

    public void StopNotificationGlow()
    {
        if (_AnimationPlayer != null)
        {
            _AnimationPlayer.Stop();
        }

        if (_ScreenGlow != null)
        {
            _ScreenGlow.Visible = false;
            _ScreenGlow.SelfModulate = new Color(1, 1, 1, 0);
        }
    }

    private void _on_pressed()
    {
        StopNotificationGlow();
    }
}

//I WILL ADD LOGIC AND COMMENTS LATER JUST WANTED TO HAVE THE FLAHSING WOKRING AND TALK MORE WITH TILDE ON NOTIFICATIONS