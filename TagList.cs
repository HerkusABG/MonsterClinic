using System;
using System.Collections.Generic;

public static class TagList
{
	public static Dictionary<string, Tag> Database = new()
	{
		["Worsening"] = new WorseningTag
		{
			name = "Worsening",
			types =
			{
				TagType.Daily
			},
			increment = 2,
			strength = 1
		},
		["Healing"] = new HealingTag
		{
            name = "Healing",
            types =
			{
				TagType.Daily
			},
			increment = 2,
			strength = -2
		},
		["Unstable"] = new UnstableTag
		{
            name = "Unstable",
            types =
			{
				TagType.Daily,
				TagType.Interaction
			},
			strength = 2
		},
		["Deadly"] = new DeadlyTag
		{
            name = "Deadly",
            types =
			{
				TagType.Daily,
				TagType.MaxSeverity
			}
		},
		["Resistant"] = new ResistantTag
		{
            name = "Resistant",
            types =
			{
				TagType.Interaction
			},
			ratioA = 2,
			ratioB = 4
		},
		["StrongWorsening"] = new WorseningTag
		{
            name = "Strong Worsening",
            types =
			{
				TagType.Daily
			},
			increment = 2,
			strength = 2
		},
		["WeakWorsening"] = new WorseningTag
		{
            name = "Weak Worsening",
            types =
			{
				TagType.Daily
			},
			increment = 3,
			strength = 1
		},
		["WeakHealing"] = new HealingTag
		{
            name = "Weak Healing",
            types =
			{
				TagType.Daily
			},
			increment = 3,
			strength = -1
		}
	};
}
