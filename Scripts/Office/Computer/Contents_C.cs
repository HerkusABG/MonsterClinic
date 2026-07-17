using Godot;
using System;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;
using System.Linq;


public partial class Contents_C : Node2D
{
    //Storing a reference to all the buttons, labels, etc., for easy reference in the methods
    Button DealerButton;
    Button MapButton;
    Button CatalogueButton;
    Button LogOutButton;
    Label DealerWindow;
    Button UpgradesButton;
    Label UpgradesWindow;
    VBoxContainer UpgradesList;
    Button AspirinUnlock;
    Button WaitingRoomUpgrade;
    Button RemoteMedicineUnlock;
    Button CloseUpgrades;
    Button TreatmentResourcesButton;
    Label ResourcesWindow;
    Button CloseResources;
    Button CloseDealerWindowButton;
    Label DealerWindowMoneyDisplay;
    VBoxContainer MedicineContainer;
    GridContainer RoomContainer;

    //Button BuyMedicine1Button;
    //Button BuyMedicine2Button;
    //Button BuyMedicine3Button;

    Button SelfTreatmentButton;
    Label InsufficientFunds;
    Button CloseFundsPopup;
    Label InsufficientAvailability;
    Button CloseInsufficientStockPopup;
    Control MapControl;
    Button CloseMapWindow;
    Label CatalogueWindow;
    Button CloseCatalogueWindow;

    [Export] MapUI mapUi;
    [Export] Button UpButton;
    [Export] Button DownButton;

    List<DealerButton> DealerButtons = new List<DealerButton>();

    int startingIndex = 0;

    private readonly Dictionary<DealerButton, Action> Subscriptions = new();


    // Called when the node enters the scene tree for the first time.
    public void Initialize()
    {
        //grabs references to all the necessary nodes
        GetNodes();

        //Assigns methods to button actions
        Subscribe();

        //Initializing any children with their own scripts
        InitializeChildren();

        DealerMenuNavigation(0);
    }

    private void GetNodes()
    {
        //Basically just grabbing all buttons. I have to reference the control because
        //otherwise they wouldn't be found.
        Control control = GetNode<Control>("Player_Interactables_C");
        DealerButton = control.GetNode<Button>("Dealer");
        MapButton = control.GetNode<Button>("Map");
        CatalogueButton = control.GetNode<Button>("Malady_Catalogue");
        LogOutButton = control.GetNode<Button>("Log_out");

        //separate section for everything in the dealer window
        DealerWindow = control.GetNode<Label>("Dealer_PH");
        UpgradesButton = DealerWindow.GetNode<Button>("Upgrades_Button");
        UpgradesWindow = DealerWindow.GetNode<Label>("Upgrades_Window");
        UpgradesList = UpgradesWindow.GetNode<VBoxContainer>("Upgrades_List");
        AspirinUnlock = UpgradesList.GetNode<Button>("Aspirin_Unlock");
        RemoteMedicineUnlock = UpgradesList.GetNode<Button>("Remote_Medicine_Unlock");
        WaitingRoomUpgrade = UpgradesList.GetNode<Button>("Waiting_Room_Upgrade");
        CloseUpgrades = UpgradesWindow.GetNode<Button>("Close");
        TreatmentResourcesButton = DealerWindow.GetNode<Button>("Treatment_Resources_Button");
        ResourcesWindow = DealerWindow.GetNode<Label>("Resources_Window");
        CloseResources = ResourcesWindow.GetNode<Button>("Close");
        CloseDealerWindowButton = DealerWindow.GetNode<Button>("Close");
        DealerWindowMoneyDisplay = DealerWindow.GetNode<Label>("Money_Display");
        MedicineContainer = ResourcesWindow.GetNode<VBoxContainer>("VBoxContainer");
        //BuyMedicine1Button = MedicineContainer.GetNode<Button>("Medicine1");
        //BuyMedicine2Button = MedicineContainer.GetNode<Button>("Medicine2");
        //BuyMedicine3Button = MedicineContainer.GetNode<Button>("Medicine3");
        SelfTreatmentButton = MedicineContainer.GetNode<Button>("SelfTreatment");
        InsufficientFunds = DealerWindow.GetNode<Label>("Insufficient_Funds");
        CloseFundsPopup = InsufficientFunds.GetNode<Button>("Close");
        InsufficientAvailability = DealerWindow.GetNode<Label>("Insufficient_Availability");
        CloseInsufficientStockPopup = InsufficientAvailability.GetNode<Button>("Close_IA");

        //seperate section for the map window
        MapControl = control.GetNode<MapUI>("MapControl");
        CloseMapWindow = MapControl.GetNode<Button>("Close");
        RoomContainer = MapControl.GetNode<MarginContainer>("MapMarginContainer").GetNode<GridContainer>("RoomContainer");

        //separate section for the malady catalogue
        CatalogueWindow = control.GetNode<Label>("Malady_PH");
        CloseCatalogueWindow = CatalogueWindow.GetNode<Button>("Close");

        int count = 0;
        foreach(Button button in MedicineContainer.GetChildren())
        {
            DealerButton castButton = button as DealerButton;
            if(castButton != null)
            {
                DealerButtons.Add(castButton);
                castButton.index = count;
                count++;
            }
        }
    }

