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
		Mathf.Clamp(reputation, 0, 20);
	}

	public static int GetReputation()
	{
		return reputation;
	}
}

public enum ReputationValue
{
	ShotPatient = -3,
    DeadPatient = -2,
	CuredPatient = 2,
	CuredStoryPatient = 4
}