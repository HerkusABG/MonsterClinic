using Godot;
using System;
using System.Collections.Generic;
using System.Text.Json;

[Serializable]
public class SaveDataDTO
{
    public int Money { get; set; }
    public int InGameDays { get; set; }
    public int Countdown { get; set; }
    public int DailyEarnings { get; set; }
    public bool DialogDealer { get; set; }

    public Dictionary<string, int> MedicineStock { get; set; } = new Dictionary<string, int>();
}

public static class SaveManager
{
    private const string SAVE_PATH = "user://SaveGame.json";

    public static bool SaveFileExists()
    {
        return FileAccess.FileExists(SAVE_PATH);
    }

    public static void SaveGame()
    {
        try
        {
            SaveDataDTO data = new SaveDataDTO
            {
                Money = DoctorInventory.Money,
                InGameDays = GlobalData.Player_Ingame_Days,
                Countdown = GlobalData.Countdown,
                DailyEarnings = GlobalData.DailyEarnings,
                DialogDealer = GlobalData.Dialog_Dealer
            };

            if (MedicineManager.Database != null)
            {
                foreach (var pair in MedicineManager.Database)
                {
                    data.MedicineStock[pair.Key] = pair.Value.amount;
                }
            }

            string jsonString = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            using var saveFile = FileAccess.Open(SAVE_PATH, FileAccess.ModeFlags.Write);

            if (saveFile != null)
            {
                saveFile.StoreString(jsonString);
                GD.Print("[SaveManager] Game saved successfully!");
            }
            else
            {
                GD.PrintErr("[SaveManager] Failed to write save file: " + FileAccess.GetOpenError());
            }
        }
        catch (Exception ex)
        {
            GD.PrintErr("[SaveManager] Exception during SaveGame: " + ex.Message + "\n" + ex.StackTrace);
        }
    }

    public static void LoadGame()
    {
        if (!SaveFileExists())
        {
            GD.PrintErr("[SaveManager] Save file does not exist.");
            return;
        }

        try
        {
            using var saveFile = FileAccess.Open(SAVE_PATH, FileAccess.ModeFlags.Read);
            if (saveFile == null)
            {
                GD.PrintErr("[SaveManager] Failed to read save file: " + FileAccess.GetOpenError());
                return;
            }

            string jsonString = saveFile.GetAsText();
            SaveDataDTO data = JsonSerializer.Deserialize<SaveDataDTO>(jsonString);
            if (data == null) return;

            DoctorInventory.Money = data.Money;
            GlobalData.Player_Ingame_Days = data.InGameDays;
            GlobalData.Countdown = data.Countdown;
            GlobalData.DailyEarnings = data.DailyEarnings;
            GlobalData.Dialog_Dealer = data.DialogDealer;

            if (data.MedicineStock != null && MedicineManager.Database != null)
            {
                foreach (var pair in data.MedicineStock)
                {
                    if (MedicineManager.Database.ContainsKey(pair.Key))
                    {
                        MedicineManager.Database[pair.Key].amount = pair.Value;
                    }
                }
            }

            GD.Print("[SaveManager] Game loaded successfully!");
        }
        catch (Exception ex)
        {
            GD.PrintErr("[SaveManager] Error parsing save data: " + ex.Message);
        }
    }

    public static void DeleteSave()
    {
        if (SaveFileExists())
        {
            DirAccess.RemoveAbsolute(SAVE_PATH);
            GD.Print("[SaveManager] Save file deleted for new game.");
        }
    }
}