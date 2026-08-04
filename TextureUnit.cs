using Godot;
using System;

public class TextureUnit
{
	public TextureSet BodySet;
    public TextureSet HeadSet;
    public TextureSet MaladySet;
    public TextureType unitType;

    public Texture2D Clothing;

    public void Initialize()
	{
        unitType = TextureType.None;
        BodySet = new TextureSet();
		BodySet = TextureList.GetRandomSet(TextureList.BodyTextures).Clone();
        HeadSet = TextureList.GetRandomSet(TextureList.HeadTextures).Clone();
        Clothing = TextureList.GetRandomTexture(TextureList.CivilianOutfits);
    }

    public void SaveMaladySet(string input, TextureType type)
    {
        if (type == TextureType.Normal)
        {
            MaladySet = TextureList.MaladyTextures[input].Clone();
        }
        else
        {
            MaladySet = TextureList.TopMaladyTextures[input].Clone();
        }
        //TextureList.GetRandomSet(TextureList.BodyTextures).Clone();
    }
}

public enum TextureType
{
    None,
    Normal,
    Top
}