    private void PurchaseMedicine(DealerButton button)
    {
        int myIndex = button.index;
        DealerSlot slot = DealerList.Database.ElementAt(myIndex + startingIndex).Value;
        slot.BuyMedicine();
        RefreshDealerButtons(startingIndex);
        DealerWindowMoneyDisplay.Text = DoctorInventory.Money.ToString();
    }

    private void DealerMenuNavigation(int input)
    {
        startingIndex += input;
        RefreshDealerButtons(startingIndex);
        if (startingIndex == 0)
        {
            UpButton.Disabled = true;
        }
        else if(startingIndex + DealerButtons.Count >= DealerList.Database.Count)
        {
            DownButton.Disabled = true;
        }
        else
        {
            UpButton.Disabled = false;
            DownButton.Disabled = false;
        }
    }


    private void RefreshDealerButtons(int start)
    {
        for (int i = 0; i < DealerButtons.Count; i++)
        {
            DealerButtons[i].Text = DealerList.Database.ElementAt(i + start).Value.GetSlotText();
        }
    }
    private void InitializeChildren()
    {
        MapUI mapUI = MapControl as MapUI;
        mapUI.Initialize();
    }

    private void Subscribe()
    {
        //assigning methods to all the buttons
        UpButton.Pressed += () => DealerMenuNavigation(-1);
        DownButton.Pressed += () => DealerMenuNavigation(1);
        DealerButton.Pressed += ShowDealerWindow;
        MapButton.Pressed += ShowMapWindow;
        CatalogueButton.Pressed += ShowCatalogueWindow;
        LogOutButton.Pressed += LogOut;
        TreatmentResourcesButton.Pressed += OpenResourcesWindow;
        UpgradesButton.Pressed += OpenUpgradesWindow;
        AspirinUnlock.Pressed += UnlockAspirin;
        WaitingRoomUpgrade.Pressed += () => Upgrades.IntegerUpgrade(Upgrades.IntUpgradeDatabase["PatientSlots"], 1, WaitingRoomUpgrade, UpdateMoneyDisplay, ShowInsufficientFunds);
        RemoteMedicineUnlock.Pressed += () => Upgrades.BooleanUpgrade(Upgrades.BoolUpgradeDatabase["RemoteMedicine"], RemoteMedicineUnlock, UpdateMoneyDisplay, ShowInsufficientFunds);
        //yes this looks kinda wacky, but apparently that's how I gotta write it if I want to have methods that take arguments
        CloseResources.Pressed += () => CloseParent(CloseResources);
        CloseUpgrades.Pressed += () => CloseParent(CloseUpgrades);
        CloseDealerWindowButton.Pressed += () => CloseParent(CloseDealerWindowButton);
        //BuyMedicine1Button.Pressed += () => BuyMedicine(BuyMedicine1Button);
        //BuyMedicine2Button.Pressed += () => BuyMedicine(BuyMedicine2Button);
        //BuyMedicine3Button.Pressed += () => BuyMedicine(BuyMedicine3Button);
        CloseFundsPopup.Pressed += () => CloseParent(CloseFundsPopup);
        SelfTreatmentButton.Pressed += () => BuyMedicine(SelfTreatmentButton);
        CloseMapWindow.Pressed += () => CloseParent(CloseMapWindow);
        CloseCatalogueWindow.Pressed += () => CloseParent(CloseCatalogueWindow);


        foreach(DealerButton button in DealerButtons)
        {
            Action handler = () => PurchaseMedicine(button);

            button.Pressed += handler;
        }
    }

