using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using static System.Net.Mime.MediaTypeNames;


public partial class Contents_C : ExpNode2D
{
    //Storing a reference to all the buttons, labels, etc., for easy reference in the methods
    BaseButton DealerButton;
    Button MapButton;
    BaseButton CatalogueButton;
    BaseButton LogOutButton;
    Control DealerWindow;
    TextureButton UpgradesButton;
    Label UpgradesWindow;
    VBoxContainer UpgradesList;
    Button CloseUpgrades;
    TextureButton TreatmentResourcesButton;
    Label ResourcesWindow;
    Button CloseResources;
    TextureButton CloseDealerWindowButton;
    Label DealerWindowMoneyDisplay;
    VBoxContainer MedicineContainer;
    GridContainer RoomContainer;

    TextureButton SpecialOffersButton;
    Label SpecialOffersWindow;
    VBoxContainer SpecialOffersList;

    TextureButton BodyDisposalButton;
    TextureButton SelfTreatmentButton;
    //Label InsufficientFunds;
    //Button CloseFundsPopup;
    //Label InsufficientAvailability;
    //Button CloseInsufficientStockPopup;
    Control MapControl;
    Button CloseMapWindow;
    CatalogueManager CatalogueWindow;
    TextureButton CloseCatalogueWindow;

    Label PurchaseInfo;
    TextureButton PurchaseButton;

    [Export] MapUI mapUi;
    [Export] TextureButton UpButtonDealer;
    [Export] TextureButton DownButtonDealer;

    [Export] TextureButton UpButtonUpgrades;
    [Export] TextureButton DownButtonUpgrades;

    [Export] Popup Popup;

    List<DealerButton> DealerButtons = new List<DealerButton>();
    List<DealerButton> UpgradeButtons = new List<DealerButton>();

    int dealerStartingIndex = 0;
    int upgradeStartingIndex = 0;

    [Export] PackedScene Transition = ResourceLoader.Load<PackedScene>("res://fade_animation.tscn");
    DealerButton PurchaseButtonHolder;
    string PurchaseMode;

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
        DealerButton = control.GetNode<BaseButton>("Dealer");
        MapButton = control.GetNode<Button>("Map");
        CatalogueButton = control.GetNode<BaseButton>("Malady_Catalogue");
        LogOutButton = control.GetNode<BaseButton>("Log_out");

        //separate section for everything in the dealer window
        DealerWindow = control.GetNode<Control>("Dealer_PH");

        UpgradesButton = DealerWindow.GetNode<TextureButton>("Upgrades_Button");
        UpgradesWindow = DealerWindow.GetNode<Label>("Upgrades_Window");
        UpgradesList = UpgradesWindow.GetNode<VBoxContainer>("Upgrades_List");

        SpecialOffersButton = DealerWindow.GetNode<TextureButton>("Special_Offers_Button");
        SpecialOffersWindow = DealerWindow.GetNode<Label>("Special_Offers_Window");
        SpecialOffersList = SpecialOffersWindow.GetNode<VBoxContainer>("Special_Offers_List");
        SelfTreatmentButton = SpecialOffersList.GetNode<TextureButton>("SelfTreatment");
        BodyDisposalButton = SpecialOffersList.GetNode<TextureButton>("BodyDisposal");

        CloseUpgrades = UpgradesWindow.GetNode<Button>("Close");
        TreatmentResourcesButton = DealerWindow.GetNode<TextureButton>("Treatment_Resources_Button");
        ResourcesWindow = DealerWindow.GetNode<Label>("Resources_Window");
        CloseResources = ResourcesWindow.GetNode<Button>("Close");
        CloseDealerWindowButton = DealerWindow.GetNode<TextureButton>("Close");
        DealerWindowMoneyDisplay = DealerWindow.GetNode<Label>("Money_Display");
        MedicineContainer = ResourcesWindow.GetNode<VBoxContainer>("VBoxContainer");
        
