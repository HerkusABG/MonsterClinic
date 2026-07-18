using Godot;
using System;

//the kind of upgrade that can be purchased multiple times to unlock more and more of what it offers, such as buying extra patient rooms or waiting room seats
public class IncrementalUpgrade
{
    public string name {  get; set; }
    public int price { get; set; }
    public bool fullyUnlocked { get; set; } = false;
    public Medicine medicine { get; set; }

    public Action OnUpgradePressed { get; set; }
}
