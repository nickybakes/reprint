using System.Collections.Generic;
using UnityEngine;

public class IntentDisplayGroup : MonoBehaviour
{
    [SerializeField] private IntentDisplay physicalDamageDisplay;
    [SerializeField] private IntentDisplay bleedDisplay;
    [SerializeField] private IntentDisplay shockDisplay;
    [SerializeField] private IntentDisplay burnDisplay;
    [SerializeField] private IntentDisplay bioDisplay;
    [SerializeField] private IntentDisplay defensiveDisplay;
    [SerializeField] private IntentDisplay buffDisplay;
    [SerializeField] private IntentDisplay chainDisplay;

    public void Refresh(EnemyAbility ability, Character activator)
    {
        AbilityStats stats = ability.GetAbilityStats(activator);

        if (physicalDamageDisplay)
        {
            physicalDamageDisplay.gameObject.SetActive(ability.Intent == EnemyIntent.PhysicalDamage);
            physicalDamageDisplay.DisplayIntent(stats.MinPhysicalDamage, stats.MaxPhysicalDamage);
        }

        if (bleedDisplay)
        {
            bleedDisplay.gameObject.SetActive(ability.Intent == EnemyIntent.Bleed);
        }

        if (shockDisplay)
        {
            shockDisplay.gameObject.SetActive(ability.Intent == EnemyIntent.Bleed);
        }

        if (burnDisplay)
        {
            burnDisplay.gameObject.SetActive(ability.Intent == EnemyIntent.Bleed);
        }

        if (bioDisplay)
        {
            bioDisplay.gameObject.SetActive(ability.Intent == EnemyIntent.Bleed);
        }

        if (defensiveDisplay)
        {
            defensiveDisplay.gameObject.SetActive(ability.Intent == EnemyIntent.Defensive);
            defensiveDisplay.DisplayIntent(stats.MinDodge, stats.MaxDodge);
        }

        if (buffDisplay)
        {
            buffDisplay.gameObject.SetActive(ability.Intent == EnemyIntent.Bleed);
        }

        if (chainDisplay)
        {
            chainDisplay.gameObject.SetActive(ability.Intent == EnemyIntent.ChainDamage);
            chainDisplay.DisplayIntent(stats.MinChain, stats.MaxChain);
        }
    }
}
