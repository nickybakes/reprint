using System.Collections.Generic;
using UnityEngine;

public class BattleSceneSetup : MonoBehaviour
{

    /// <summary>
    /// A game object representing the spawn point of the player.
    /// </summary>
    [SerializeField]
    private GameObject playerSpawnPoint;

    /// <summary>
    /// A list of game objects that contain spawn points as child game objects. The index in the list is for different numbers of enemies.
    /// </summary>
    [SerializeField]
    private List<GameObject> enemySpawnPointContainers;

    /// <summary>
    /// The angle, in degrees, that the player's model should face when spawned in.
    /// </summary>
    [SerializeField]
    private float playerDirection;

    /// <summary>
    /// The angle, in degrees, that the enemy models should face when spawned in.
    /// </summary>
    [SerializeField]
    private float enemyDirection;

    public float PlayerDirection
    {
        get
        {
            return playerDirection;
        }
    }

    public Vector3 PlayerSpawnPoint
    {
        get
        {
            return playerSpawnPoint.transform.position;
        }
    }

    public float EnemyDirection
    {
        get
        {
            return enemyDirection;
        }
    }

    public List<Vector3> GetEnemySpawnPoints(int numEnemies)
    {
        List<Vector3> spawnPoints = new List<Vector3>();
        GameObject container = enemySpawnPointContainers[numEnemies - 1];

        for (int i = 0; i < container.transform.childCount; i++)
        {
            spawnPoints.Add(container.transform.GetChild(i).position);
        }

        return spawnPoints;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
