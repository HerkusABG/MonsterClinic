using Godot;
using System;

public partial class PatientInfoManager : Control
{
    TabInfo GeneralTab;
    TabInfo MaladyTab;
    TabInfo HistoryTab;

    Room room;
    PatientStats patient;


	public void Initialize(Room room)
	{
        this.room = room;
        GetNodes();

        GeneralTab.Initialize();
        MaladyTab.Initialize();
        HistoryTab.Initialize();
    }

    public void SetPatient(PatientStats input)
    {
        if(input != null)
        {
            patient = input;
        }
        else
        {
            GeneralTab.Write("");
            MaladyTab.Write("");
            HistoryTab.Write("");
        }
    }

    private void GetNodes()
    {
        GeneralTab = GetNode<TabInfo>("General");
        MaladyTab = GetNode<TabInfo>("Malady");
        HistoryTab = GetNode<TabInfo>("History");
    }

    public void UpdateText(TabType type, bool showThisTab)
    {
        UpdateGeneralTab(patient);
        UpdateMaladyTab(patient);
        UpdateHistoryTab();
        /*if (type == TabType.General)
        {
            if(showThisTab)
            {
                GeneralTab.Show();
            }
            UpdateGeneralTab(patient);
        }
        else if (type == TabType.Malady)
        {
            if (showThisTab)
            {
                MaladyTab.Show();
            }
            UpdateMaladyTab(patient);
        }*/
    }

    private void UpdateGeneralTab(PatientStats patient)
    {
        string input;
        if (patient.IsPatientAlive())
        {
            input = "Alive";
        }
        else
        {
            input = "Dead";
        }
        string mainText = 
            $" Malady: {patient.malady.name}" +
            $"\n First Name: {patient.firstName}" +
            $"\n Last Name: {patient.lastName}" +
            $" \n Age: {patient.age}" +
            $"\n Status: {input}";
        GeneralTab.Write(mainText);
    }

    private void UpdateMaladyTab(PatientStats patient)
    {
        string symptoms = "";
        string tags = "";
        foreach(string symptom in patient.malady.allSymptoms)
        {
            symptoms += symptom;
            symptoms += ", ";
        }
        foreach (Tag tag in patient.malady.tags)
        {
            tags += tag.name;
            tags += ", ";
        }
        string mainText = 
            $" Malady: {patient.malady.name}" +
            $" \n Severity: {patient.malady.severity} " +
            $"\n Symptoms: {symptoms}" + 
            $"\n Tags: {tags}";
        MaladyTab.Write(mainText);
    }

    private void UpdateHistoryTab()
    {
        string output = "";
        foreach (ClinicAction action in patient.clinicActions)
        {
            output += $"{action.output} \n";
            //output += ", ";
        }
        HistoryTab.Write(output);
    }
}

public enum TabType
{
    General,
    Malady,
    History
}