using TMPro;
using UnityEngine;

public class CharacterStatsPanel : Panel
{

    [SerializeField] protected StatDisplay healthDisplay;

    [SerializeField] protected StatDisplay abilityPointsDisplay;
    [SerializeField] protected TextMeshProUGUI chainTextDisplay;

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
        displayedStats = new CharacterStats();
    }

    public void UpdateStats(CharacterStats stats)
    {
        UpdateHealth(stats);
        UpdateAbilityPoints(stats);
        UpdateChain(stats);
    }

    private void UpdateHealth(CharacterStats stats)
    {
        if (healthDisplay)
        {
            healthDisplay.UpdateStatDisplay(stats.Health, stats.HealthMax);
        }
        displayedStats.Health = stats.Health;
        displayedStats.HealthMax = stats.HealthMax;
    }

    private void UpdateAbilityPoints(CharacterStats stats)
    {
        if (abilityPointsDisplay)
        {
            abilityPointsDisplay.UpdateStatDisplay(stats.AbilityPoints, stats.AbilityPointsMax);
        }
        displayedStats.AbilityPoints = stats.AbilityPoints;
        displayedStats.AbilityPointsMax = stats.AbilityPointsMax;
    }

    private void UpdateChain(CharacterStats stats)
    {
        if (chainTextDisplay)
        {
            chainTextDisplay.SetText(stats.Chain.ToString());
        }
        displayedStats.Chain = stats.Chain;
    }
}
