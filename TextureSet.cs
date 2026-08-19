using Godot;
using System;
using System.Xml.Linq;

public class TextureSet
{
	public Texture2D standing;
	public Texture2D sitting;

	public TextureSet Clone()
	{
		TextureSet textureSet = new TextureSet
		{
			standing = standing,
			sitting = sitting,
		};
		return textureSet;
	}
}
