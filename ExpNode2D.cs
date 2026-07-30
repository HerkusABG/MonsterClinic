using Godot;
using System;

public partial class ExpNode2D : Node2D
{
    public virtual void OnRoomEnter() { }

    public virtual void OnRoomEnter(Node mainNode) { }
    public virtual void OnRoomExit() { }

    public virtual void OnRoomExit(Node mainNode) { }
}
