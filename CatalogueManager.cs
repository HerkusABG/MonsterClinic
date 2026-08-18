using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public partial class CatalogueManager : Control
{
	[Export] Control IconTemplate;
    [Export] Control ContainerTemplate;
    private List<CatalogueSection> sections = new List<CatalogueSection>();

    [Export] TextureButton MaladyButton;
    [Export] Control MaladyContainer;

    [Export] TextureButton MedicineButton;
    [Export] Control MedicineContainer;

    [Export] TextureButton TagButton;
    [Export] Control TagContainer;

    public void Initialize()
	{
        /*foreach(Node node in GetChildren())
        {
            TextureButton button = node as TextureButton;
            if(button != null)
            {
                CreateSection(button);
            }
        }*/
        //CreateSection(MaladyList.Database);
        CreateSection(MaladyButton, MaladyContainer, InfoList.Maladies);

        CreateSection(MedicineButton, MedicineContainer, InfoList.Medicines);

        CreateSection(TagButton, TagContainer, InfoList.Tags);
    }


    private void CreateSection(TextureButton inputButton, Control inputContainer, List<CatalogueInfoPackage> packageList)
    {
        CatalogueSection section = new CatalogueSection();
        //GridContainer container = GetNode<GridContainer>("ScrollCatalog/GridContainer");
        GridContainer container = inputContainer.GetNode<GridContainer>("GridContainer");
        inputButton.Pressed += () => ShowContainer(inputContainer);
        for (int i = 1; i < packageList.Count; i++)
        {
            CatalogueIcon clonedIcon = IconTemplate.Duplicate() as CatalogueIcon;
            container.AddChild(clonedIcon);

            //CatalogueInfoPackage package = new CatalogueInfoPackage(dictionary.ElementAt(i).Value);
            clonedIcon.Initialize(packageList[i]);
        }
    }

    private void ShowContainer(Control inputContainer)
    {
        MaladyContainer.Hide();
        MedicineContainer.Hide();
        TagContainer.Hide();
        inputContainer.Show();
    }
}
