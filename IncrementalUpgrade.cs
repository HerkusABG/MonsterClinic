using Godot;
using System;

//the kind of upgrade that can be purchased multiple times to unlock more and more of what it offers, such as buying extra patient rooms or waiting room seats
public class IncrementalUpgrade
{
    //the int that gets incremented when an upgrade is purchased
    public int incrementTarget { get; set; }
    //the limit on how many upgrades can be bought (just set it stupid high if there's no cap)
    public int cap {  get; set; }
    public int price { get; set; }
}
