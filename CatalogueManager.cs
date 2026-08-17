using Godot;
using System;
using System.Linq;

public partial class CatalogueManager : Control
{
	[Export] Control IconTemplate;

	public void Initialize()
	{
        GridContainer container = GetNode<GridContainer>("ScrollCatalog/GridContainer");
        for(int i = 1; i < MaladyList.Database.Count; i++)
        {
            CatalogueIcon clonedIcon = IconTemplate.Duplicate() as CatalogueIcon;
            container.AddChild(clonedIcon);

            CatalogueInfoPackage package = new CatalogueInfoPackage(MaladyList.Database.ElementAt(i).Value);
            clonedIcon.Initialize(package);
        }
    }
}
