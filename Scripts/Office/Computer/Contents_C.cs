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
    Control DealerWindow;
    Button UpgradesButton;
    Label UpgradesWindow;
    VBoxContainer UpgradesList;
    Button CloseUpgrades;
    Button TreatmentResourcesButton;
    Label ResourcesWindow;
    Button CloseResources;
    Button CloseDealerWindowButton;
    Label DealerWindowMoneyDisplay;
    VBoxContainer MedicineContainer;
    GridContainer RoomContainer;

    Button SpecialOffersButton;
    Label SpecialOffersWindow;
    VBoxContainer SpecialOffersList;

    Button BodyDisposalButton;
    Button SelfTreatmentButton;
    Label InsufficientFunds;
    Button CloseFundsPopup;
    Label InsufficientAvailability;
    Button CloseInsufficientStockPopup;
    Control MapControl;
    Button CloseMapWindow;
    Control CatalogueWindow;
    Button CloseCatalogueWindow;

    [Export] MapUI mapUi;
    [Export] Button UpButtonDealer;
    [Export] Button DownButtonDealer;

    [Export] Button UpButtonUpgrades;
    [Export] Button DownButtonUpgrades;

    List<DealerButton> DealerButtons = new List<DealerButton>();
    List<DealerButton> UpgradeButtons = new List<DealerButton>();

    int dealerStartingIndex = 0;
    int upgradeStartingIndex = 0;

    //private readonly Dictionary<DealerButton, Action> Subscriptions = new();


    // Called when the node enters the scene tree for the first time.
    public void Initialize()
    {
        //grabs references to all the necessary nodes
        GetNodes();

        //Assigns methods to button actions
        Subscribe();

        //Initializing any children with their own scripts
        InitializeChildren();

        DealerMenuNavigation(dealerStartingIndex);
        UpgradeMenuNavigation(upgradeStartingIndex);
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
        DealerWindow = control.GetNode<Control>("Dealer_PH");

        UpgradesButton = DealerWindow.GetNode<Button>("Upgrades_Button");
        UpgradesWindow = DealerWindow.GetNode<Label>("Upgrades_Window");
        UpgradesList = UpgradesWindow.GetNode<VBoxContainer>("Upgrades_List");

        SpecialOffersButton = DealerWindow.GetNode<Button>("Special_Offers_Button");
        SpecialOffersWindow = DealerWindow.GetNode<Label>("Special_Offers_Window");
        SpecialOffersList = SpecialOffersWindow.GetNode<VBoxContainer>("Special_Offers_List");
        SelfTreatmentButton = SpecialOffersList.GetNode<Button>("SelfTreatment");
        BodyDisposalButton = SpecialOffersList.GetNode<Button>("BodyDisposal");

        CloseUpgrades = UpgradesWindow.GetNode<Button>("Close");
        TreatmentResourcesButton = DealerWindow.GetNode<Button>("Treatment_Resources_Button");
        ResourcesWindow = DealerWindow.GetNode<Label>("Resources_Window");
        CloseResources = ResourcesWindow.GetNode<Button>("Close");
        CloseDealerWindowButton = DealerWindow.GetNode<Button>("Close");
        DealerWindowMoneyDisplay = DealerWindow.GetNode<Label>("Money_Display");
        MedicineContainer = ResourcesWindow.GetNode<VBoxContainer>("VBoxContainer");
        
        InsufficientFunds = DealerWindow.GetNode<Label>("Insufficient_Funds");
        CloseFundsPopup = InsufficientFunds.GetNode<Button>("Close");
        InsufficientAvailability = DealerWindow.GetNode<Label>("Insufficient_Availability");
        CloseInsufficientStockPopup = InsufficientAvailability.GetNode<Button>("Close_IA");

        //seperate section for the map window
        MapControl = control.GetNode<MapUI>("MapControl");
        CloseMapWindow = MapControl.GetNode<Button>("Close");
        RoomContainer = MapControl.GetNode<MarginContainer>("MapMarginContainer").GetNode<GridContainer>("RoomContainer");

        //separate section for the malady catalogue
        CatalogueWindow = control.GetNode<Control>("Malady_PH");
        CloseCatalogueWindow = CatalogueWindow.GetNode<Button>("Close");

        int count = 0;
        foreach(Button button in MedicineContainer.GetChildren())
        {
            DealerButton castButton = button as DealerButton;
            if(castButton != null)
            {
                DealerButtons.Add(castButton);
                castButton.Initialize();
                castButton.index = count;
                count++;
            }
        }
        count = 0;
        foreach (Button button in UpgradesList.GetChildren())
        {
            DealerButton castButton = button as DealerButton;
            if (castButton != null)
            {
                UpgradeButtons.Add(castButton);
                castButton.Initialize();
                castButton.index = count;
                count++;
            }
        }
    }
    private void Subscribe()
    {
        //assigning methods to all the buttons
        UpButtonDealer.Pressed += () => DealerMenuNavigation(-1);
        DownButtonDealer.Pressed += () => DealerMenuNavigation(1);
        UpButtonUpgrades.Pressed += () => UpgradeMenuNavigation(-1);
        DownButtonUpgrades.Pressed += () => UpgradeMenuNavigation(1);


        BodyDisposalButton.Pressed += BodyDisposal;
        DealerButton.Pressed += ShowDealerWindow;
        MapButton.Pressed += ShowMapWindow;
        CatalogueButton.Pressed += ShowCatalogueWindow;
        LogOutButton.Pressed += LogOut;
        TreatmentResourcesButton.Pressed += OpenResourcesWindow;
        UpgradesButton.Pressed += OpenUpgradesWindow;
        SpecialOffersButton.Pressed += OpenSpecialOffersWindow;
        CloseResources.Pressed += () => CloseParent(CloseResources);
        CloseUpgrades.Pressed += () => CloseParent(CloseUpgrades);
        CloseDealerWindowButton.Pressed += () => CloseParent(CloseDealerWindowButton);
        CloseDealerWindowButton.Pressed += mapUi.OnMapUiClose;
        CloseFundsPopup.Pressed += () => CloseParent(CloseFundsPopup);
        SelfTreatmentButton.Pressed += () => BuyMedicine(SelfTreatmentButton);
        CloseMapWindow.Pressed += () => CloseParent(CloseMapWindow);
        CloseMapWindow.Pressed += mapUi.OnMapUiClose;
        CloseCatalogueWindow.Pressed += () => CloseParent(CloseCatalogueWindow);


        foreach (DealerButton button in DealerButtons)
        {
            Action handler = () => PurchaseMedicine(button);

            button.Pressed += handler;
        }

        foreach (DealerButton button in UpgradeButtons)
        {
            Action handler = () => PurchaseUpgrade(button);

            button.Pressed += handler;
        }
    }
    private void PurchaseMedicine(DealerButton button)
    {
        int myIndex = button.index;
        DealerSlot slot = DealerList.MedicineDatabase.ElementAt(myIndex + dealerStartingIndex).Value;
        slot.BuyMedicine();
        RefreshDealerButtons(dealerStartingIndex, DealerButtons);
        DealerWindowMoneyDisplay.Text = DoctorInventory.Money.ToString();
    }

    private void PurchaseUpgrade(DealerButton button)
    {
        int myIndex = button.index;
        DealerSlot slot = DealerList.UpgradeDatabase.ElementAt(myIndex + upgradeStartingIndex).Value;
        if(slot.type == DealerSlot.SlotType.Boolean)
        {
            BooleanUpgrade upgrade = DealerList.UpgradeDatabase.ElementAt(myIndex + upgradeStartingIndex).Value.upgrade as BooleanUpgrade;
            Upgrades.BooleanUpgrade(upgrade, button, UpdateMoneyDisplay, ShowInsufficientFunds);
        }
        else if(slot.type == DealerSlot.SlotType.Integer)
        {
            IntegerUpgrade upgrade = DealerList.UpgradeDatabase.ElementAt(myIndex + upgradeStartingIndex).Value.upgrade as IntegerUpgrade;
            Upgrades.IntegerUpgrade(upgrade, 1,button, UpdateMoneyDisplay, ShowInsufficientFunds);
        }
        RefreshUpgradeButtons(upgradeStartingIndex, UpgradeButtons);
        DealerWindowMoneyDisplay.Text = DoctorInventory.Money.ToString();
    }

    private void DealerMenuNavigation(int input)
    {
        dealerStartingIndex += input;
        RefreshDealerButtons(dealerStartingIndex, DealerButtons);
        if (dealerStartingIndex == 0)
        {
            UpButtonDealer.Disabled = true;
        }
        else if(dealerStartingIndex + DealerButtons.Count >= DealerList.MedicineDatabase.Count)
        {
            DownButtonDealer.Disabled = true;
        }
        else
        {
            UpButtonDealer.Disabled = false;
            DownButtonDealer.Disabled = false;
        }
    }

    private void UpgradeMenuNavigation(int input)
    {
        upgradeStartingIndex += input;
        RefreshUpgradeButtons(upgradeStartingIndex, UpgradeButtons);
        if (upgradeStartingIndex == 0)
        {
            UpButtonUpgrades.Disabled = true;
        }
        else if (upgradeStartingIndex + UpgradeButtons.Count >= DealerList.UpgradeDatabase.Count)
        {
            DownButtonUpgrades.Disabled = true;
        }
        else
        {
            UpButtonUpgrades.Disabled = false;
            DownButtonUpgrades.Disabled = false;
        }
    }

    private void RefreshDealerButtons(int start, List<DealerButton> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            //list[i].Text = DealerList.MedicineDatabase.ElementAt(i + start).Value.GetSlotText();
            list[i].ChangeText(DealerList.MedicineDatabase.ElementAt(i + start).Value.GetSlotText());
            if (!DealerList.MedicineDatabase.ElementAt(i + start).Value.medicine.unlocked)
            {
                list[i].Disabled = true;
            }
            else
            {
                list[i].Disabled = false;
            }
        }
    }
    private void RefreshUpgradeButtons(int start, List<DealerButton> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            //list[i].Text = DealerList.UpgradeDatabase.ElementAt(i + start).Value.GetSlotText();
            list[i].ChangeText(DealerList.UpgradeDatabase.ElementAt(i + start).Value.GetSlotText());
            if(DealerList.UpgradeDatabase.ElementAt(i + start).Value.upgrade.fullyUnlocked)
            {
                list[i].Disabled = true;
            }
            else
            {
                list[i].Disabled = false;
            }
        }
    }
    private void InitializeChildren()
    {
        MapUI mapUI = MapControl as MapUI;
        mapUI.Initialize();
    }

    

    private void ShowDealerWindow()
    {
        UpdateMoneyDisplay();
        DealerWindow.Show();
        ResourcesWindow.Hide();
        UpgradesWindow.Hide();
        SpecialOffersWindow.Hide();
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
        
    }
    //semi-modular method for buying every type of medicine

    private void OpenResourcesWindow()
    {
        UpdateMoneyDisplay();
        UpgradesWindow.Hide();
        ResourcesWindow.Show();
        SpecialOffersWindow.Hide();
        DealerMenuNavigation(0);
    }
    private void OpenUpgradesWindow()
    {
        UpdateMoneyDisplay();
        ResourcesWindow.Hide();
        UpgradesWindow.Show();
        SpecialOffersWindow.Hide();
        UpgradeMenuNavigation(0);
    }

    private void OpenSpecialOffersWindow()
    {
        UpdateMoneyDisplay();
        ResourcesWindow.Hide();
        UpgradesWindow.Hide();
        SpecialOffersWindow.Show();
        UpdateBodyDisposalButton();
        SelfTreatmentButton.GetNode<Label>("DealerLabel").Text = "Self Treatment  (Price:" + GlobalData.MedicineCost + ") " +
            " Owned: " + GlobalData.MedicinePlayer.ToString() + " " +
            " availability in: " + GlobalData.Medicincavailability.ToString();
    }

    private void ShowInsufficientFunds()
    {
        InsufficientFunds.Show();
    }

    private void UpdateMoneyDisplay()
    {
        DealerWindowMoneyDisplay.Text = "Credits: " + DoctorInventory.Money.ToString();
    }
    
    private void UpdateBodyDisposalButton()
    {
        int count = RoomManager.GetDeadPatientCount();
        int cost = Economy.bodyDisposalCost * count;
        BodyDisposalButton.GetNode<Label>("DealerLabel").Text = $"Dispose of {count} dead patients \n Price: {cost}";
        BodyDisposalButton.Disabled = count <= 0;
    }

    private void BodyDisposal()
    {
        Room[] deadRooms = RoomManager.GetAllDeadPatients();
        int count = RoomManager.GetDeadPatientCount();
        for (int i = 0; i < count; i++)
        {
            GlobalData.patientCount--;
            Room deadRoom = deadRooms[i];
            //Room deadRoom = RoomManager.FindDeadPatient();
            deadRoom.SetAlreadyTreated(false);
            deadRoom.DeletePatient();
        }
        UpdateBodyDisposalButton();
    }
    private void BuyMedicine(Button button)
    {
        GD.Print("buying self treatment");
        if (DoctorInventory.Money >= GlobalData.MedicineCost && GlobalData.Medicincavailability <= 0)
        {
            // Money deduction, player gets the medicine and the cost of the medicine gets increased (probally needs balancing)
            DoctorInventory.Money -= GlobalData.MedicineCost;
            GlobalData.MedicinePlayer++;
            GlobalData.MedicineCost = GlobalData.MedicineCost * 2; // Increase the cost for the next purchase
            button.GetNode<Label>("DealerLabel").Text = "Self Treatment (Price: " + GlobalData.MedicineCost + ") Owned: " + GlobalData.MedicinePlayer.ToString() + ") availability in: " + GlobalData.Medicincavailability.ToString();
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
}
