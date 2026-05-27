using UnityEngine;

public class CharacterBattlePanel : CharacterStatsPanel
{
    [SerializeField] private IntentDisplayGroup intentDisplayGroup;

    [SerializeField] private BetterButton targetButton;

    [SerializeField] private Display AbilityStatsGroup;

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

    public void UpdatePosition()
    {
        if (figure && view)
        {
            rectTransform.anchoredPosition = view.WorldToCanvasPoint(figure.Center);
        }
    }

    void Update()
    {
        UpdatePosition();
    }
}
