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
        Player = new PlayerCharacter(data.playerCharacterData);

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
        RefreshAllCharacterInGameValues();

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

    public void RefreshAllCharacterInGameValues()
    {
        int numberOfEnemies = EnemyTeam.GetNumberOfAliveMembers();
        Player.RefreshInGameValues(numberOfEnemies);

        foreach (Character enemy in EnemyTeam.Members)
        {
            enemy.RefreshInGameValues(numberOfEnemies);
        }
    }

    public virtual void PlayerSubmitAbility(Ability ability)
    {
        playerAbilitySequence.AddOrOverclockAbility(ability, Player.AbilitySequencingStats.AbilityPoints);
        SubmitPlayerChangeAbilitySequence();
    }

    public virtual void PlayerSubmitTarget(Character target)
    {
        playerAbilitySequence.SetLastAbilityTarget(target);
        SubmitPlayerChangeAbilitySequence();
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
        List<AbilityResults> results = new List<AbilityResults>();

        foreach (AbilitySelection abilitySelection in sortedAbilitySequence)
        {
            RefreshAllCharacterInGameValues();
            AbilityResults result = StatCalculation.GetPlayerAbilityResult(abilitySelection, playerAbilitySequence, this);
            abilitySelections.Add(abilitySelection);
            results.Add(result);
        }

        pendingBattleChanges.Add(new PlayerDoAbilitySequence(Player, abilitySelections, results));

        EnemyTeam.CalculateTurnOrder(Player.DodgePriorities);

        SubmitChanges();
    }

    public virtual void DoEnemyAbilities()
    {
        foreach (EnemyCharacter enemy in EnemyTeam.EnemiesInTurnOrder)
        {
            if (enemy.IsAlive)
            {
                RefreshAllCharacterInGameValues();
                AbilityResults results = StatCalculation.GetEnemyAbilityResult(enemy.ChosenAbility, enemy, this);
                pendingBattleChanges.Add(new EnemyDoAbility(enemy.ChosenAbility, results));
            }
        }

        SubmitChanges();
    }

    public virtual void PlayerSubmitBack()
    {
        if (playerAbilitySequence != null)
        {
            playerAbilitySequence.StepBackInSequenceBuilding();
            SubmitPlayerChangeAbilitySequence();
        }
    }

    protected virtual void SubmitPlayerChangeAbilitySequence()
    {
        RefreshAllCharacterInGameValues();
        Player.RefreshAbilitySequencingStats(playerAbilitySequence);
        pendingBattleChanges.Add(new PlayerChangeAbilitySequence(playerAbilitySequence, Player.AbilitySequencingStats));
        SubmitChanges();
    }
}
