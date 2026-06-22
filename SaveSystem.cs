using Godot;
using Godot.Collections;
using System;
using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;


public static class SaveSystem
{
    public static void SaveToFile_Settings()
    {
       // to realize to the Json format, Godot use Dictionary. I used String as for the save varables, becuase it was easier to add to the Json file
       Dictionary<string, string> Save_Settings = new Dictionary<string, string>();

       // Add the Data to the Dictonary
       Save_Settings.Add("SXF", SettingsControls.SXF_Volume.ToString());
       Save_Settings.Add("Ambient", SettingsControls.Ambient_Volume.ToString());
       Save_Settings.Add("Music", SettingsControls.Music_Volume.ToString());
       Save_Settings.Add("Fullscreen", SettingsControls.FullscreenToggle_on.ToString());

       // Json uses strings to get written, all the data is in the saveJson
       string saveJson = Json.Stringify(Save_Settings);

       // File gets open, it gets a name, then it writes the dictinories
       using var settingsfile_system = FileAccess.Open("user://Settings.Json", FileAccess.ModeFlags.Write);
       
       // It stores the String in the Json and closes the file
       settingsfile_system.StoreString(saveJson);
       settingsfile_system.Close();

    }

    public static void LoadFile_Settings()
    {
        // checks for the existing file
        if (!FileAccess.FileExists("user://Settings.Json"))
        {
            return;
        }
        // File gets open. Needs a name, which it file should open. Reads the file
        using var file_system = FileAccess.Open("user://Settings.Json", FileAccess.ModeFlags.Read);

        // The Content_System gets the Text from the Json file.
        string Content_System = file_system.GetAsText();

        // now it gets converted to the dictonary again
        var data = Json.ParseString(Content_System).AsGodotDictionary();

        // all the varables get set to the SettingsControls
        SettingsControls.Music_Volume = float.Parse((string)data["Music"]);
        SettingsControls.SXF_Volume = float.Parse((string)data["SXF"]);
        SettingsControls.Ambient_Volume = float.Parse((string)data["Ambient"]);
        SettingsControls.FullscreenToggle_on = bool.Parse((string)data["Fullscreen"]);


        // set up the settings for the mainmenu
        Setup_Settings();

    }


    public static void Setup_Settings()
    {
        // Fullscreen gets toogled, so it gets loaded in the mainmenu
        if (SettingsControls.FullscreenToggle_on == true)
        {
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);

        }
        else
        {
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);

        }

        // Audio gets set, so it gets loaded in the mainmenus
        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Master"), SettingsControls.Music_Volume);
        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("SXF"), SettingsControls.SXF_Volume);
        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Ambient"), SettingsControls.Ambient_Volume);
    }

    public static void Save_Days()
    {

        Dictionary<string, string> saveDays = new Dictionary<string, string>();

        saveDays.Add("DaysLeft", GlobalData.Countdown.ToString());
        saveDays.Add("DaysIngame", GlobalData.Player_Ingame_Days.ToString());
        string saveJson = Json.Stringify(saveDays);

        using var Daysfile_System = FileAccess.Open("user://Days.Json", FileAccess.ModeFlags.Write);
        Daysfile_System.StoreString(saveJson);
        Daysfile_System.Close();
    }

    public static void Delete_Days()
    {
        // sets the varabiles for the player to their standard
        GlobalData.Countdown = 4;
        GlobalData.Player_Ingame_Days = 0;
       
        // delets the json file "user://Days.Json"
        DirAccess.RemoveAbsolute("user://Days.Json");
        // reloads the editor, so the game is up to date if the file is deleted
        //string g = ProjectSettings.GlobalizePath("user://Days.Json");
       
       //EditorInterface.Singleton.GetResourceFilesystem().UpdateFile("user://Days.Json");
       // saves the files

    }

    public static void Load_Days()
    {
        if (!FileAccess.FileExists("user://Days.Json"))
        {
            return;
        }

        using var Daysfile_System = FileAccess.Open("user://Days.Json", FileAccess.ModeFlags.Read);
        string Content = Daysfile_System.GetAsText();
        var data = Json.ParseString(Content).AsGodotDictionary();
        GlobalData.Countdown = int.Parse((string)data["DaysLeft"]);
        GlobalData.Player_Ingame_Days = int.Parse((string)data["DaysIngame"]);

    }
}
