using Godot;
using System;

public class IntegerUpgrade : IncrementalUpgrade
{
    //the int that gets incremented when an upgrade is purchased
    public int incrementTarget { get; set; }
    //the limit on how many upgrades can be bought (just set it stupid high if there's no cap)
    public int cap { get; set; }
}
