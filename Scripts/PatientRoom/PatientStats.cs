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
	public string[] firstNameArray = { "Ace", "Addie", "Addison", "Adi", "Adrean", "Adrian", "Aiden", "Ainsleigh", "Ainsley", "Ainslie", "Alex", "Alexis", "Allie", "Ally", "Alta", "Amari", "Andee", "Andi", "Andie", "Andy", "Angel", "Arden", "Ari", "Ariel", "Armani", "Ash", "Ashe", "Ashley", "Ashton", "Aspen", "Aubrey", "Auburn", "Aude", "Audie", "Audy", "August", "Averi", "Avery", "Ayden", "Bailee", "Bailey", "Baylee", "Billie", "Billy", "Blair", "Bobbie", "Bobby", "Brayden", "Briggs", "Brook", "Brooklyn", "Brynn", "Cacey", "Caden", "Cadence", "Cady", "Caelin", "Caiden", "Cam", "Camden", "Camdyn", "Cameron", "Campbell", "Camron", "Camryn", "Carlen", "Carmen", "Carson", "Carter", "Casey", "Cassidy", "Cayden", "Caylin", "Cedar", "Charlie", "Chey", "Chris", "Christan", "Cobie", "Coby", "Codie", "Cody", "Coren", "Corey", "Corrie", "Cory", "Cyan", "Cyd", "Cypress", "Cyprus", "Dakota", "Dale", "Dallas", "Dana", "Dani", "Dannie", "Danny", "Darci", "Darcy", "Darrell", "Darryl", "Dayrl", "Delaney", "Demi", "Demy", "Devan", "Devin", "Devyn", "Diamond", "Dominique", "Dorian", "Drew", "Dusty", "Dylan", "Easton", "Eden", "Elisha", "Ellery", "Elliot", "Ellis", "Ellory", "Elly", "Ember", "Embry", "Embyr", "Emerson", "Emery", "Emory", "Eris", "Esme", "Esmé", "Fae", "Fen", "Fenn", "Fin", "Finley", "Finn", "Florence", "Forrest", "Fox", "Fran", "Frances", "Gabbi", "Gabby", "Genesis", "Gerrie", "Gerry", "Gris", "Griz", "Grizz", "Hailey", "Haley", "Halie", "Halley", "Harlen", "Harlow", "Harlyn", "Harper", "Hartley", "Hayden", "Haylee", "Hayley", "Hilary", "Hollis", "Hunter", "Idgie", "Iggie", "Iggy", "Indigo", "Ira", "Izzy", "Jackie", "Jade", "Jaidan", "Jaiden", "Jaidin", "Jaidyn", "Jaime", "Jaimie", "Jamie", "Jan", "Jay", "Jaydan", "Jayden", "Jaydin", "Jaydyn", "Jaylin", "Jayme", "Jean", "Jerrie", "Jerry", "Jess", "Jesse", "Jessie", "Jett", "Jo", "Jody", "Joe", "Joey", "Jonni", "Jonnie", "Jude", "Juniper", "Justice", "Kacey", "Kacie", "Kaden", "Kai", "Kaiden", "Kam", "Kameron", "Kamron", "Karter", "Kasey", "Kay", "Kayden", "Kaylin", "Kelly", "Kelsey", "Kelsie", "Kendall", "Kerri", "Kerry", "Kirby", "Kit", "Kodi", "Koree", "Koren", "Kory", "Kris", "Krishna", "Kristen", "Ky", "Kye", "Kyle", "Kyrie", "Lacy", "Lain", "Landry", "Lane", "Larkin", "Laurel", "Lauren", "Lauris", "Leaf", "Lee", "Leighton", "Lennon", "Lennox", "Lesley", "Leslie", "Lin", "Lindsay", "Linn", "Logan", "London", "Londyn", "Lonnie", "Loren", "Lorin", "Lou", "Lowe", "Luan", "Luca", "Lyn", "Lynn", "Mackenzie", "Mackinley", "Madison", "Madox", "Mallory", "Marin", "Marion", "Marley", "Marlowe", "Mars", "Mason", "Mckinley", "Meadow", "Mel", "Meredith", "Merrill", "Micah", "Micki", "Mika", "Mischa", "Misha", "Morgan", "Neely", "Nicki", "Nico", "Nikko", "Niko", "Nova", "Paige", "Paisley", "Parker", "Pat", "Pax", "Payton", "Peyton", "Phoenix", "Piper", "Presley", "Quinn", "Rae", "Rain", "Raine", "Rainn", "Raven", "Ray", "Reagan", "Reese", "Reilly", "Remi", "Rémi", "Remington", "Remy", "Rémy", "Rey", "Riley", "River", "Robbie", "Robin", "Rogue", "Rori", "Rory", "Russi", "Ryan", "Rylan", "Rylee", "Ryley", "Sage", "Salix", "Sam", "Sammie", "Sammy", "Sandi", "Sandy", "Santana", "Sasha", "Sawyer", "Scout", "Seneca", "Shannon", "Shay", "Shea", "Shelley", "Siban", "Sibán", "Sky", "Skylar", "Skyler", "Slater", "Spencer", "Stacy", "Stevie", "Storm", "Syd", "Sydney", "Tash", "Tate", "Tatum", "Tay", "Tayler", "Taylor", "Teagen", "Teal", "Teegan", "Terra", "Terry", "Tobin", "Tommie", "Toni", "Tony", "Tori", "Torrey", "Tory", "Tracey", "Traci", "Tracy", "Tristen", "Tristyn", "Tyler", "Val", "Valentine", "Viv", "Vivian", "Whitney", "Willow", "Xan", "Xander", "Yael", "Zan", "Zane", "Zephyr", "Zoe", "Zoé", "Zoë", "Zoey"  };
	public string[] lastNameArray = { "Weber", "Moses", "Ford", "Pace", "Mullins", "Conrad", "Hamilton", "Zuniga", "Newton", "Moran", "Figueroa", "Mendez", "James", "Young", "Hensley", "Arroyo", "Conway", "Ho", "Moyer", "Ochoa", "Singleton", "Cook", "Barrett", "Mcclain", "Fitzpatrick", "Choi", "Collins", "Macias", "Parrish", "Barker", "Estrada", "Mccarty", "Valencia", "Weaver", "Clark", "Fuller", "Miles", "Andrade", "White", "Bauer", "Osborne", "Munoz", "Barron", "Watsonv", "Sullivan", "Robles", "Holloway", "Serrano", "Hale", "Knight", "Chaney", "Lawson", "Turner", "Shepard", "Jacobson", "Wilkerson", "Wiggins", "Rosales", "Bright", "Murray", "Mcclure", "Oneill", "Ritter", "Mcguire", "Brock", "Burgess", "Dickson", "Taylor", "Middleton", "Wyatt", "Bolton", "Cameron", "Maddox", "Tran", "Walsh", "Ramirez", "Hopkins", "Medina", "Higgins", "Odonnell", "Pena", "Little", "Trevino", "Buck", "Gibbs", "Rogers", "Walter", "Morton", "Meyers", "Esparza", "Stafford", "Melendez", "Dorsey", "Stone", "Bentley", "Horn", "Shea", "Mcfarland", "Lutz", "Petty", "York", "Oliver", "Glass", "Bautista", "Burke", "Robertson", "Kerr", "Harvey", "Guerra", "Hughes", "Wood", "Hancock", "Wolfe", "Mendoza", "Hines", "Weeks", "Galvan", "Manning", "Hester", "Khan", "Patrick", "Wiley", "Henderson", "Strong", "Terrell", "Madden", "Villarreal", "Peters", "Randolph", "Perry", "Ayala", "Francis", "Collier", "Bass", "Rivas", "Rodgers", "Suarez", "Saunders", "Sims", "Cain", "Frank", "Huang", "Kim", "Jenkins", "Burch", "Humphrey", "Owen", "Koch", "Waters", "Sharp", "Rowe", "Simmons", "Harding", "Mercado", "Kaufman", "Myers", "Maxwell", "Gould", "Abbott", "Burns", "Guzman", "Riley", "Ashley", "Bullock", "Hall", "Joseph", "Bowman", "Galloway", "Gibson", "Herrera", "Lin", "Santana", "Irwin", "Dyer", "Leonard", "Werner", "Villegas", "Huynh", "Clayton", "Brennan", "Lambert", "Miranda", "Vaughan", "Macdonald", "Pennington", "Aguirre", "Martin", "Hunter", "Chen", "Coffey", "Spence", "Haney", "Palmer", "Reeves", "Hurley", "Greer", "Jensen", "Hogan", "Hooper", "Calhoun", "Lara", "Clay", "Villanueva", "Walls", "Chung", "Bryant", "Mejia", "Beck", "Ewing", "Carson", "Small", "Kline", "Delgado", "Garrison", "Wilson", "Holt", "Vance", "Blackburn", "Graham", "Morrow", "Levine", "Walker", "Ferguson", "Maynard", "Morales", "Goodwin", "Mclaughlin", "Armstrong", "Franco", "Ray", "Peterson", "Oneal", "Becker", "Farley", "Carpenter", "Wolf", "Hammond", "Bailey", "Pitts", "Frost", "Patton", "Adams", "Grimes", "Reyes", "Dougherty", "Watts", "Fisher", "Oconnell", "Ayers", "Nicholson", "Baker", "Gordon", "Spears", "Daniels", "Mcmahon", "Leon", "Shaw", "Norman", "Stanton", "Marsh", "Santos", "Waller", "Rush", "Hull", "Mckenzie", "Alvarado", "Dillon", "Norris", "Edwards", "Soto", "Swanson", "Case", "Acevedo", "Wade", "House", "Mcintosh", "Montoya", "Wallace", "Kent", "Griffin", "Hatfield", "Williams", "Salas", "Brady", "Barr", "Cummings", "Finley", "Monroe", "Larson", "Church", "Fitzgerald", "Torres", "Barnett", "Huerta", "Lawrence", "Bryan", "Faulkner", "Ali", "Powers", "Mathis", "Tyler", "Clarke", "Harrington", "Estes", "Cooke", "Marks", "Stein", "Morse", "Perkins", "Gonzales", "Mcdowell", "Richardson", "Gregory", "Bartlett", "Glenn", "Ball", "Freeman", "Proctor", "Roth", "Fry", "Love", "Wells", "Andrews", "Holmes", "Cox", "Tate", "Mosley", "Cole", "Hartman", "Ross", "Pacheco", "Keith", "Alexander", "Dominguez", "Flowers", "Herring", "Avila", "Rangel", "Thomas", "Hood", "Farmer", "Lloyd", "Rubio", "Franklin", "Chandler", "Brandt", "Farrell", "Ingram", "Douglas", "Sherman", "Woodward", "Hart", "Hurst", "Cortez", "Howe", "Best", "Cruz", "Dennis", "Conner", "Navarro", "Andersen", "Gray", "Cohen", "Mcneil", "Hudson", "Montes", "Hickman", "Johns", "Pope", "Cross", "Snow", "Decker", "Holden", "Chambers", "Copeland", "Barajas", "Jennings", "Nunez", "Johnston", "Chang", "Strickland", "Cochran", "Nielsen", "Skinner", "Marquez", "Mccoy", "Underwood", "Delacruz", "King", "Mckay", "Holland", "Cunningham", "Byrd", "Roach", "Craig", "Moreno", "Montgomery", "Mccall", "Stark", "Ware" };
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
		age = rnd.Next(18, 35); // random ages of patients between 18 and 90 seemed appropriate for the game
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
	

   
