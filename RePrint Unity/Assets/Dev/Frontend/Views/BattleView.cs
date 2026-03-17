using System.Collections.Generic;
using UnityEngine;

public class BattleView : MonoBehaviour
{
    /// <summary>
    /// Reference to the Battle Manager in the scene.
    /// </summary>
    [SerializeField] private BattleManager battleManager;

    /// <summary>
    /// Reference to a full screen raycast target to block player interactions.
    /// </summary>
    [SerializeField] private GameObject uiRaycastShield;

    /// <summary>
    /// A queue of game changes that the view manager should run through and display.
    /// </summary>
    private List<BattleStateChange> battleStateChangesQueue;

    /// <summary>
    /// How long the current game action should take, in seconds.
    /// </summary>
    private float timeForCurrentChange;

    /// <summary>
    /// How long the current game action has taken, in seconds.
    /// </summary>
    private float currentChangeTime;

    /// <summary>
    /// The index of the current game change that the view manager is on.
    /// </summary>
    private int currentChangeIndex;

    /// <summary>
    /// Whether the view manager should be working its way through displaying game changes.
    /// </summary>
    private bool parsingGameChanges;


    /// <summary>
    /// Initialize lists and disable player interactions on Awake
    /// </summary>
    void Awake()
    {
        battleStateChangesQueue = new List<BattleStateChange>();
        parsingGameChanges = true;
        // DisablePlayerInteractions();
    }

    /// <summary>
    /// Captures the current list of game changes and begins displaying them.
    /// </summary>
    /// <param name="changes">The changes to display.</param>
    public void CaptureAndDisplayGameChanges(List<BattleStateChange> changes)
    {
        // If not currently rendering changes, capture the new list itself.
        if (currentChangeIndex >= battleStateChangesQueue.Count)
        {
            battleStateChangesQueue = changes;
            currentChangeIndex = 0;
        }
        else
        {
            // If currently rendering changes, add the new changes onto the end.
            battleStateChangesQueue.AddRange(changes);
        }

        // Don't let the player interact while displaying changes.
        DisablePlayerInteractions();

        currentChangeTime = timeForCurrentChange;
    }

    /// <summary>
    /// Enables the full screen UI raycast object that blocks player interactions.
    /// </summary>
    public void DisablePlayerInteractions()
    {
        uiRaycastShield.SetActive(true);
    }

    /// <summary>
    /// Disables the full screen UI raycast object that blocks player interactions.
    /// </summary>
    public void EnablePlayerInteractions()
    {
        uiRaycastShield.SetActive(false);
    }

    /// <summary>
    /// Every frame try to parse game changes that still need to be parsed.
    /// </summary>
    void Update()
    {
        if (parsingGameChanges)
        {
            currentChangeTime += Time.deltaTime;
            while (currentChangeTime >= timeForCurrentChange && currentChangeIndex < battleStateChangesQueue.Count && parsingGameChanges)
            {
                ParseGameStateChange(battleStateChangesQueue[currentChangeIndex]);
                currentChangeTime = 0;
                timeForCurrentChange = 0;

                // switch (changesThisTurn[currentChangeIndex].ChangeTime)
                // {
                //     case GameStateChangeTime.Instant:
                //         timeForCurrentChange = 0;
                //         break;
                //     case GameStateChangeTime.Short:
                //         timeForCurrentChange = timeBetweenShortGameChangeAction;
                //         break;
                //     case GameStateChangeTime.Medium:
                //         timeForCurrentChange = timeBetweenMediumGameChangeAction;
                //         break;
                //     case GameStateChangeTime.Long:
                //         timeForCurrentChange = timeBetweenLongGameChangeAction;
                //         break;
                // }

                currentChangeIndex++;
                if (currentChangeIndex >= battleStateChangesQueue.Count)
                {
                    EnablePlayerInteractions();
                }
            }
        }
    }


    /// <summary>
    /// Controls the objects displayed on screen depending on the data from a game state change.
    /// </summary>
    /// <param name="change">The game state change to parse.</param>
    private void ParseGameStateChange(GameStateChange change)
    {
    }
}
