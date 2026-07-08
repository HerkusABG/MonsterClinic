using Godot;
using System;

public class Tag
{
    public TagType type;

    public TagType GetTagType()
    {
        return type;
    }
    public virtual void DailyAction()
    {

    }

    public virtual void Execute(Malady inputMalady)
    {

    }
}
public enum TagType
{
    Daily,
    Medicine
}
public class WorseningTag : Tag
{
    public WorseningTag()
    {
        //type = TagType.Daily;
    }

    public override void Execute(Malady inputMalady)
    {
        inputMalady.severity++;
    }
}

public class HealingTag : Tag
{
    public HealingTag()
    {
        //type = TagType.Daily;
    }

    public override void Execute(Malady inputMalady)
    {
        inputMalady.severity--;
    }
}


