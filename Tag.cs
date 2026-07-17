using Godot;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using static Godot.EditorToaster;

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

    public virtual void ExecuteDaily(PatientStats patient) { }

    public virtual void ExecuteInteraction(PatientStats patient) { }

    public virtual void ExecuteMaxSeverity(PatientStats patient) { }

    public virtual Tag Clone() { return null; }
    

    public void GetParentData(Tag tag)
    {
        tag.increment = increment;
        tag.types = types;
        tag.strength = strength;
    }
    public Tag CloneParent()
    {
        Tag clonedTag = new Tag
        {
            increment = increment,
            types = types,
            strength = strength
        };
        return clonedTag;
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

    public override Tag Clone()
    {
        WorseningTag specifiedClone = new WorseningTag
        {
            count = count
        };
        GetParentData(specifiedClone);
        return specifiedClone;
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

    public override Tag Clone()
    {
        HealingTag specifiedClone = new HealingTag
        {
            count = count
        };
        GetParentData(specifiedClone);
        return specifiedClone;
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

    public override Tag Clone()
    {
        UnstableTag specifiedClone = new UnstableTag
        {
            count = count,
            wasTreatedToday = wasTreatedToday
        };
        GetParentData(specifiedClone);
        return specifiedClone;
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

    public override Tag Clone()
    {
        DeadlyTag specifiedClone = new DeadlyTag
        {
        };
        GetParentData(specifiedClone);
        return specifiedClone;
    }
}

public class ResistantTag : Tag
{
    public int ratioA;
    public int ratioB;

    public ResistantTag()
    {
    }

    public override void ExecuteInteraction(PatientStats patient)
    {
        Random rnd = new Random();
        int chance = rnd.Next(1, ratioB + 1);
        if(chance <= ratioA)
        {
            patient.malady.isImmune = true;
        }
    }

    public override Tag Clone() 
    {
        ResistantTag specifiedClone = new ResistantTag
        {
            ratioA = ratioA,
            ratioB = ratioB
        };
        GetParentData(specifiedClone);
        return specifiedClone;
    }
}

