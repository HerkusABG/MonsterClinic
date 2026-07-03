using Godot;
using System;
using System.Collections.Generic;


public class Malady

    //the different stats a malady can have (Placeholders)
{
    public int severity { get; set; }
	public int spreadability { get; set; }
    public int vulnerability { get; set; }
    public string name { get; set; }

    public List<string> allSymptoms = new List<string>();

    public List<Symptom> dialogueSymptoms = new List<Symptom>();

    public List<Symptom> pulseSymptoms = new();

    public List<Symptom> temperatureSymptoms = new();

    public Malady()
    {
        severity = -1;
    }
}

