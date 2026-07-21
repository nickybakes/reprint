using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BattleManager : MonoBehaviour
{
    /// <summary>
    /// Sends out the current list of game changes in this event.
    /// </summary>
    [SerializeField] private UnityEvent<List<BattleStateChange>> submitChangesEvent;

    /// <summary>
    /// List of changes to the battle that have been calculated but not shown to the player yet.
    /// </summary>
    private List<BattleStateChange> pendingBattleChanges;

    public PlayerCharacter Player { get; private set; }

    public EnemyTeam EnemyTeam { get; private set; }

    /// <summary>
    /// The number of the current turn the battle is in. Increments once all teams have done an ability and loops back to the first team.
    /// </summary>
    public int TurnIndex { get; private set; } = -1;

    /// <summary>
    /// The data for what characters are in this battle scenario
    /// </summary>
    private BattleData battleData;

    private AbilitySequence playerAbilitySequence;

    /// <summary>
    /// Invokes the Submit Changes Event and starts a new list of changes.
    /// </summary>
    public virtual void SubmitChanges()
    {
        submitChangesEvent.Invoke(pendingBattleChanges);
        pendingBattleChanges = new List<BattleStateChange>();
    }

    public void SetupBattle(BattleData data)
    {
        TurnIndex = -1;
        Player = new PlayerCharacter(data.playerCharacterData, data.playerMods);

        EnemyTeam = new EnemyTeam();

        foreach (EnemyData enemyData in data.enemyDatas)
        {
            EnemyTeam.AddMember(new EnemyCharacter(enemyData));
        }

        pendingBattleChanges = new List<BattleStateChange>
        {
            new BattleInitialized(Player, EnemyTeam)
        };
        SubmitChanges();

        StartPlayerTurn();
    }

    public virtual void StartPlayerTurn()
    {
        playerAbilitySequence = new AbilitySequence();
        TurnIndex++;
        Player.ResetForTurn();
        Player.RefreshAbilitySequencingStats(playerAbilitySequence);

        foreach (EnemyCharacter enemy in EnemyTeam.Enemies)
        {
            if (enemy.IsAlive)
            {
                enemy.DecideAbility(this);
                pendingBattleChanges.Add(new EnemyDecideAbility(enemy.ChosenAbility, enemy));
            }
        }

        pendingBattleChanges.Add(new PlayerTurnStart(Player.AbilitySequencingStats, TurnIndex));
        SubmitChanges();
    }

    public virtual void PlayerSubmitAbility(Ability ability)
    {
        AbilitySequenceChangeType changeType = playerAbilitySequence.AddOrOverclockAbility(ability, Player.AbilitySequencingStats.AbilityPoints);
        if (changeType != AbilitySequenceChangeType.None)
        {
            SubmitPlayerChangeAbilitySequence(changeType);
        }
    }

    public virtual void PlayerSubmitTarget(Character target)
    {
        AbilitySequenceChangeType changeType = playerAbilitySequence.SetLastAbilityTarget(target);
        if (changeType != AbilitySequenceChangeType.None)
        {
            SubmitPlayerChangeAbilitySequence(changeType);
        }
    }

    public virtual void PlayerSubmitConfirmAbilitySequence()
    {
        pendingBattleChanges.Add(new PlayerTurnEnd());
        DoPlayerAbilitySequence();

        DoEnemyAbilities();

        // Check if all enemies are eliminated. If not, next turn!

        StartPlayerTurn();
    }

    public virtual void DoPlayerAbilitySequence()
    {
        Player.ApplyAbilitySequencingStats();

        List<AbilitySelection> sortedAbilitySequence = playerAbilitySequence.GetSortedSequence();

        List<AbilitySelection> abilitySelections = new List<AbilitySelection>();
        List<StatChangeBreakdown> statChangeBreakdowns = new List<StatChangeBreakdown>();

        int abilitySeqIndex = 0;

        foreach (AbilitySelection abilitySelection in sortedAbilitySequence)
        {
            StatChangeBreakdown result = StatCalculation.GetPlayerAbilityResult(abilitySelection, abilitySeqIndex, playerAbilitySequence, this);
            abilitySelections.Add(abilitySelection);
            statChangeBreakdowns.Add(result);
            abilitySeqIndex++;
        }

        pendingBattleChanges.Add(new PlayerDoAbilitySequence(Player, abilitySelections, statChangeBreakdowns));

        EnemyTeam.CalculateTurnOrder(Player.DodgePriorities);

        SubmitChanges();
    }

    public virtual void DoEnemyAbilities()
    {
        foreach (EnemyCharacter enemy in EnemyTeam.EnemiesInTurnOrder)
        {
            if (enemy.IsAlive)
            {
                StatChangeBreakdown results = StatCalculation.GetEnemyAbilityResult(enemy.ChosenAbility, enemy, this);
                pendingBattleChanges.Add(new EnemyDoAbility(enemy.ChosenAbility, results));
            }
        }

        SubmitChanges();
    }

    public virtual void PlayerSubmitBack()
    {
        if (playerAbilitySequence != null)
        {
            AbilitySequenceChangeType changeType = playerAbilitySequence.StepBackInSequenceBuilding();
            if (changeType != AbilitySequenceChangeType.None)
            {
                SubmitPlayerChangeAbilitySequence(changeType);
            }
        }
    }

    protected virtual void SubmitPlayerChangeAbilitySequence(AbilitySequenceChangeType changeType)
    {
        Player.RefreshAbilitySequencingStats(playerAbilitySequence);
        pendingBattleChanges.Add(new PlayerChangeAbilitySequence(playerAbilitySequence, Player.AbilitySequencingStats, changeType));
        SubmitChanges();
    }
}
