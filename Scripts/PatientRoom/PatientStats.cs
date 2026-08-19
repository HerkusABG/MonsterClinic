using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class PatientStats
{
	// This class is used for storing the patient's data inside of the patient admission interface.
	// This will later be plugged in a way where this gets instantiated every time there is a new patient to be admitted.
	// The relevant stats will be changed according to the game designer's wishes.

	//Patients ID
	public string patientID;
	public int age;
	public Color PortraitColor;

	// Also defining a bool that tracks if the patient is alive, in case he gets SHOT
	public bool isAlive;

	public Malady malady;
	private Room myRoom;

	int dialogueIndexInner = 0;
    int dialogueIndexOuter = 0;

	int pulseIndex = 0;

	int temperatureIndex = 0;

    public TextureUnit textureUnit;

	public List<ClinicAction> clinicActions = new List<ClinicAction>();
    public string[] firstNameArray = { "John", "Jane", "Alex", "Emily", "Michael", "Sarah", "David", "Olivia", "Daniel", "Sophia", "James", "Scott", "Andrew", "Frank", "Gregory", "Ava", "Charlotte", "Sofia", "Camila", "Harper", "Taylor" };
    public string[] lastNameArray = { "Smith", "Johnson", "Williams", "Smith", "Jones", "Miller", "Davis", "Garcia", "Miller", "Martinez", "Hernandez", "Lopez", "Gonzalez", "Wilson", "Anderson", "Thomas", "Clark", "Moore", "Jackson", "Martin", "Lee", "Harris" };
    public string firstName;
    public string lastName;
    public PatientStats()
	{
		// refresh the patient's data.
		// For just assigning random numbers, this will be overhauled later.
		textureUnit = new TextureUnit();
		textureUnit.Initialize();

		Random rnd = new Random();
		malady = new Malady();
        dialogueIndexInner = 0;
        dialogueIndexOuter = 0;
        AssignMaladyValues(MaladyList.Database.ElementAt(rnd.Next(2, 8)).Value);
        //AssignMaladyValues(MaladyList.Database.ElementAt(rnd.Next(4, 5)).Value);
        textureUnit.unitType = malady.layerType;
		if (malady.layerType == TextureType.Normal || malady.layerType == TextureType.Top)
		{
			textureUnit.SaveMaladySet(malady.visualKey, malady.layerType);
		}
		if (malady.severity == -1)
		{
			//malady.severity = rnd.Next(2, 5);
            malady.severity = rnd.Next(2, 3);
        }
		isAlive = true;
		patientID = rnd.Next(1, 1000).ToString("D3");//  "D3" writes the ID as a 3-digit string  005 
		age = rnd.Next(18, 91); // random ages of patients between 18 and 90 seemed appropriate for the game
        firstName = firstNameArray[rnd.Next(0, firstNameArray.Length)]; // assigns random firstname from the array. 0 to the length of all the names in the array
        lastName = lastNameArray[rnd.Next(0, lastNameArray.Length)]; // assigns random lastname from the array. 0 to the length of all the names in the array.


        // Assigning a random color to the patient's portrait, This will be changed later when we have actual portraits.
        PortraitColor = new Color(
			1,
			1,
			1
		);
		clinicActions.Clear();
		NewClinicAction(ClinicActionList.Actions["Admitted"].output);
	}

	private void AssignMaladyValues(Malady inputMalady)
	{
		//ALL malady related information must go through here,
		//otherwise the malady reference is static and curing one patient cures all patients.
		malady = inputMalady.Clone();
	}

	public bool TryCurePatient(Medicine inputMedicine)
	{
		bool isSuccessful = malady.cures.Contains(inputMedicine);
		if (isSuccessful)
		{
			malady.severity--;
			//TriggerInteractionTags();
		}
		return isSuccessful;
	}

	public bool IsPatientCured()
	{
		return malady.severity <= 1;
	}
	public string GetDialogue()
	{
		//Grab generic dialogue.
		/*if (malady.dialogueSymptoms.Count > 0)
		{
			Random rnd = new Random();
			int length = malady.dialogueSymptoms.Count;
			int symptomId = rnd.Next(0, length);
			int quoteListLength = malady.dialogueSymptoms[symptomId].quotes.Count;
			string returnDialogue = malady.dialogueSymptoms[symptomId].quotes[rnd.Next(0, quoteListLength)];
			return returnDialogue;
		}*/
		if (malady.dialogueSymptoms.Count > 0)
		{
            string returnDialogue = malady.dialogueSymptoms[dialogueIndexInner].quotes[dialogueIndexOuter];

			//if(malady.dialogueSymptoms[dialogueIndexInner].quotes.Count > 0)
			if(malady.dialogueSymptoms.Count > 1)
			{
				if(dialogueIndexInner + 1 >= malady.dialogueSymptoms.Count)
				{
					dialogueIndexInner = 0;
                    if (dialogueIndexOuter + 1 >= malady.dialogueSymptoms[dialogueIndexInner].quotes.Count)
                    {
                        dialogueIndexOuter = 0;
                    }
                    else
                    {
                        dialogueIndexOuter++;
                    }
                }
				else
				{
                    dialogueIndexInner++;
                   /* if (dialogueIndexOuter + 1 >= malady.dialogueSymptoms[dialogueIndexInner].quotes.Count)
					{
						GD.Print($"Outer plus one is {dialogueIndexOuter + 1}, count is {malady.dialogueSymptoms[dialogueIndexInner].quotes.Count}");
						dialogueIndexOuter = 0;
                    }
                    else
                    {
                        dialogueIndexOuter++;
                    }*/
                }
            }

            return returnDialogue;
        }
        return "...";
	}
	public virtual string GetAdmittedDialogue()
	{
		if (malady.admittedDialogue.Count > 0)
		{
			Random rnd = new Random();
			int length = malady.admittedDialogue.Count;
			string returnDialogue = malady.admittedDialogue[rnd.Next(0, length)];
			return returnDialogue;
		}
		return "...";
	}
	public string GetPulse()
	{
		//Grab stethoscope dialogue
		if (malady.pulseSymptoms.Count > 0)
		{
			string returnDialogue = malady.pulseSymptoms[0].quotes[pulseIndex];
			if(pulseIndex + 1 >= malady.pulseSymptoms[0].quotes.Count)
			{
				pulseIndex = 0;
			}
			else
			{
				pulseIndex++;
			}
			return returnDialogue;
		}
		return "A nice steady rhythm.";
	}
	public string GetTemperature()
	{
		//Grab temperature dialogue
		if (malady.temperatureSymptoms.Count > 0)
		{
			string returnDialogue = malady.temperatureSymptoms[0].quotes[temperatureIndex];
            if (temperatureIndex + 1 >= malady.temperatureSymptoms[0].quotes.Count)
            {
                temperatureIndex = 0;
            }
            else
            {
                temperatureIndex++;
            }
            return returnDialogue;
		}
		return "Not too hot, not too cold!";
	}

	public void TriggerDailyTags()
	{
		NewDayClinicAction();
		foreach (Tag tag in malady.tags)
		{
			if (tag.HasTagType(TagType.Daily))
			{
				tag.ExecuteDaily(this);
			}
		}
		CheckLifeStatus();
	}
	public void NewDayClinicAction()
	{
		ClinicAction action = new ClinicAction();
		action.output = $"---Day {GlobalData.Player_Ingame_Days}---";
		clinicActions.Add(action);
	}
	public void NewClinicAction(string input)
	{
		ClinicAction action = new ClinicAction();
		action.output = $"{input}";
		clinicActions.Add(action);
	}

	public void NewClinicAction(string input, string extraInfo)
	{
		ClinicAction action = new ClinicAction();
		action.output = $"{input} {extraInfo}";
		clinicActions.Add(action);
	}
	public void NewClinicAction(string input, string extraInfo, string result)
	{
		ClinicAction action = new ClinicAction();
		action.output = $"{input}{extraInfo}{result}";
		clinicActions.Add(action);
	}

	public void TriggerInteractionTags()
	{
		
		foreach (Tag tag in malady.tags)
		{
			if (tag.HasTagType(TagType.Interaction))
			{
				tag.ExecuteInteraction(this);
			}
		}
		CheckLifeStatus();
	}

	public void ShowCorrectMedicineDialogue(SpeechManager speechManager)
	{
		int length = Quotes.Database["CorrectMedicine"].Count;
		List<string> list = Quotes.Database["CorrectMedicine"];

		Random rnd = new Random();
		speechManager.SpeechText(list[rnd.Next(0, length)]);
	}

	public void ShowIncorrectMedicineDialogue(SpeechManager speechManager)
	{
		int length = Quotes.Database["IncorrectMedicine"].Count;
		List<string> list = Quotes.Database["IncorrectMedicine"];

		Random rnd = new Random();
		speechManager.SpeechText(list[rnd.Next(0, length)]);
	}

	private void CheckLifeStatus()
	{
		if (malady.severity <= 1)
		{
			myRoom.PatientCuredInAbsence();
		}
		foreach (Tag tag in malady.tags)
		{
			if (tag.HasTagType(TagType.MaxSeverity))
			{
				tag.ExecuteMaxSeverity(this);
			}
		}
	}

	public bool IsPatientAlive()
	{
		return isAlive;
	}

	public void KillPatient()
	{
		NewClinicAction(ClinicActionList.Actions["Dead"].output);
		isAlive = false;
	}

	public void AssignRoom(Room room)
	{
		myRoom = room;
	}

	public TextureUnit GetPatientTextures()
	{
		return textureUnit;
	}

	public void GivePayout()
	{
        Economy.GiveDailyEarnings(malady.payout);
		FinanceInfo.SaveFinanceInfo(malady.payout, malady.name, 1);
    }
	public void AddSeverity()
	{
		malady.severity++;
	}
}
	

   
