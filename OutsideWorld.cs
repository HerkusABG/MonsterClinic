using Godot;
using System;

public static class OutsideWorld
{
	private static int reputation;

	public static void Initialize()
	{
		reputation = 10;
	}

	public static void ChangeReputation(int input)
	{
		reputation += input;
	}

	public static int GetReputation()
	{
		return reputation;
	}
}
