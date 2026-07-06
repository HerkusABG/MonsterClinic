using System;
using System.Collections.Generic;

public static class TagList
{
    public static Dictionary<string, Tag> Database = new()
    {
        ["Worsening"] = new WorseningTag
        {
            type = TagType.Daily
        }
    };
}