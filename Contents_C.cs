using Godot;
using System;
using static System.Net.Mime.MediaTypeNames;

public partial class Contents_C : Node2D
{
    //Storing a reference to all the buttons, labels, etc., for easy reference in the methods
    Button DealerButton;
    Button MapButton;
    Button CatalogueButton;
    Button LogOutButton;
    Button MoneyAdderButton;
    Button MoneySubtractorButton;
    Label MoneyDisplay;
    Label DealerWindow;
    Button CloseDealerWindowButton;
    Label DealerWindowMoneyDisplay;
    HBoxContainer MedicineContainer;
    Button BuyMedicine1Button;
    Button BuyMedicine2Button;
    Button BuyMedicine3Button;
    Button SelfTreatmentButton;
    Label InsufficientFunds;
    Button CloseFundsPopup;
    Label InsufficientAvailability;
    Button CloseInsufficientStockPopup;
    Label MapWindow;
    Button CloseMapWindow;
    Label CatalogueWindow;
    Button CloseCatalogueWindow;



    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //grabs references to all the necessary nodes
        GetNodes();

        //assigning methods to all the buttons
        DealerButton.Pressed += ShowDealerWindow;
        MapButton.Pressed += ShowMapWindow;
        CatalogueButton.Pressed += ShowCatalogueWindow;
        LogOutButton.Pressed += LogOut;
        MoneyAdderButton.Pressed += AddMoney;
        MoneySubtractorButton.Pressed += SubtractMoney;
        //yes this looks kinda wacky, but apparently that's how I gotta write it if I want to have methods that take arguments
        CloseDealerWindowButton.Pressed += () => CloseParent(CloseDealerWindowButton);
        BuyMedicine1Button.Pressed += () => BuyMedicine(BuyMedicine1Button);
        BuyMedicine2Button.Pressed += () => BuyMedicine(BuyMedicine2Button);
        BuyMedicine3Button.Pressed += () => BuyMedicine(BuyMedicine3Button);
        CloseFundsPopup.Pressed += () => CloseParent(CloseFundsPopup);
        SelfTreatmentButton.Pressed += () => BuyMedicine(SelfTreatmentButton);
        CloseMapWindow.Pressed += () => CloseParent(CloseMapWindow);
        CloseCatalogueWindow.Pressed += () => CloseParent(CloseCatalogueWindow) ;

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
        MoneyAdderButton = control.GetNode<Button>("Money_Adder");
        MoneySubtractorButton = control.GetNode<Button>("Money_Subtractor");
        MoneyDisplay = control.GetNode<Label>("Money_Display");

        //separate section for everything in the dealer window
        DealerWindow = control.GetNode<Label>("Dealer_PH");
        CloseDealerWindowButton = DealerWindow.GetNode<Button>("Close");
        DealerWindowMoneyDisplay = DealerWindow.GetNode<Label>("Money_Display");
        MedicineContainer = DealerWindow.GetNode<HBoxContainer>("HBoxContainer");
        BuyMedicine1Button = MedicineContainer.GetNode<Button>("Medicine1");
        BuyMedicine2Button = MedicineContainer.GetNode<Button>("Medicine2");
        BuyMedicine3Button = MedicineContainer.GetNode<Button>("Medicine3");
        SelfTreatmentButton = MedicineContainer.GetNode<Button>("SelfTreatment");
        InsufficientFunds = DealerWindow.GetNode<Label>("Insufficient_Funds");
        CloseFundsPopup = InsufficientFunds.GetNode<Button>("Close");
        InsufficientAvailability = DealerWindow.GetNode<Label>("Insufficient_Availability");
        CloseInsufficientStockPopup = InsufficientAvailability.GetNode<Button>("Close_IA");

        //seperate section for the map window
        MapWindow = control.GetNode<Label>("Map_PH");
        CloseMapWindow = MapWindow.GetNode<Button>("Close");

