using TMPro;
using UnityEngine;

public class CharacterStatsPanel : Panel
{

    [SerializeField] protected StatDisplay healthDisplay;

    [SerializeField] protected SegmentedMeter abilityPointsMeter;
    [SerializeField] protected StatDisplay chainDisplay;
    [SerializeField] protected StatDisplay dodgeDisplay;

    /// <summary>
    /// The currently displayed states. When a character's stats get updated, we can reference these old stats to
    /// do unique effects like playing specific HUD animations for losing or gaining health.
    /// </summary>
    protected CharacterStats displayedStats;

    protected Character character;

    /// <summary>
    /// Set up rect transform data.
    /// </summary>
    void Awake()
    {
        SetupRectTransform();
    }

    public void UpdateStats(CharacterStats stats)
    {
        UpdateHealth(stats);
        UpdateAbilityPoints(stats);
        UpdateChain(stats);
        UpdateDodge(stats);
    }

    private void UpdateHealth(CharacterStats stats)
    {
        if (healthDisplay)
        {
            healthDisplay.DisplayFraction(stats.Health, stats.HealthMax);
        }
        displayedStats.Health = stats.Health;
        displayedStats.HealthMax = stats.HealthMax;
    }

    private void UpdateAbilityPoints(CharacterStats stats)
    {
        if (abilityPointsMeter)
        {
            abilityPointsMeter.Refresh(stats.AbilityPoints, stats.AbilityPointsMax);
        }
        displayedStats.AbilityPoints = stats.AbilityPoints;
        displayedStats.AbilityPointsMax = stats.AbilityPointsMax;
    }

    private void UpdateChain(CharacterStats stats)
    {
        if (chainDisplay)
        {
            chainDisplay.DisplayValue(stats.Chain);
        }
        displayedStats.Chain = stats.Chain;
    }

    private void UpdateDodge(CharacterStats stats)
    {
        if (dodgeDisplay)
        {
            dodgeDisplay.DisplayValue(stats.Dodge);
        }
        displayedStats.Dodge = stats.Dodge;
    }
}
