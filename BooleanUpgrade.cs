using Godot;
using System;

public class BooleanUpgrade : IncrementalUpgrade
{
    //the bool that gets incremented when an upgrade is purchased
    public bool unlocked { get; set; }

    public bool alreadyUpgraded { get; set;}
}
