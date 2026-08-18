using System;
using System.Collections.Generic;
using System.Linq;

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
			behaviour = "Severity worsens by 1 every 2 days.",
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
            behaviour = "Severity improves by 2 every 2 days.",
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
            behaviour = "Using the wrong type of medicine will increase the severity by 2.",
            strength = 2
		},
		["Deadly"] = new DeadlyTag
		{
			name = "Deadly",
			types =
			{
				TagType.Daily,
				TagType.MaxSeverity
			},
            behaviour = "Patient will die if severity reaches 5.",
        },
		["Resistant"] = new ResistantTag
		{
			name = "Resistant",
			types =
			{
				TagType.Interaction
			},
            behaviour = "There's a chance that the treatment will fail.",
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
            behaviour = "Severity worsens by 2 every 2 days",
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
            behaviour = "Severity worsens by 1 every 3 days.",
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
            behaviour = "Severity improves by 1 every 3 days.",
            increment = 3,
			strength = -1
		}
	};

    public static void Initialize()
    {
        SaveCatalogueInfo();
    }
    public static void SaveCatalogueInfo()
    {
        for (int i = 0; i < Database.Count; i++)
        {
            CatalogueInfoPackage package = new CatalogueInfoPackage(Database.ElementAt(i).Value);
            InfoList.Tags.Add(package);
        }
    }
}
