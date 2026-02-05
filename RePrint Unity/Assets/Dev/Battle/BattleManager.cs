using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{

    public static BattleManager battle;

    [SerializeField]
    private Character characterPrefab;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BattleManager.battle = this;

        SetupBattle();
    }

    public void SetupBattle()
    {
        // Get reference to battle scene setup data.
        battleSceneSetup = GameObject.FindWithTag("Battle Scene Setup").GetComponent<BattleSceneSetup>();

        battleData = GameManager.game.GetBattleData();

        teams = new List<List<Character>>
        {
            new List<Character>(),
            new List<Character>()
        };


        // Instantiate the game objects for player and enemies. Also send them their specific data so they can set up themselves.

        SpawnCharacter(0, battleData.playerCharacterData);

        foreach (CharacterData data in battleData.enemyCharacterDatas)
        {
            SpawnCharacter(1, data);
        }


        // Set spawn position and rotation of player and enemies

        teams[0][0].SetSpawnTransform(battleSceneSetup.PlayerSpawnPoint, battleSceneSetup.PlayerDirection);

        List<Vector3> enemySpawnPoints = battleSceneSetup.GetEnemySpawnPoints(teams[1].Count);

        for (int i = 0; i < teams[1].Count; i++)
        {
            teams[1][i].SetSpawnTransform(enemySpawnPoints[i], battleSceneSetup.EnemyDirection);
        }
    }

    private void SpawnCharacter(int team, CharacterData data)
    {
        GameObject characterGameObject = Instantiate(characterPrefab.gameObject);
        Character characterClassReference = characterGameObject.GetComponent<Character>();
        characterClassReference.SetupCharacter(data);
        teams[team].Add(characterClassReference);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
