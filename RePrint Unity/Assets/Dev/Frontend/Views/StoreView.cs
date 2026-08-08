using System.Collections.Generic;
using UnityEngine;

public class StoreView : MonoBehaviour
{

    /// <summary>
    /// Reference to the Store Manager in the scene.
    /// </summary>
    [SerializeField] private StoreManager storeManager;

    /// <summary>
    /// Reference to the Store Controller in the scene.
    /// </summary>
    [SerializeField] private StoreController storeController;

    [field: SerializeField] public StoreTimingProfile StoreTimingProfile { get; private set; }

    /// <summary>
    /// Reference to a full screen raycast target to block player interactions.
    /// </summary>
    [SerializeField] private GameObject uiRaycastShield;

    [SerializeField] private CharacterStatsPanel playerStatsPanel;


    /// <summary>
    /// A queue of game changes that the view manager should run through and display.
    /// </summary>
    private List<StoreStateChange> storeStateChangesQueue;

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

    private bool updatingGameChange;

    private StoreStateChange stateChangeToUpdate;

    void Awake()
    {
        storeStateChangesQueue = new List<StoreStateChange>();
        parsingGameChanges = true;
        DisablePlayerInteractions();
    }

    /// <summary>
    /// Captures the current list of game changes and begins displaying them.
    /// </summary>
    /// <param name="changes">The changes to display.</param>
    public void CaptureAndDisplayGameChanges(List<StoreStateChange> changes)
    {
        // If not currently rendering changes, capture the new list itself.
        if (currentChangeIndex >= storeStateChangesQueue.Count)
        {
            storeStateChangesQueue = changes;
            currentChangeIndex = 0;
        }
        else
        {
            // If currently rendering changes, add the new changes onto the end.
            storeStateChangesQueue.AddRange(changes);
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


    public void SwitchToUpdatingStateChange(StoreStateChange stateChange)
    {
        parsingGameChanges = false;
        updatingGameChange = true;
        stateChangeToUpdate = stateChange;
    }

    public void SwitchToParsing()
    {
        parsingGameChanges = true;
        updatingGameChange = false;
    }

    /// <summary>
    /// Every frame try to parse game changes that still need to be parsed.
    /// </summary>
    void Update()
    {
        if (parsingGameChanges)
        {
            currentChangeTime += Time.deltaTime;
            while (currentChangeTime >= timeForCurrentChange && currentChangeIndex < storeStateChangesQueue.Count && parsingGameChanges)
            {
                storeStateChangesQueue[currentChangeIndex].ParseChange(this, storeController);

                currentChangeTime = 0;

                if (StoreTimingProfile != null)
                    timeForCurrentChange = StoreTimingProfile.GetTime(storeStateChangesQueue[currentChangeIndex]);

                currentChangeIndex++;
                if (currentChangeIndex >= storeStateChangesQueue.Count)
                {
                    EnablePlayerInteractions();
                }
            }
        }

        if (updatingGameChange)
        {
            stateChangeToUpdate.Update(this, storeController);
        }
    }

    public void UpdateStats(Character character)
    {
        playerStatsPanel.UpdateStats(character.Stats);
    }
}
