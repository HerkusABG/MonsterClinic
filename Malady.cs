using Godot;
using System;
using System.Collections.Generic;


public class Malady

    //the different stats a malady can have
{
    //Used for tracking how severe the malady is. 0 - cured, 5 - final stage (dead, succumbed etc)
    public int severity { get; set; }
    //How the malady name is displayed in the game.
    public string name { get; set; }
    //List of all symptoms. Hard to explain but this list stores all other malady symptoms
    //and is crucial to making the diagnosis box work.
    public List<string> allSymptoms = new List<string>();
    //Refers to symptoms that are not triggered by the two currently existing diagnosis tools.
    public List<Symptom> dialogueSymptoms = new List<Symptom>();
    //Refers to symptoms and the related dialogue that appears when the stethoscope is used.
    public List<Symptom> pulseSymptoms = new();
    //Refers to symptoms and the related dialogue that appears when the thermometer is used.
    public List<Symptom> temperatureSymptoms = new();
    //List of tags that define a malady's behaviour
    public List<Tag> tags = new List<Tag>();

    public List<Medicine> cures = new List<Medicine>();

    public bool isImmune = false;

    public Malady()
    {
        //Small piece of logic to ensure malady severity gets assigned properly in PatientStats.
        //As far as I know it helps assign severity to each instance of a malady correctly.
        severity = -1;
    }

    public Malady Clone()
    {
        return new Malady
        {
            name = name,
            severity = severity,
            allSymptoms = allSymptoms,
            dialogueSymptoms = dialogueSymptoms,
            pulseSymptoms = pulseSymptoms,
            temperatureSymptoms = temperatureSymptoms,
            tags = tags,
            cures = cures
        };
    }
}

