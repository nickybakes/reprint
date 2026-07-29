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
    public int TurnIndexInWave { get; private set; } = -1;
    public int WaveIndex { get; private set; } = -1;


    /// <summary>
    /// The data for what characters are in this battle scenario
    /// </summary>
    private BattleData battleData;

    private AbilitySequence playerAbilitySequence;

    public List<TurnResults> turnHistory;
    public TurnResults currentTurnResults;

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
        turnHistory = new List<TurnResults>();
        TurnIndex = -1;
        TurnIndexInWave = -1;
        WaveIndex = -1;
        Player = new PlayerCharacter(data.playerCharacterData, data.playerMods, this);

        EnemyTeam = new EnemyTeam();

        foreach (EnemyData enemyData in data.enemyDatas)
        {
            EnemyTeam.AddMember(new EnemyCharacter(enemyData, this));
        }

        pendingBattleChanges = new List<BattleStateChange>
        {
            new BattleInitialized(Player, EnemyTeam)
        };

        StartWave();

        SubmitChanges();

        StartPlayerTurn();
    }

    public void StartWave()
    {
        WaveIndex++;
        TurnIndexInWave = -1;
    }

    public virtual void StartPlayerTurn()
    {
        playerAbilitySequence = new AbilitySequence();
        TurnIndex++;
        TurnIndexInWave++;
        EnemyTeam.ResetTurnPriorities();
        EnemyTeam.ResetForTurn();
        Player.ResetForTurn();

        DoPlayerMods(GameEvent.PlayerTurnStart);

        EnemyTeam.SetTurnStats();
        Player.SetTurnStats();

        Player.Stats.AbilityPoints = Player.Stats.AbilityPointsMax;

        Player.RefreshAbilitySequencingStats(playerAbilitySequence, this);

        foreach (EnemyCharacter enemy in EnemyTeam.Enemies)
        {
            if (enemy.IsAlive)
            {
                enemy.DecideAbility(this);
                pendingBattleChanges.Add(new EnemyDecideAbility(enemy.ChosenAbility, enemy));
            }
        }

        pendingBattleChanges.Add(new PlayerTurnStart(this, TurnIndex));
        SubmitChanges();
    }

    public virtual void DoPlayerMods(GameEvent _gameEvent)
    {
        GameValues gameValues = new GameValues
        {
            battleManager = this,
            activator = Player,
            target = null,
            gameEvent = _gameEvent,
            abilitySequence = playerAbilitySequence,
        };

        List<ModResult> modResults = new List<ModResult>();
        StatChangeBreakdown statChangeBreakdown = new StatChangeBreakdown(null, modResults);

        Player.CalculateStatChangesFromMods(gameValues, statChangeBreakdown);
        statChangeBreakdown.ApplyStatChanges(Player, EnemyTeam);
    }

    public virtual void PlayerSubmitAbility(Ability ability)
    {
        AbilitySequenceChangeType changeType = playerAbilitySequence.AddOrOverclockAbility(ability, Player.Stats.AbilityPoints);
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

        currentTurnResults = new TurnResults(Player, EnemyTeam)
        {
            playerDoAbilitySequence = DoPlayerAbilitySequence(),
            enemyDoAbilities = DoEnemyAbilities()
        };

        currentTurnResults.CalculateStatsAfter(Player, EnemyTeam);
        turnHistory.Add(currentTurnResults);

        // TODO: Check if all enemies are eliminated. If not, next turn!

        pendingBattleChanges.Add(new BeforePlayerTurnStart(this));
        SubmitChanges();

        StartPlayerTurn();
    }

    public virtual PlayerDoAbilitySequence DoPlayerAbilitySequence()
    {
        List<AbilitySelection> sortedAbilitySequence = playerAbilitySequence.GetSortedSequence();

        Player.CurrentCombo = 0;
        Player.TotalCombo = 0;

        StatCalculation.CheckAbilitySequence(sortedAbilitySequence, playerAbilitySequence, this);

        Player.TotalCombo = StatCalculation.GetTotalCombo(sortedAbilitySequence);

        List<AbilitySelection> abilitySelections = new List<AbilitySelection>();
        List<StatChangeBreakdown> statChangeBreakdowns = new List<StatChangeBreakdown>();

        Player.CurrentCombo = 0;
        int abilitySeqIndex = 0;

        foreach (AbilitySelection abilitySelection in sortedAbilitySequence)
        {
            StatChangeBreakdown result = StatCalculation.GetPlayerAbilityStatChangeBreakdown(abilitySelection, abilitySeqIndex, playerAbilitySequence, this);
            abilitySelections.Add(abilitySelection);
            statChangeBreakdowns.Add(result);
            abilitySeqIndex++;
        }

        Player.ResetTempStats();

        PlayerDoAbilitySequence results = new PlayerDoAbilitySequence(Player, abilitySelections, statChangeBreakdowns);

        pendingBattleChanges.Add(results);

        EnemyTeam.CalculateTurnOrder();

        SubmitChanges();

        return results;
    }

    public virtual List<EnemyDoAbility> DoEnemyAbilities()
    {
        List<EnemyDoAbility> enemyDoAbilities = new List<EnemyDoAbility>();
        foreach (EnemyCharacter enemy in EnemyTeam.EnemiesInTurnOrder)
        {
            if (enemy.IsAlive)
            {
                StatChangeBreakdown results = StatCalculation.GetEnemyAbilityStatChangeBreakdown(enemy.ChosenAbility, enemy, this);
                EnemyDoAbility enemyDoAbility = new EnemyDoAbility(enemy.ChosenAbility, results);
                pendingBattleChanges.Add(enemyDoAbility);
                enemyDoAbilities.Add(enemyDoAbility);
            }
        }

        SubmitChanges();
        return enemyDoAbilities;
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
        Player.RefreshAbilitySequencingStats(playerAbilitySequence, this);
        pendingBattleChanges.Add(new PlayerChangeAbilitySequence(playerAbilitySequence, this, changeType));
        SubmitChanges();
    }
}
