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
    public int BaseMaxAP { get; protected set; }

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

    public void SetTurnStats()
    {
        TurnStatsStorage.CopyFrom(Stats);
    }

    public void RestoreTurnStats()
    {
        Stats.CopyFrom(TurnStatsStorage);
    }

    public abstract void ResetForTurn();

    public void ApplyPhysicalDamage(int damage, float damageMultiplier)
    {
        int totalDamage = (int)(damage * damageMultiplier);

        // Use dodge first
        int tempDamage = totalDamage;
        totalDamage = Math.Max(0, totalDamage - Stats.Dodge);
        Stats.Dodge = Math.Max(0, Stats.Dodge - tempDamage);

        if (totalDamage > 0)
        {
            Stats.Health -= totalDamage;
            Stats.Chain = 0;
        }
    }

    public void ApplyChain(int chain)
    {
        Stats.Chain = Math.Max(0, Stats.Chain + chain);
    }

    public void ApplyDodge(int dodge)
    {
        Stats.Dodge += dodge;
    }

    public void ResetTempStats()
    {
        Stats.TempChain = 0;
    }

    public void CalculateStatChangesFromMods(GameValues gameValues, StatChangeBreakdown statChangeBreakdown)
    {
        for (int i = 0; i < Mods.Count; i++)
        {
            Mod mod = Mods[i];

            int numPassingBehaviors = 0;

            List<bool> passingBehaviors = new List<bool>();

            foreach (ModBehavior behavior in mod.Behaviors)
            {
                passingBehaviors.Add(StatCalculation.DoGameConditionsPass(behavior.Conditions, gameValues));
                numPassingBehaviors++;
            }

            if (numPassingBehaviors == 0)
            {
                continue;
            }

            List<ModEffect> effects = mod.GetModEffects(passingBehaviors);

            ModResult modResult = new ModResult(mod, gameValues.battleManager.Player, gameValues.battleManager.EnemyTeam);

            foreach (ModEffect effect in effects)
            {
                switch (effect.Type)
                {
                    case ModEffectType.StackStatChange:
                        AmountsPerCharacter amounts = StatCalculation.GetPotentialEffectAmount(gameValues, effect);
                        modResult.statChangeAmounts.AddAmounts(amounts.Amounts, effect.StatChange);
                        break;
                }
            }

            statChangeBreakdown.modResults.Add(modResult);
        }

    }
}