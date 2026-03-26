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
    private int currentBattleTurnIndex;

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

    public virtual void ConfirmPlayerAbilitySequence()
    {

    }

    public virtual void StartPlayerTurn()
    {
        playerAbilitySequence = new AbilitySequence();
        playerCharacter.RefillAbilityPoints();
        playerCharacter.RefreshAbilitySequencingStats(playerAbilitySequence);
        pendingBattleChanges.Add(new PlayerTurnStart(playerCharacter.AbilitySequencingStats));
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
        playerCharacter.ApplyAbilitySequencingStats();



        // Check if all enemies are eliminated. If not, next turn!
        StartPlayerTurn();
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
        playerCharacter.RefreshAbilitySequencingStats(playerAbilitySequence);
        pendingBattleChanges.Add(new PlayerChangeAbilitySequence(playerAbilitySequence, playerCharacter.AbilitySequencingStats));
        SubmitChanges();
    }

    // public void SetupBattle()
    // {
    //     // Get reference to battle scene setup data.
    //     battleSceneSetup = GameObject.FindWithTag("Battle Scene Setup").GetComponent<BattleSceneSetup>();

    //     battleUIManager = Instantiate(battleUIManagerPrefrab).GetComponent<BattleUIManager>();

    //     battleData = GameManager.game.GetBattleData();

    //     teams = new List<List<Character>>
    //     {
    //         new List<Character>(),
    //         new List<Character>()
    //     };


    //     // Instantiate the game objects for player and enemies. Also send them their specific data so they can set up themselves.

    //     SpawnCharacter(0, battleData.playerCharacterData, true, 0);

    //     for (int i = 0; i < battleData.enemyCharacterDatas.Length; i++)
    //     {
    //         SpawnCharacter(1, battleData.enemyCharacterDatas[i], false, i);
    //     }

    //     // Set spawn position and rotation of player and enemies

    //     teams[0][0].SetSpawnTransform(battleSceneSetup.PlayerSpawnPoint, battleSceneSetup.PlayerDirection);

    //     List<Vector3> enemySpawnPoints = battleSceneSetup.GetEnemySpawnPoints(teams[1].Count);

    //     for (int i = 0; i < teams[1].Count; i++)
    //     {
    //         teams[1][i].SetSpawnTransform(enemySpawnPoints[i], battleSceneSetup.EnemyDirection);
    //     }

    //     CurrentBattleState.StartState();
    // }

    // private void SpawnCharacter(int team, CharacterData data, bool isPlayerControlled, int index)
    // {
    //     GameObject characterGameObject = Instantiate(characterPrefab.gameObject);
    //     Character characterClassReference = characterGameObject.GetComponent<Character>();
    //     characterClassReference.SetupCharacter(data, isPlayerControlled, index);
    //     teams[team].Add(characterClassReference);
    //     battleUIManager.SpawnCharacterHUDPanel(characterClassReference);
    // }

    // public void SwitchBattleState(BattleStateIndex index)
    // {
    //     CurrentBattleState.EndState();

    //     currentBattleStateIndex = index;

    //     CurrentBattleState.StartState();
    // }

    public void BackInput()
    {
        // if (currentBattleStateIndex == BattleStateIndex.PlayerCreateAbilitySequence)
        // {
        //     playerCreateAbilitySequenceBattleState.Back();
        // }
    }
}