    private void ShowDealerWindow()
    {
        DealerWindow.Show();
    }

    private void ShowMapWindow()
    {
        MapControl.Show();
        mapUi.OnMapButtonPressed();
    }

    private void ShowCatalogueWindow()
    {
        CatalogueWindow.Show();
    }

    private void LogOut()
    {
        //when leaving the room, hide it, show the office, and pop the room off the previous scenes stack, to not interfere with the right click functionality
        Hide();
        var OfficeScene = (Node2D)GetParent().GetNode("Office");
        OfficeScene.Show();
        GlobalData.PreviousScenes.Pop();
    }

    //universal method for closing a node's parent, used for all the x's in the top right of popups
    private void CloseParent(Button button)
    {
        var Parent = button.GetParent();
        if(Parent.GetClass() == "Label")
        {
            Label ParentLabel = (Label)Parent;
            ParentLabel.Hide();
        }
        if(Parent.GetClass() == "Control")
        {
            var ControlParent = (Control)Parent;
            ControlParent.Hide();
        }
    }
    //whenever the dealer window's visibility changes, update the text on the money display and the purchase buttons
    private void _on_dealer_ph_visibility_changed()
    {
        UpdateMoneyDisplay();

    }
    //semi-modular method for buying every type of medicine

    private void OpenResourcesWindow()
    {
        UpgradesWindow.Hide();
        ResourcesWindow.Show();
    }
    private void OpenUpgradesWindow()
    {
        ResourcesWindow.Hide();
        UpgradesWindow.Show();
    }

    private void UnlockAspirin()
    {
        //if you can afford it, unlock aspirin, disables the unlock button, and enables the purchase button
        if (DoctorInventory.Money >= 50)
        {
            Upgrades.UnlockAspirin();
            AspirinUnlock.Disabled = true;
            //BuyMedicine2Button.Disabled = false;
            UpdateMoneyDisplay();
        }
        else
        {
            ShowInsufficientFunds();
        }
    }

    private void ShowInsufficientFunds()
    {
        InsufficientFunds.Show();
    }

    private void UpdateMoneyDisplay()
    {
        DealerWindowMoneyDisplay.Text = "Credits: " + DoctorInventory.Money.ToString();
    }
    
