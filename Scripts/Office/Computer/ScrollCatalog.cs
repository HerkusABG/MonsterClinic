using Godot;
using System;
using System.Collections.Generic;

public partial class ScrollCatalog : ScrollContainer
{
    [Export] public PackedScene SlotUI;
    [Export] PackedScene SlotUIMalady = ResourceLoader.Load<PackedScene>("res://Scenes/malady_catalog_slot_ui.tscn");
    public Malady_Autoload.MaladyData MaladyData;

    public override void _Ready()
    {

        var gridContainer = GetNode<GridContainer>("GridContainer");
        var MaladyAutoload = GetNode<Malady_Autoload>("/root/MaladyAutoload");

        for (int i = 0; i < MaladyAutoload.ListMaladies.Count; i++)
        {
            var slotMalady = SlotUIMalady.Instantiate<MaladyCatalogSlotUi>();
            gridContainer.AddChild(slotMalady);

            MaladyAutoload.indexChecker++;


        }
    }
}
