using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum BattleStateIndex
{
    Intro = 0,

    PlayerCreateActionSequence = 1
}

public class BattleManager : MonoBehaviour
{

    public static BattleManager battle;

    /// <summary>
    /// Sends out the current list of game changes in this event.
    /// </summary>
    [SerializeField] private UnityEvent<List<BattleStateChange>> submitChangesEvent;

    [SerializeField]
    private Character characterPrefab;

    [SerializeField]
    private BattleUIManager battleUIManagerPrefrab;

    /// <summary>
    /// The character objects that are active in this battle and which side they are on.
    /// </summary>
    private List<List<Character>> teams;

    /// <summary>
    /// The number of the current turn the battle is in. Increments once all teams have done an action and loops back to the first team.
    /// </summary>
    private int currentBattleTurnIndex;

    /// <summary>
    /// The team that is currently chosing/doing their actions for this turn in battle.
    /// </summary>
    private int currentTeamInTurn;

    /// <summary>
    /// The data for what characters are in this battle scenario
    /// </summary>
    private BattleData battleData;

    /// <summary>
    /// The data for how to set up the scene of the battle.
    /// </summary>
    private BattleSceneSetup battleSceneSetup;

    /// <summary>
    /// The manager for the UI of the battle.
    /// </summary>
    private BattleUIManager battleUIManager;

    private BattleState[] battleStates;

    private BattleStateIndex currentBattleStateIndex;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        battle = this;

        // battleStates = new BattleState[] { introBattleState, playerCreateActionSequenceBattleState };

        // SetupBattle();
    }

    public BattleUIManager ui
    {
        get
        {
            return battleUIManager;
        }
    }

    public BattleState CurrentBattleState
    {
        get
        {
            return battleStates[(int)currentBattleStateIndex];
        }
    }

    public Character PlayerCharacter
    {
        get
        {
            return teams[0][0];
        }
    }

    public BattleState GetBattleState(BattleStateIndex index)
    {
        return battleStates[(int)index];
    }

    public void SetupBattle()
    {
        // Get reference to battle scene setup data.
        battleSceneSetup = GameObject.FindWithTag("Battle Scene Setup").GetComponent<BattleSceneSetup>();

        battleUIManager = Instantiate(battleUIManagerPrefrab).GetComponent<BattleUIManager>();

        battleData = GameManager.game.GetBattleData();

        teams = new List<List<Character>>
        {
            new List<Character>(),
            new List<Character>()
        };


        // Instantiate the game objects for player and enemies. Also send them their specific data so they can set up themselves.

        SpawnCharacter(0, battleData.playerCharacterData, true, 0);

        for (int i = 0; i < battleData.enemyCharacterDatas.Length; i++)
        {
            SpawnCharacter(1, battleData.enemyCharacterDatas[i], false, i);
        }

        // Set spawn position and rotation of player and enemies

        teams[0][0].SetSpawnTransform(battleSceneSetup.PlayerSpawnPoint, battleSceneSetup.PlayerDirection);

        List<Vector3> enemySpawnPoints = battleSceneSetup.GetEnemySpawnPoints(teams[1].Count);

        for (int i = 0; i < teams[1].Count; i++)
        {
            teams[1][i].SetSpawnTransform(enemySpawnPoints[i], battleSceneSetup.EnemyDirection);
        }

        CurrentBattleState.StartState();
    }

    private void SpawnCharacter(int team, CharacterData data, bool isPlayerControlled, int index)
    {
        GameObject characterGameObject = Instantiate(characterPrefab.gameObject);
        Character characterClassReference = characterGameObject.GetComponent<Character>();
        characterClassReference.SetupCharacter(data, isPlayerControlled, index);
        teams[team].Add(characterClassReference);
        battleUIManager.SpawnCharacterHUDPanel(characterClassReference);
    }

    public void SwitchBattleState(BattleStateIndex index)
    {
        CurrentBattleState.EndState();

        currentBattleStateIndex = index;

        CurrentBattleState.StartState();
    }

    public void BackInput()
    {
        // if (currentBattleStateIndex == BattleStateIndex.PlayerCreateActionSequence)
        // {
        //     playerCreateActionSequenceBattleState.Back();
        // }
    }
}