    /*private void UnlockRemoteMedicine()
    {
        //if you can afford it, unlocks remote medicine, disables the unlock button, and enables the purchase button
        if (DoctorInventory.Money >= 200)
        {
            Upgrades.UnlockRemoteMedicine();
            RemoteMedicineUnlock.Disabled = true;
            UpdateMoneyDisplay();
        }
        else
        {
            ShowInsufficientFunds();
        }
    }*/
    private void _on_resources_window_visibility_changed()
    {
        if (ResourcesWindow.Visible)
        {
            if (Upgrades.AspirinUnlock == true)
            {
                //BuyMedicine2Button.Disabled = false;
                //BuyMedicine2Button.Text = $"{MedicineManager.Database["Aspirin"].name} \n (Price: {MedicineManager.Database["Aspirin"].cost}) \n \n Owned: {MedicineManager.Database["Aspirin"].amount}";
            }
            else
            {
                //BuyMedicine2Button.Text = $"{MedicineManager.Database["Aspirin"].name} \n (Price: {MedicineManager.Database["Aspirin"].cost}) \n \n Currently unavailable";
            }

            //BuyMedicine1Button.Text = $"{MedicineManager.Database["Morphine"].name} \n (Price: {MedicineManager.Database["Morphine"].cost}) \n \n Owned: {MedicineManager.Database["Morphine"].amount}";
            //BuyMedicine3Button.Text = $"{MedicineManager.Database["Ozempic"].name} \n (Price: {MedicineManager.Database["Ozempic"].cost}) \n \n Owned: {MedicineManager.Database["Ozempic"].amount}";

            SelfTreatmentButton.Text = "Self Treatment \n (Price:" + GlobalData.MedicineCost + ")\n \n Owned: " + GlobalData.MedicinePlayer.ToString() + " \n availability in: " + GlobalData.Medicincavailability.ToString();
        }
    }
    private void BuyMedicine(Button button)
    {
        /* if (button == BuyMedicine1Button)
         {
             //if you can afford it, subtract the price from your money, add it to your inventory, and update the text
             if (DoctorInventory.Money >= MedicineManager.Database["Morphine"].cost)
             {
                 DoctorInventory.Money -= MedicineManager.Database["Morphine"].cost;
                 MedicineManager.Database["Morphine"].amount++;
                 button.Text = $"{MedicineManager.Database["Morphine"].name} \n (Price: {MedicineManager.Database["Morphine"].cost}) \n \n Owned: {MedicineManager.Database["Morphine"].amount}";
                 UpdateMoneyDisplay();

                 //if you can't afford it, give em the poor idiot popup
             }
             else
             {
                 ShowInsufficientFunds();
             }
         }
         else if (button == BuyMedicine2Button)
         {
             //if you can afford it, subtract the price from your money, add it to your inventory, and update the text
             if (DoctorInventory.Money >= MedicineManager.Database["Aspirin"].cost)
             {
                 DoctorInventory.Money -= MedicineManager.Database["Aspirin"].cost;
                 MedicineManager.Database["Aspirin"].amount++;
                 button.Text = $"{MedicineManager.Database["Aspirin"].name} \n (Price: {MedicineManager.Database["Aspirin"].cost}) \n \n Owned: {MedicineManager.Database["Aspirin"].amount}";
                 UpdateMoneyDisplay();
             }
             //if you can't afford it, give em the poor idiot popup
             else
             {
                 ShowInsufficientFunds();
             }
         }
         else if (button == BuyMedicine3Button)
         {
             //if you can afford it, subtract the price from your money, add it to your inventory, and update the text
             if (DoctorInventory.Money >= MedicineManager.Database["Ozempic"].cost)
             {
                 DoctorInventory.Money -= MedicineManager.Database["Ozempic"].cost;
                 MedicineManager.Database["Ozempic"].amount++;
                 button.Text = $"{MedicineManager.Database["Ozempic"].name} \n (Price: {MedicineManager.Database["Ozempic"].cost}) \n \n Owned: {MedicineManager.Database["Ozempic"].amount}";
                 UpdateMoneyDisplay();
             }
             //if you can't afford it, give em the poor idiot popup
             else
             {
                 ShowInsufficientFunds();
             }
         } 
         else if (button == SelfTreatmentButton) 
         {
             // Check if the player has the money and if the medicine is available before allowing him to purchase item
             if (DoctorInventory.Money >= GlobalData.MedicineCost && GlobalData.Medicincavailability <= 0)
             {
                 // Money deduction, player gets the medicine and the cost of the medicine gets increased (probally needs balancing)
                 DoctorInventory.Money -= GlobalData.MedicineCost;
                 GlobalData.MedicinePlayer++;
                 GlobalData.MedicineCost = GlobalData.MedicineCost * 2; // Increase the cost for the next purchase
                 button.Text = "Self Treatment \n (Price: " + GlobalData.MedicineCost + ") \n \n Owned: " + GlobalData.MedicinePlayer.ToString() + ") \n availability in: " + GlobalData.Medicincavailability.ToString();
                 UpdateMoneyDisplay();
             }
             else if (DoctorInventory.Money < GlobalData.MedicineCost)
             {
                 ShowInsufficientFunds();
             }
             else
             {
                 InsufficientAvailability.Show();
             }
         }
         else
         {
             GD.Print("well this isn't supposed to happen");
         }*/
    }
}
