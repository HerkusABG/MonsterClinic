using System;
using System.Collections.Generic;

public static class TagList
{
    public static Dictionary<string, Tag> Database = new()
    {
        ["Worsening"] = new WorseningTag
        {
            types =
            {
                TagType.Daily
            },
            increment = 2,
            strength = 1
        },
        ["Healing"] = new HealingTag
        {
            types =
            {
                TagType.Daily
            },
            increment = 2,
            strength = -1
        },
        ["Unstable"] = new UnstableTag
        {
            types =
            {
                TagType.Daily,
                TagType.Interaction
            },
            strength = 2
        },
        ["Deadly"] = new DeadlyTag
        {
            types =
            {
                TagType.Daily,
                TagType.MaxSeverity
            }
        },
        ["Resistant"] = new ResistantTag
        {
            types =
            {
                TagType.Interaction
            },
            ratioA = 2,
            ratioB = 4
        }
    };
}