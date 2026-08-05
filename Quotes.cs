using Godot;
using System;
using System.Collections.Generic;

public static class Quotes
{
    public static Dictionary<string, List<string>> Database = new()
    {
        ["CorrectMedicine"] = new List<string>()
        {
            "This is the correct medicine. Thank you!",
            "I love this medicine. Nice!",
            "I have a feeling this will work :D",
            "Thank you :-)"
        },
        ["IncorrectMedicine"] = new List<string>()
        {
            "I hate this medicine",
            "This won't work whatsoever",
            "This medicine sucks"
        }
    };
}