        //seperate section for the map window
        MapControl = control.GetNode<MapUI>("MapControl");
        CloseMapWindow = MapControl.GetNode<Button>("Close");
        RoomContainer = MapControl.GetNode<MarginContainer>("MapMarginContainer").GetNode<GridContainer>("RoomContainer");

        //separate section for the malady catalogue
        CatalogueWindow = control.GetNode<CatalogueManager>("Malady_PH");
        CloseCatalogueWindow = CatalogueWindow.GetNode<TextureButton>("Close");

        PurchaseInfo = DealerWindow.GetNode<Label>("Purchase_Info");
        PurchaseButton = DealerWindow.GetNode<TextureButton>("Purchase_Button");

        int count = 0;
        foreach(TextureButton button in MedicineContainer.GetChildren())
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
        foreach (TextureButton button in UpgradesList.GetChildren())
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


        BodyDisposalButton.Pressed += BodyDisposalInfo;
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
        //CloseFundsPopup.Pressed += () => CloseParent(CloseFundsPopup);
        SelfTreatmentButton.Pressed += SelfTreatmentInfo;
        CloseMapWindow.Pressed += () => CloseParent(CloseMapWindow);
        CloseMapWindow.Pressed += mapUi.OnMapUiClose;
        CloseCatalogueWindow.Pressed += () => CloseParent(CloseCatalogueWindow);
        PurchaseButton.Pressed += Purchase;


        foreach (DealerButton button in DealerButtons)
        {
            Action handler = () => ConnectPurchaseToMedicine(button);

            button.Pressed += handler;
        }

