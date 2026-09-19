using UnityEngine;

public class CharacterBattlePanel : CharacterStatsPanel
{
    [SerializeField] private IntentDisplayGroup intentDisplayGroup;

    [SerializeField] private BetterButton targetButton;

    [SerializeField] private Display AbilityStatsGroup;
    [SerializeField] private NumberPopupGroup physicalDamageNumberPopupGroup;
    [SerializeField] private NumberPopupGroup critDamageNumberPopupGroup;

    protected CharacterFigure figure;
    protected BattleView view;

    protected BattleController controller;

    /// <summary>
    /// Set up rect transform data.
    /// </summary>
    void Awake()
    {
        SetupRectTransform();
    }

    public void SetupPanel(CharacterFigure _figure, Character _character, BattleView _view, BattleController _controller)
    {
        figure = _figure;
        view = _view;
        controller = _controller;
        character = _character;
        displayedStats = new CharacterStats(character);
        if (AbilityStatsGroup != null)
        {
            // AbilityStatsGroup.transform.parent = AbilityStatsGroup.transform.parent.parent.parent;
            view.PlayerAbilityDisplayGroup.abilityStatsGroup = AbilityStatsGroup;
        }
        UpdateStats(_character.Stats);
    }

    public void UpdateStatsWithDifference(CharacterStats statsBefore, CharacterStats statsAfter)
    {
        UpdateStats(statsAfter);
        ShowPhysicalDamageTaken(statsBefore.PhysicalDamageTaken, statsAfter.PhysicalDamageTaken);
        ShowCritDamageTaken(statsBefore.CriticalDamageTaken, statsAfter.CriticalDamageTaken);
    }

    public void EnableTargetSelection()
    {
        targetButton.Show();
    }

    public void DisableTargetSelection()
    {
        targetButton.Hide();
    }

    public void SubmitTarget()
    {
        controller.SubmitTarget(character);
    }

    public void UpdateIntent(EnemyAbility ability)
    {
        if (intentDisplayGroup)
        {
            intentDisplayGroup.Refresh(ability, character);
        }
    }

    public void ShowPhysicalDamageTaken(int before, int after)
    {
        int value = after - before;
        if (physicalDamageNumberPopupGroup && value != 0)
        {
            physicalDamageNumberPopupGroup.DisplayNumber(value);
        }
    }

    public void ShowCritDamageTaken(int before, int after)
    {
        int value = after - before;
        if (critDamageNumberPopupGroup && value != 0)
        {
            critDamageNumberPopupGroup.DisplayNumber(value);
        }
    }

    public void UpdatePosition()
    {
        if (figure && view)
        {
            rectTransform.anchoredPosition = UIView.WorldToCanvasPoint(figure.Center);
        }
    }

    void Update()
    {
        UpdatePosition();
    }
}
