using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character
{

    public CharacterProfile Profile { get; protected set; }
    public CharacterStats Stats { get; protected set; }
    public CharacterStats TurnStatsStorage { get; private set; }

    public List<Mod> Mods { get; protected set; }

    public string Name { get; protected set; }

    public bool IsAlive
    {
        get
        {
            return Stats.Health > 0;
        }
    }

    public Character()
    {
        Stats = new CharacterStats(this);
        TurnStatsStorage = new CharacterStats(this);
    }

    public abstract void ResetForTurn();

    public void ApplyPhysicalDamage(int damage)
    {
        //TODO: Use any mods/resistances on the victim to lessen the damage

        // Use dodge first
        int tempDamage = damage;
        damage = Math.Max(0, damage - Stats.Dodge);
        Stats.Dodge = Math.Max(0, Stats.Dodge - tempDamage);

        if (damage > 0)
        {
            Stats.Health -= damage;
            Stats.Chain = 0;
        }
    }

    public void ApplyChain(int chain)
    {
        Stats.Chain += chain;
    }

    public void ApplyDodge(int dodge)
    {
        Stats.Dodge += dodge;
    }

    public void CalculateStatChangesFromMods(GameValues gameValues, StatChangeBreakdown statChangeBreakdown)
    {
        for (int i = 0; i < Mods.Count; i++)
        {
            Mod mod = Mods[i];
            List<bool> passingBehaviors = new List<bool>();

            foreach (ModBehavior behavior in mod.Behaviors)
            {
                passingBehaviors.Add(StatCalculation.DoGameConditionsPass(behavior.Conditions, gameValues));
            }
        }
    }
}