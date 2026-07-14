using Godot;
using System;

public partial class MedicineButton : TextureButton
{

    Label MedName;
    Label MedCount;
    public void Initialize()
	{
        MedName = GetNode<Label>("Name");
        MedCount = GetNode("Stripe").GetNode<Label>("Count");
    }

    public void RenderText(Medicine inputMedicine)
    {
        MedName.Text = $"{inputMedicine.name}";
        MedCount.Text = $"{inputMedicine.amount}";
    }
}
