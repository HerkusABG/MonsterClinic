using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class AdmissionManager : Node
{
    [Export] Contents_P_I PatientAdmission;
    [Export] public Sprite2D PatientSpriteDisplay;
    [Export] Button AdmitButton;

    private int patientsLeft;

    PatientStats nullPatient = new PatientStats();

    private PatientStats InternalPatient;

    Node2D LatestRoom = null;

    private List<StoryPatientStats> StoryPatientsLeft = new List<StoryPatientStats>();

    //need to store where in the StoryPatientsLeft list we got the current patient from, so we can remove them when we admit them
    private int storyPatientsLeftPosition;

    public void Initialize()
    {
        NullPatientInitialize();

        // Added stuff for the patient sprite at the window
        UpdateWindowStatus();

        //grab all the story patients and put them in a list
        for (int i = 0; i < StoryPatientList.Database.Count; i++)
        {
            StoryPatientsLeft.Add(StoryPatientList.Database.ElementAt(i).Value);
            StoryPatientsLeft[i].AddInfo();
        }
    }
    public void Admit()
    {
        //Logic for admitting the patient.
        Node mainNode = GetParent().GetParent();
        if (IsClinicFull()) //  <----- Is the clinic full?
       {
            //If yes, then find an empty room for the new patient!
            var roomNode = RoomManager.FindEmptyRoom(); 
            if (roomNode != null)
            {
                //Get the room class from the Node2D class
                Room room = roomNode as Room;
                //Assign patient to room
                room.AssignPatient(PatientAdmission.PatientPointer);
                GlobalData.patientCount++;
                //if we're admitting a story patient, remove them from the list so they don't come back
                if (PatientAdmission.PatientPointer is StoryPatientStats)
                {
                    StoryPatientsLeft.RemoveAt(storyPatientsLeftPosition);
                }

                //This method is needed to make the visit button work.
                SetLatestPatientRoom(room);
                //Checking to see if the clinic is full (again)

                // Added stuff for the patient sprite at the window
                PatientQueueLogic(); 
                UpdateWindowStatus();

                IsClinicFull();
            }
       }
    }
    public PatientStats GetNullPatient()
    {
        //Grabbing the null patient. This is used when the patient admission has no more patients at the clinic.
        //When the null patient is used, the dialogue manager won't show any messages, just "..."
        return nullPatient;
    }
    public void PatientQueueLogic()
    {
        //Any logic that has to do with the patient queue goes here.
        //Not to be confused with a similar method in Contents_P_I, there the class is mostly used for
        //UI stuff, here for variables etc., that have to do with both the patient queue and the admission manager
        patientsLeft--;
    }

    public int HowManyPatientsLeft()
    {
        //Return how many patients are left.
        return patientsLeft;
    }

    public void NewDayLogic()
    {
        //New day logic
        LatestRoom = null;
        patientsLeft = Upgrades.newPatientSlots.incrementTarget;
    }

    public Node2D GetLatestRoom()
    {
        //Return the last room in which a patient has been admitted.
        //Used mainly for the visit button.
        return LatestRoom;
    }
    public void Reject()
    {
        // Added stuff for the patient sprite at the window
        PatientQueueLogic();
        UpdateWindowStatus();
    }

    // Added stuff for the patient sprite at the window
    private void UpdateWindowStatus()
    {
        GlobalData.IsPatientInWindow = (patientsLeft > 0);
    }

    public bool IsClinicFull()
    {
        //Seeing if the clinic is full
        //And then in the end we determine whether it's TRUE or FALSE
        int emptyRoomCount = RoomManager.GetEmptyRoomCount();
        //if (HowManyPatientsLeft() > 0) return true;
        if (emptyRoomCount > 0)
        {
            SetButtonStatus(AdmitButton, true);
            return true;

        }
        else
        {
            SetButtonStatus(AdmitButton, false);
            return false;
        }
    }

    public void SetButtonStatus(Button button, bool status)
    {
        //Used for enabling/disabling specific admission manager buttons. In this case
        //used mainly for the IsClinicFull() method.
        button.Disabled = !status;
    }
    public PatientStats GenerateNewPatient()
    {
        //Create a new instance of a patient.
        PatientStats patientStats;
        StoryPatientStats storyPatientStats;
        Random random = new Random();
        int odds = random.Next(10);
        //based on the odds, or if there's no story patients left, make a normal one
        if (odds > 3 || StoryPatientsLeft.Count == 0)
        {
            patientStats = new PatientStats();
            InternalPatient = patientStats;
            return patientStats;
        //else, make a story patient
        } 
        else
        {
            storyPatientStats = new StoryPatientStats();
            storyPatientsLeftPosition = random.Next(0, StoryPatientsLeft.Count);
            storyPatientStats.StoryPatientSetup(storyPatientsLeftPosition);
            //storyPatientStats = StoryPatientsLeft[storyPatientsLeftPosition];
            InternalPatient = storyPatientStats;
            return storyPatientStats;
        }
    }

    public void SetLatestPatientRoom(Node2D room)
    {
        //Set the last room where a patient has been assigned.
        LatestRoom = room;
    }
    private void VisitInternalLogic()
    {
        //CURRENTLY DEPRECATED
        GlobalData.inPatientRoom = true;
    }
    private void NullPatientInitialize()
    {
        //Initializing the null patient.
        nullPatient.malady = MaladyList.Database.ElementAt(0).Value;
        nullPatient.age = 0;
        nullPatient.patientID = "";
    }
}