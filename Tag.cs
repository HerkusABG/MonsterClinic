using Godot;
using System;
using System.Collections.Generic;

public class Tag
{
    public List<TagType> types = new List<TagType>();
    public int increment;
    public int strength;

    public bool HasTagType(TagType inputType)
    {
        if(types.Contains(inputType))
        {
            return true;
        }
        return false;
    }

    public virtual void ExecuteDaily(PatientStats patient)
    {

    }

    public virtual void ExecuteInteraction(PatientStats patient)
    {

    }

    public virtual void ExecuteMaxSeverity(PatientStats patient)
    {

    }
}
public enum TagType
{
    Daily,
    Interaction,
    MaxSeverity
}
public class WorseningTag : Tag
{
    public int count;
    public WorseningTag()
    {
        count = 0;
    }

    public override void ExecuteDaily(PatientStats patient)
    {
        Malady malady = patient.malady;
        count++;
        if (count >= increment)
        {
            malady.severity += strength;
            if (malady.severity > 5)
            {
                malady.severity = 5;
            }
            count = 0;
        }
    }
}

public class HealingTag : Tag
{
    public int count;
    public HealingTag()
    {
        count = 0;
    }

    public override void ExecuteDaily(PatientStats patient)
    {
        Malady malady = patient.malady;
        count++;
        if (count >= increment)
        {
            malady.severity += strength;
            count = 0;
        }
    }
}

public class UnstableTag : Tag
{
    public int count;
    bool wasTreatedToday;
    public UnstableTag()
    {
        count = 0;
        wasTreatedToday = false;
    }

    public override void ExecuteDaily(PatientStats patient)
    {
        Malady malady = patient.malady;
        if (wasTreatedToday)
        {
            wasTreatedToday = false;
        }
        else
        {
            malady.severity += strength;
        }
    }

    public override void ExecuteInteraction(PatientStats patient)
    {
        wasTreatedToday = true;
    }
}


public class DeadlyTag : Tag
{
    public DeadlyTag()
    {
    }

    public override void ExecuteMaxSeverity(PatientStats patient)
    {
        Malady malady = patient.malady;
        if(malady.severity >= 5)
        {
            patient.KillPatient();
        }
    }
}