        foreach (DealerButton button in UpgradeButtons)
        {
            Action handler = () => ConnectPurchaseToUpgrade(button);

            button.Pressed += handler;
        }
    }
    private void ConnectPurchaseToMedicine(DealerButton button)
    {
        PurchaseButtonHolder = button;
        PurchaseMode = "medicine";
        PurchaseButton.Show();
        if (button.unavailable)
        {
            PurchaseButton.Disabled = true;
        }
        else
        {
            PurchaseButton.Disabled = false;
        }
    }

    private void ConnectPurchaseToUpgrade(DealerButton button)
    {
        PurchaseButtonHolder = button;
        PurchaseMode = "upgrade";
        PurchaseButton.Show();
        if (button.unavailable)
        {
            PurchaseButton.Disabled = true;
        } else
        {
            PurchaseButton.Disabled = false;
        }
    }

    private void Purchase()
    {
        if (PurchaseMode == "medicine")
        {
            PurchaseMedicine(PurchaseButtonHolder);
        }
        else if (PurchaseMode == "upgrade")
        {
            PurchaseUpgrade(PurchaseButtonHolder);
        } else if (PurchaseMode == "self")
        {
            BuyMedicine(PurchaseButtonHolder);
        } else if (PurchaseMode == "body")
        {
            BodyDisposal();
        }
    }

    private void PurchaseMedicine(DealerButton button)
    {
        int myIndex = button.index;
        DealerSlot slot = DealerList.MedicineDatabase.ElementAt(myIndex + dealerStartingIndex).Value;
        if(slot.BuyMedicine())
        {
            RefreshDealerButtons(dealerStartingIndex, DealerButtons);
            DealerWindowMoneyDisplay.Text = DoctorInventory.Money.ToString();
        }
        else
        {
            ShowInsufficientFunds();
        }
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
            UpButtonDealer.Modulate = new Color(1, 1, 1, (float)0.5);
        }
        else if(dealerStartingIndex + DealerButtons.Count >= DealerList.MedicineDatabase.Count)
        {
            DownButtonDealer.Disabled = true;
            DownButtonDealer.Modulate = new Color(1, 1, 1, (float)0.5);
        }
        else
        {
            UpButtonDealer.Disabled = false;
            UpButtonDealer.Modulate = new Color(1, 1, 1, 1);
            DownButtonDealer.Disabled = false;
            DownButtonDealer.Modulate = new Color(1, 1, 1, 1);
        }
    }

    private void UpgradeMenuNavigation(int input)
    {
        upgradeStartingIndex += input;
        RefreshUpgradeButtons(upgradeStartingIndex, UpgradeButtons);
        if (upgradeStartingIndex == 0)
        {
            UpButtonUpgrades.Disabled = true;
            UpButtonUpgrades.Modulate = new Color(1, 1, 1, (float)0.5);
        }
        else if (upgradeStartingIndex + UpgradeButtons.Count >= DealerList.UpgradeDatabase.Count)
        {
            DownButtonUpgrades.Disabled = true;
            DownButtonUpgrades.Modulate = new Color(1, 1, 1, (float)0.5);
        }
        else
        {
            UpButtonUpgrades.Disabled = false;
            UpButtonUpgrades.Modulate = new Color(1, 1, 1, 1);
            DownButtonUpgrades.Disabled = false;
            DownButtonUpgrades.Modulate = new Color(1, 1, 1, 1);
        }
    }

    private void RefreshDealerButtons(int start, List<DealerButton> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            //list[i].Text = DealerList.MedicineDatabase.ElementAt(i + start).Value.GetSlotText();
            //grab the text for the button
            list[i].ChangeText("Buy " + DealerList.MedicineDatabase.ElementAt(i + start).Value.GetSlotName());
            //grab the text for the info box
            if (DealerList.MedicineDatabase.ElementAt(i + start).Value.medicine.unlocked)
            {
                list[i].StoreInfo(DealerList.MedicineDatabase.ElementAt(i + start).Value.GetSlotText());
            } else
            {
                list[i].StoreInfo(DealerList.MedicineDatabase.ElementAt(i + start).Value.GetSlotText() + "\n(Not unlocked yet)");
            }
            //when buying. if the current button in the loop is the one whose info is currently displayed in the info box, update the text in it
            //can't compare the whole strings because the quantity of medicine at the end is different, so we just compare enough of it to confirm it's a match
            if (string.Compare(PurchaseInfo.Text, 0, DealerList.MedicineDatabase.ElementAt(i + start).Value.GetSlotText(), 0, 10) == 0)
            {
                list[i].UpdateInfo();
            }
            if (!DealerList.MedicineDatabase.ElementAt(i + start).Value.medicine.unlocked)
            {
                list[i].unavailable = true;
            }
            else
            {
                list[i].unavailable = false;
            }
        }
    }
    private void RefreshUpgradeButtons(int start, List<DealerButton> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            //list[i].Text = DealerList.UpgradeDatabase.ElementAt(i + start).Value.GetSlotText();
            list[i].ChangeText(DealerList.UpgradeDatabase.ElementAt(i + start).Value.GetSlotName());
            list[i].StoreInfo(DealerList.UpgradeDatabase.ElementAt(i + start).Value.GetSlotText());
            if (DealerList.UpgradeDatabase.ElementAt(i + start).Value.upgrade.fullyUnlocked && list[i].index == PurchaseButtonHolder.index)
            {
                list[i].unavailable = true;
                PurchaseButton.Disabled = true;
            }
        }
    }
    private void InitializeChildren()
    {
        MapUI mapUI = MapControl as MapUI;
        mapUI.Initialize();
        Popup.Initialize();

        CatalogueWindow.Initialize();
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
        DealerWindow.Hide();
        ResourcesWindow.Hide();
        UpgradesWindow.Hide();
        SpecialOffersWindow.Hide();
        GlobalData.PreviousScenes.Pop();

        //RoomTracker.EnterRoom(ActiveRoom.Office);
        RoomTracker.GoBack();
        // show Dialog in the office, if the dialog didnt ended.
        var DialogScene = (Control)GetParent().GetNode("Dialog");
        if (GlobalData.Dialog_Dealer == true)
        {
            DialogScene.Show();
        }
        else
        {
            DialogScene.Hide();
        }
    }

    //universal method for closing a node's parent, used for all the x's in the top right of popups
    private void CloseParent(BaseButton button)
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
    private void OpenResourcesWindow()
    {
        UpdateMoneyDisplay();
        UpgradesWindow.Hide();
        ResourcesWindow.Show();
        SpecialOffersWindow.Hide();
        PurchaseInfo.Text = "";
        PurchaseButton.Hide();
        DealerMenuNavigation(0);
    }
    private void OpenUpgradesWindow()
    {
        UpdateMoneyDisplay();
        ResourcesWindow.Hide();
        UpgradesWindow.Show();
        SpecialOffersWindow.Hide();
        PurchaseInfo.Text = "";
        PurchaseButton.Hide();
        UpgradeMenuNavigation(0);
    }

    private void OpenSpecialOffersWindow()
    {
        UpdateMoneyDisplay();
        ResourcesWindow.Hide();
        UpgradesWindow.Hide();
        SpecialOffersWindow.Show();
        PurchaseInfo.Text = "";
        PurchaseButton.Hide();
        UpdateBodyDisposalInfo();
        SelfTreatmentButton.GetNode<Label>("DealerLabel").Text = "Self Treatment";
    }


    private void SelfTreatmentInfo()
    {
        PurchaseInfo.Text = "Self Treatment  (Price:" + GlobalData.MedicineCost + ") \n" +
            "Owned: " + GlobalData.MedicinePlayer.ToString() + "\n" +
            "Availability in: " + GlobalData.Medicincavailability.ToString() + "\n \n"
            + "Purchase self treatment, which you must buy in order to survive. This will allow you to extend your lifestpan by 5 days.";
        PurchaseButtonHolder = SelfTreatmentButton as DealerButton;
        PurchaseMode = "self";
        PurchaseButton.Show();
    }

    private void ShowInsufficientFunds()
    {
        //InsufficientFunds.Show();
        Popup.DisplayPopup(PopupMessages.ComputerMessages["NoMoney"]);
    }

    private void UpdateMoneyDisplay()
    {
        DealerWindowMoneyDisplay.Text = DoctorInventory.Money.ToString();
    }
    
    private void UpdateBodyDisposalInfo()
    {
        BodyDisposalInfo();
    }

    private void BodyDisposalInfo()
    {
        int count = RoomManager.GetDeadPatientCount();
        int cost = Economy.bodyDisposalCost * count;
        PurchaseInfo.Text = $"Dispose of {count} dead patients. \n Price: {cost}";
        BodyDisposalButton.Disabled = count <= 0;
        PurchaseMode = "body";
        PurchaseButton.Show();
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
        UpdateBodyDisposalInfo();
    }
    private void BuyMedicine(TextureButton button)
    {
        if (DoctorInventory.Money >= GlobalData.MedicineCost && GlobalData.Medicincavailability <= 0)
        {
            // Money deduction, player gets the medicine and the cost of the medicine gets increased (probally needs balancing)
            DoctorInventory.Money -= GlobalData.MedicineCost;
            GlobalData.MedicinePlayer++;
            GlobalData.Dialog_Dealer = true;
            GlobalData.MedicineCost = GlobalData.MedicineCost * 2; // Increase the cost for the next purchase
            PurchaseInfo.Text = "Self Treatment  (Price:" + GlobalData.MedicineCost + ") \n" +
            "Owned: " + GlobalData.MedicinePlayer.ToString() + "\n" +
            "Availability in: " + GlobalData.Medicincavailability.ToString();
            UpdateMoneyDisplay();
        }
        else if (DoctorInventory.Money < GlobalData.MedicineCost)
        {
            //ShowInsufficientFunds();
            Popup.DisplayPopup(PopupMessages.ComputerMessages["NoMoney"]);
        }
        else
        {
            Popup.DisplayPopup(PopupMessages.ComputerMessages["NoMedicine"]);
        }
       
    }

    public override void OnRoomEnter(Node mainNode)
    {
        TriggerFading();
        //GD.Print("Entering computer");
    }

    public override void OnRoomExit()
    {
        //GD.Print("Exiting computer");
    }

    private void TriggerFading()
    {
        // instantiate the scene FadeAnimation
        var fading = Transition.Instantiate<FadeAnimation>();
        // add the scene FadeAnimation and call the Methode Fades
        AddChild(fading);
        fading.Fades();
    }
}