        //separate section for the malady catalogue
        CatalogueWindow = control.GetNode<Label>("Malady_PH");
        CloseCatalogueWindow = CatalogueWindow.GetNode<Button>("Close");

    }

    private void ShowDealerWindow()
    {
        DealerWindow.Show();
    }

    private void ShowMapWindow()
    {
        MapWindow.Show();
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

    //add money, update display
    private void AddMoney()
    {
        DoctorInventory.Money += 10;
        MoneyDisplay.Text = DoctorInventory.Money.ToString();

        //GlobalData.Money = GlobalData.Money + 10;
        //MoneyDisplay.Text = GlobalData.Money.ToString();
    }
    //subtract money, update display
    private void SubtractMoney()
    {
        DoctorInventory.Money -= 10;
        MoneyDisplay.Text = DoctorInventory.Money.ToString();
        //GlobalData.Money = GlobalData.Money - 10;
        //MoneyDisplay.Text = GlobalData.Money.ToString();
    }
    //update display whenever the display's visibility changes (this is about as infrequently as I can easily make it update while still always being up to date
    private void _on_money_display_visibility_changed()
    {
        MoneyDisplay.Text = DoctorInventory.Money.ToString();
        //MoneyDisplay.Text = GlobalData.Money.ToString(); 
    }
    //universal method for closing a node's parent, used for all the x's in the top right of popups
    private void CloseParent(Button button)
    {
        var Parent = (Label)button.GetParent();
        Parent.Hide();
    }
    //whenever the dealer window's visibility changes, update the text on the money display and the purchase buttons
    private void _on_dealer_ph_visibility_changed()
    {
        MoneyDisplay.Text = DoctorInventory.Money.ToString();
        DealerWindowMoneyDisplay.Text = DoctorInventory.Money.ToString();

        BuyMedicine1Button.Text = $"{MedicineManager.Database["Morphine"].name} \n (Price: {MedicineManager.Database["Morphine"].cost}) \n \n Owned: {MedicineManager.Database["Morphine"].amount}";
        BuyMedicine2Button.Text = $"{MedicineManager.Database["Aspirin"].name} \n (Price: {MedicineManager.Database["Aspirin"].cost}) \n \n Owned: {MedicineManager.Database["Aspirin"].amount}";
        BuyMedicine3Button.Text = $"{MedicineManager.Database["Ozempic"].name} \n (Price: {MedicineManager.Database["Ozempic"].cost}) \n \n Owned: {MedicineManager.Database["Ozempic"].amount}";

        //BuyMedicine1Button.Text = "gagagugi \n (Price: 10) \n \n Owned: " + GlobalData.Medicine1Count.ToString();
        //BuyMedicine2Button.Text = "Medicine 2 \n (Price: 20) \n \n Owned: " + GlobalData.Medicine2Count.ToString();
        //BuyMedicine3Button.Text = "Medicine 3 \n (Price: 30) \n \n Owned: " + GlobalData.Medicine3Count.ToString();
        SelfTreatmentButton.Text = "Self Treatment \n (Price:" + GlobalData.MedicineCost + ")\n \n Owned: " + GlobalData.MedicinePlayer.ToString() + " \n availability in: " + GlobalData.Medicincavailability.ToString();
    }
    //semi-modular method for buying every type of medicine
    private void BuyMedicine(Button button)
    {
        if (button == BuyMedicine1Button)
        {
            //if you can afford it, subtract the price from your money, add it to your inventory, and update the text
            if (DoctorInventory.Money >= MedicineManager.Database["Morphine"].cost)
            {
                DoctorInventory.Money -= MedicineManager.Database["Morphine"].cost;
                MedicineManager.Database["Morphine"].amount++;
                button.Text = $"{MedicineManager.Database["Morphine"].name} \n (Price: {MedicineManager.Database["Morphine"].cost}) \n \n Owned: {MedicineManager.Database["Morphine"].amount}";
                DealerWindowMoneyDisplay.Text = DoctorInventory.Money.ToString();




                //if you can't afford it, give em the poor idiot popup
            }
            else
            {
                InsufficientFunds.Show();
            }
        }
        else if (button == BuyMedicine2Button)
        {
            //if you can afford it, subtract the price from your money, add it to your inventory, and update the text
            if (DoctorInventory.Money >= MedicineManager.Database["Aspirin"].cost)
            {
                DoctorInventory.Money -= MedicineManager.Database["Aspirin"].cost;
                MedicineManager.Database["Aspirin"].amount++;
                //GlobalData.Medicine2Count += 1;
                button.Text = $"{MedicineManager.Database["Aspirin"].name} \n (Price: {MedicineManager.Database["Aspirin"].cost}) \n \n Owned: {MedicineManager.Database["Aspirin"].amount}";
                DealerWindowMoneyDisplay.Text = DoctorInventory.Money.ToString();
            }
            //if you can't afford it, give em the poor idiot popup
            else
            {
                InsufficientFunds.Show();
            }
        }
        else if (button == BuyMedicine3Button)
        {
            //if you can afford it, subtract the price from your money, add it to your inventory, and update the text
            if (DoctorInventory.Money >= MedicineManager.Database["Ozempic"].cost)
            {
                DoctorInventory.Money -= MedicineManager.Database["Ozempic"].cost;
                MedicineManager.Database["Ozempic"].amount++;
                //GlobalData.Medicine3Count += 1;
                button.Text = $"{MedicineManager.Database["Aspirin"].name} \n (Price: {MedicineManager.Database["Aspirin"].cost}) \n \n Owned: {MedicineManager.Database["Aspirin"].amount}";
                DealerWindowMoneyDisplay.Text = DoctorInventory.Money.ToString();
            }
            //if you can't afford it, give em the poor idiot popup
            else
            {
                InsufficientFunds.Show();
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
                DealerWindowMoneyDisplay.Text = DoctorInventory.Money.ToString();

            }
            else if (DoctorInventory.Money < GlobalData.MedicineCost)
            {
                InsufficientFunds.Show();
            }
            else
            {
                InsufficientAvailability.Show();
            }
        }
        else
        {
            Console.WriteLine("well this isn't supposed to happen");
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
