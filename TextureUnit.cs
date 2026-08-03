using Godot;
using System;

public class TextureUnit
{
	public TextureSet BodySet;
    public TextureSet HeadSet;

    public void Initialize()
	{
		BodySet = new TextureSet();
		BodySet = TextureList.GetRandomSet(TextureList.BodyTextures).Clone();
        HeadSet = TextureList.GetRandomSet(TextureList.HeadTextures).Clone();
        //BodySet.sitting = TextureList.BodyTextures["Body1"].sitting;
    }
}
