using Godot;
using System;

public partial class RoomStructureRenderer
{
    //This class is used for managing the visual room rendering inside of
    //the "map" tab in the computer. This class should only be responsible for generating these
    //buttons and nothing else.
    public void GenerateRooms(GridContainer container, int amount)
	{
        GD.Print("GenerateMapButtons");
        
        for (int i = 0; i < amount; i++)
        {
            Button newButton = new Button();
            newButton.Name = "MapRoom" + amount;
            newButton.Modulate = new Color(1, 0, 0, 1);
            newButton.CustomMinimumSize = new Vector2(150, 150);
            //newButton.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            //newButton.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
            newButton.Text = $"Room {Upgrades.IntUpgradeDatabase["Rooms"].incrementTarget}";
            //newButton.Position = new Vector2(x + 200, 800);
            //control.AddChild(newButton);
            container.AddChild(newButton);
        }
    }

    public void GenerateRoom(GridContainer container)
    {
        Button newButton = new Button();
        newButton.Name = "MapRoom" + Upgrades.IntUpgradeDatabase["Rooms"].incrementTarget.ToString();
        newButton.Modulate = new Color(1, 0, 0, 1);
        newButton.CustomMinimumSize = new Vector2(150, 150);
        newButton.Text = $"Room {Upgrades.IntUpgradeDatabase["Rooms"].incrementTarget}";
        container.AddChild(newButton);
    }
}
