using Godot;
using System;
using System.Xml.Linq;

public partial class ControlSettingsContainer : Control
{
    // The varables gets the settings from the SettingsControls
    Boolean toggle_on = SettingsControls.FullscreenToggle_on;
    float music_volume = SettingsControls.Music_Volume;
    float sxf_volume = SettingsControls.SXF_Volume;
    float ambient_volume = SettingsControls.Ambient_Volume;
    public override void _Ready()
	{
        // Loads the file
        SaveSystem.LoadFile_Settings();

        // The visuals show the settings
        SetvisualsSettings();


    }

 


    private void _on_window_size_slider_toggled(Boolean toggle_on)
	{
        // checks for Fullscreen or Window
		if (toggle_on == true)
		{
            // Fullscreen
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.ExclusiveFullscreen);
            // Sets the toggle_on to the SettingsControls -> it is easier for the save system
            SettingsControls.FullscreenToggle_on = toggle_on;
        
        }
		else
		{
            // Window
            //grab the size we want for the windowed mode, fullscreen / 2
            Vector2I windowSize = new Vector2I (DisplayServer.WindowGetSize().X / 2,DisplayServer.WindowGetSize().Y / 2);
            //go windowed
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
            //resize to the desired size
            DisplayServer.WindowSetSize(windowSize);
            // Sets the toggle_on to the SettingsControls -> it is easier for the save system
            SettingsControls.FullscreenToggle_on = toggle_on;
            //GD.Print("kibidi");
        }
        
    }

  

    private void _on_music_slider_value_changed(float music_volume)
    {
        // the Audio volume gets set up for the Audioserver. The Audioserver needs a AudioStreamer and a set up for the Audio -> no audios needed just the placeholder
        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Master"), music_volume);
        // Sets the music_volume to the SettingsControls -> it is easier for the save system
        SettingsControls.Music_Volume = music_volume;
    }


    private void _on_sxf_slider_value_changed(float sxf_volume)
    {
        // the Audio volume gets set up for the Audioserver. The Audioserver needs a AudioStreamer and a set up for the Audio -> no audios needed just the placeholder
        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("SXF"), sxf_volume);
        // Sets the sxf_volume to the SettingsControls -> it is easier for the save system
        SettingsControls.SXF_Volume = sxf_volume;
    }


    private void _on_ambient_slide_value_changed(float ambient_volume)
    {
        // the Audio volume gets set up for the Audioserver. The Audioserver needs a AudioStreamer and a set up for the Audio -> no audios needed just the placeholder
        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Ambient"), ambient_volume);
        // Sets the ambient_volume to the SettingsControls -> it is easier for the save system
        SettingsControls.Ambient_Volume = ambient_volume;
    }


    private void SetvisualsSettings()
    {
        // get the Checkbox for the toggle -> The checkbox gets toggled to show the player if it Fullscreened or Windowed
        var fullscreenSlider = GetNode<CheckButton>("TabContainer/Grafic/VBoxContainer3/WindowSize_Slider");
        if (SettingsControls.FullscreenToggle_on == true)
        {
            //Fullscreen
            fullscreenSlider.ButtonPressed = true;
        }
        else
        {
            // Window
            fullscreenSlider.ButtonPressed = false;

        }

        // get all slider
        var musicSlider = GetNode<HSlider>("TabContainer/Sound/VBoxContainer3/Music_Slider");
        var sxfSlider = GetNode<HSlider>("TabContainer/Sound/VBoxContainer3/SXF_Slider");
        var ambientSlider = GetNode<HSlider>("TabContainer/Sound/VBoxContainer3/Ambient_Slide");

        // toggles(set up) the volume slider for the visuals
        musicSlider.Value = music_volume;
        sxfSlider.Value = ambient_volume;
        ambientSlider.Value = sxf_volume;
    }

}
