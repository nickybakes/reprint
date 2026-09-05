using System.Collections.Generic;
using UnityEngine;

public class BattleStatsPanel : Panel
{
    [SerializeField] private BattleView view;
    [SerializeField] private BattleController controller;
    [SerializeField] private CharacterBattlePanel playerBattlePanelPrefab;
    [SerializeField] private CharacterBattlePanel enemyBattlePanelPrefab;

    public CharacterBattlePanel PlayerStatPanel { get; private set; }
    public Dictionary<Character, CharacterBattlePanel> EnemyStatPanels { get; private set; }

    /// <summary>
    /// Set up rect transform data.
    /// </summary>
    void Awake()
    {
        SetupRectTransform();
        EnemyStatPanels = new Dictionary<Character, CharacterBattlePanel>();
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
            EnemyStatPanels.Add(enemy, panel);
        }
    }

    public void UpdateAllEnemyStats(Dictionary<Character, CharacterStats> stats)
    {
        foreach (Character enemy in stats.Keys)
        {
            if (EnemyStatPanels.ContainsKey(enemy))
            {
                EnemyStatPanels[enemy].UpdateStats(stats[enemy]);
            }
        }
    }

    public void UpdateAllEnemyStats(Team enemyTeam)
    {
        foreach (Character enemy in enemyTeam.Members)
        {
            if (EnemyStatPanels.ContainsKey(enemy))
            {
                EnemyStatPanels[enemy].UpdateStats(enemy.Stats);
            }
        }
    }

    public void UpdateEnemyIntent(EnemyAbility ability, Character enemy)
    {
        if (EnemyStatPanels.ContainsKey(enemy))
        {
            EnemyStatPanels[enemy].UpdateIntent(ability);
        }
    }

    public void EnableEnemySelection()
    {
        foreach (CharacterBattlePanel panel in EnemyStatPanels.Values)
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
        foreach (CharacterBattlePanel panel in EnemyStatPanels.Values)
        {
            panel.DisableTargetSelection();
        }
    }

    public void HideAllEnemyPanels()
    {
        foreach (Character enemy in EnemyStatPanels.Keys)
        {
            EnemyStatPanels[enemy].Hide();
        }
    }

    public void ShowAllEnemyPanels()
    {
        foreach (Character enemy in EnemyStatPanels.Keys)
        {
            EnemyStatPanels[enemy].Show();
        }
    }
}
