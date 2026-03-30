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

    private Character playerCharacter;

    private Team enemyTeam;

    /// <summary>
    /// The number of the current turn the battle is in. Increments once all teams have done an ability and loops back to the first team.
    /// </summary>
    private int turnIndex = -1;

    /// <summary>
    /// The data for what characters are in this battle scenario
    /// </summary>
    private BattleData battleData;

    /// <summary>
    /// The data for how to set up the scene of the battle.
    /// </summary>
    private BattleSceneSetup battleSceneSetup;

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
        turnIndex = -1;
        playerCharacter = new Character(data.playerCharacterData);

        enemyTeam = new Team();

        foreach (CharacterData enemyData in data.enemyCharacterDatas)
        {
            enemyTeam.AddMember(new Character(enemyData));
        }

        pendingBattleChanges = new List<BattleStateChange>
        {
            new BattleInitialized(playerCharacter, enemyTeam)
        };
        SubmitChanges();

        StartPlayerTurn();
    }

    public virtual void StartPlayerTurn()
    {
        playerAbilitySequence = new AbilitySequence();
        turnIndex++;
        playerCharacter.RefillAbilityPoints();
        playerCharacter.ResetDodge();
        playerCharacter.RefreshAbilitySequencingStats(playerAbilitySequence);
        RefreshAllCharacterIncomingValues();
        pendingBattleChanges.Add(new PlayerTurnStart(playerCharacter.AbilitySequencingStats, turnIndex));
        SubmitChanges();
    }

    public void RefreshAllCharacterIncomingValues()
    {
        int numberOfEnemies = enemyTeam.GetNumberOfAliveMembers();
        playerCharacter.RefreshIncomingValues(numberOfEnemies);

        foreach (Character enemy in enemyTeam.Members)
        {
            enemy.RefreshIncomingValues(numberOfEnemies);
        }
    }

    public virtual void PlayerSubmitAbility(Ability ability)
    {
        playerAbilitySequence.AddOrOverclockAbility(ability, playerCharacter.AbilitySequencingStats.AbilityPoints);
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

        // Check if all enemies are eliminated. If not, next turn!

        StartPlayerTurn();
    }

    public virtual void DoPlayerAbilitySequence()
    {
        playerCharacter.ApplyAbilitySequencingStats();

        foreach (AbilitySelection abilitySelection in playerAbilitySequence.Sequence)
        {
            RefreshAllCharacterIncomingValues();
            AbilityResults result = StatCalculation.GetPlayerAbilityResult(abilitySelection, playerAbilitySequence, playerCharacter, enemyTeam);
            pendingBattleChanges.Add(new PlayerDoAbility(abilitySelection, result));
        }

        enemyTeam.CalculateTurnOrder(playerCharacter.DodgePriorities);

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
        RefreshAllCharacterIncomingValues();
        playerCharacter.RefreshAbilitySequencingStats(playerAbilitySequence);
        pendingBattleChanges.Add(new PlayerChangeAbilitySequence(playerAbilitySequence, playerCharacter.AbilitySequencingStats));
        SubmitChanges();
    }
}
