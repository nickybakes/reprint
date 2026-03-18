using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum BattleStateIndex
{
    Intro = 0,

    PlayerCreateActionSequence = 1
}

public enum TeamIndex
{
    Player,
    Enemy,
}

public class BattleManager : MonoBehaviour
{
    /// <summary>
    /// Sends out the current list of game changes in this event.
    /// </summary>
    [SerializeField] private UnityEvent<List<BattleStateChange>> submitChangesEvent;

    [SerializeField] private BattleUIManager battleUIManagerPrefrab;

    /// <summary>
    /// List of changes to the battle that have been calculated but not shown to the player yet.
    /// </summary>
    private List<BattleStateChange> pendingBattleChanges;

    private Character playerCharacter;
    private Team enemyTeam;

    private List<Team> teams;

    /// <summary>
    /// The number of the current turn the battle is in. Increments once all teams have done an action and loops back to the first team.
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
        teams = new List<Team>(2);

        playerCharacter = new Character(data.playerCharacterData);
        teams.Add(new Team(playerCharacter));

        enemyTeam = new Team();
        teams.Add(enemyTeam);

        foreach (CharacterData enemyData in data.enemyCharacterDatas)
        {
            enemyTeam.AddMember(new Character(enemyData));
        }

        pendingBattleChanges = new List<BattleStateChange>
        {
            new BattleInitialized(playerCharacter, enemyTeam)
        };
        SubmitChanges();
    }


    public virtual void PlayerSubmitAction(CharacterAction action)
    {
        Debug.Log("Player submit action " + action.Name);
    }

    public virtual void PlayerSubmitTarget(Character target)
    {
        Debug.Log("Player submit target " + target.Name);
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
        // if (currentBattleStateIndex == BattleStateIndex.PlayerCreateActionSequence)
        // {
        //     playerCreateActionSequenceBattleState.Back();
        // }
    }
}
