using System.Collections.Generic;
using UnityEngine;

public class Mod
{
    public ModProfile Profile { get { return baseData.Profile; } }

    public string Name { get { return baseData.Profile.Name; } }
    public string Description { get { return baseData.Profile.Description; } }

    protected List<List<ModBehavior>> behaviorsTable;

    protected ModData baseData;

    public Mod(ModData data)
    {
        baseData = data;

        behaviorsTable = new List<List<ModBehavior>>()
        {
            data.ModBehaviors.List
        };
    }

}
