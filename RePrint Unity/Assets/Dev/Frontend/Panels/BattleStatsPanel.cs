using System.Collections.Generic;
using UnityEngine;

public class BattleStatsPanel : Panel
{
    [SerializeField] private BattleView view;
    [SerializeField] private BattleController controller;
    [SerializeField] private CharacterBattlePanel playerBattlePanelPrefab;
    [SerializeField] private CharacterBattlePanel enemyBattlePanelPrefab;

    public CharacterBattlePanel PlayerStatPanel { get; private set; }
    public List<CharacterBattlePanel> EnemyStatPanels { get; private set; }

    /// <summary>
    /// Set up rect transform data.
    /// </summary>
    void Awake()
    {
        SetupRectTransform();
        EnemyStatPanels = new List<CharacterBattlePanel>();
    }

    public void AddPlayerStatsPanel(Character character)
    {
        CharacterBattlePanel panel = Instantiate(playerBattlePanelPrefab, transform);
        panel.SetupPanel(view.PlayerFigureGroup.GetFigure(character), character, view, controller);
        PlayerStatPanel = panel;
    }

    public void AddEnemyStatsPanels(Team enemyTeam)
    {
        foreach (Character enemy in enemyTeam.Members)
        {
            CharacterBattlePanel panel = Instantiate(enemyBattlePanelPrefab, transform);
            panel.SetupPanel(view.EnemyFigureGroup.GetFigure(enemy), enemy, view, controller);
            EnemyStatPanels.Add(panel);
        }
    }

    public void EnableEnemySelection()
    {
        foreach (CharacterBattlePanel panel in EnemyStatPanels)
        {
            panel.EnableTargetSelection();
        }
    }

    public void EnablePlayerSelection()
    {
        PlayerStatPanel.EnableTargetSelection();
    }

    public void DisableAllTargetSelection()
    {
        PlayerStatPanel.DisableTargetSelection();
        foreach (CharacterBattlePanel panel in EnemyStatPanels)
        {
            panel.DisableTargetSelection();
        }
    }
}
