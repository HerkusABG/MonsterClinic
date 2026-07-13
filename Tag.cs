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

    public virtual void ExecuteDaily(Malady inputMalady)
    {

    }

    public virtual void ExecuteInteraction(Malady inputMalady)
    {

    }
}
public enum TagType
{
    Daily,
    Interaction
}
public class WorseningTag : Tag
{
    public int count;
    public WorseningTag()
    {
        count = 0;
    }

    public override void ExecuteDaily(Malady inputMalady)
    {
        count++;
        GD.Print($"Count is now {count}, increment is {increment}");
        if (count >= increment)
        {
            inputMalady.severity += strength;
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

    public override void ExecuteDaily(Malady inputMalady)
    {
        count++;
        GD.Print($"Count is now {count}, increment is {increment}");
        if (count >= increment)
        {
            inputMalady.severity += strength;
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

    public override void ExecuteDaily(Malady inputMalady)
    {
        if(wasTreatedToday)
        {
            wasTreatedToday = false;
        }
        else
        {
            inputMalady.severity += strength;
        }
    }

    public override void ExecuteInteraction(Malady inputMalady)
    {
        wasTreatedToday = true;
    }
}


