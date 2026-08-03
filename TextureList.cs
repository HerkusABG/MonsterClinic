using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public static class TextureList
{
    public static Dictionary<string, TextureSet> BodyTextures = new()
    {
        /*["Body1"] = new TextureSet()
        {
            sitting = GD.Load<Texture2D>("res://Assets/2DArt/Patient/Sitting/Body/patient-sitting-body1.png"),
            standing = GD.Load<Texture2D>("res://Assets/2DArt/Patient/Standing/patient-standing-body1.png")
        },
        ["Body2"] = new TextureSet()
        {
            sitting = GD.Load<Texture2D>("res://Assets/2DArt/Patient/Sitting/Body/patient-sitting-body2.png"),
            standing = GD.Load<Texture2D>("res://Assets/2DArt/Patient/Standing/patient-standing-body2.png")
        },
        ["Body3"] = new TextureSet()
        {
            sitting = GD.Load<Texture2D>("res://Assets/2DArt/Patient/Sitting/Body/patient-sitting-body3.png"),
            standing = GD.Load<Texture2D>("res://Assets/2DArt/Patient/Standing/patient-standing-body3.png")
        }*/
    };

    public static Dictionary<string, TextureSet> HeadTextures = new()
    {
        /*["Head1"] = new TextureSet()
        {
            sitting = GD.Load<Texture2D>("res://Assets/2DArt/Patient/Sitting/Head/patient-sitting-head1.png"),
        }*/
    };

    public static void Initialize()
    {
        //LoadTextures();
        LoadDesiredTexture("res://Assets/2DArt/Patient/Standing/Body/patient-standing-body",
            "res://Assets/2DArt/Patient/Sitting/Body/patient-sitting-body",
            "Body",
            3,
            BodyTextures);
        LoadDesiredTexture("res://Assets/2DArt/Patient/Standing/Head/patient-standing-head",
            "res://Assets/2DArt/Patient/Sitting/Head/patient-sitting-head",
            "Head",
            48,
            HeadTextures);
    }

    private static void LoadDesiredTexture(string standingFilepath, string sittingFilepath, string keyName, int count, Dictionary<string, TextureSet> dictionary)
    {
        //string path = filepath;
        for (int i = 0; i < count; i++)
        {
            int index = i + 1;
            string key = "keyName" + (index);

            TextureSet set = new TextureSet()
            {
                standing = GD.Load<Texture2D>(standingFilepath + index + ".png"),
                sitting = GD.Load<Texture2D>(sittingFilepath + index + ".png")
            };
            dictionary.Add(key, set);
        }
    }

    public static TextureSet GetRandomSet(Dictionary<string, TextureSet> dictionary)
    {
        int length = dictionary.Count;
        Random rnd = new Random();
        int randIndex = rnd.Next(0, length);
        GD.Print($"max range is {length}, index is {randIndex}");
        return dictionary.ElementAt(randIndex).Value;
    }
}
