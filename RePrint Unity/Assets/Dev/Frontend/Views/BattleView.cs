using System.Collections.Generic;
using UnityEngine;

public class BattleView : MonoBehaviour
{
    /// <summary>
    /// Reference to the Battle Manager in the scene.
    /// </summary>
    [SerializeField] private BattleManager battleManager;

    /// <summary>
    /// Reference to the Battle Controller in the scene.
    /// </summary>
    [SerializeField] private BattleController battleController;

    /// <summary>
    /// Reference to a full screen raycast target to block player interactions.
    /// </summary>
    [SerializeField] private GameObject uiRaycastShield;

    [SerializeField] private BattleTimingProfile battleTimingProfile;


    [field: SerializeField] public RectTransform CanvasRect { get; private set; }

    [field: SerializeField] public BattleStatsPanel BattleStatsPanel { get; private set; }

    [field: SerializeField] public AbilityDisplayGroup PlayerAbilityDisplayGroup { get; private set; }

    [field: SerializeField] public CharacterFigureGroup PlayerFigureGroup { get; private set; }

    [field: SerializeField] public CharacterFigureGroup EnemyFigureGroup { get; private set; }
    [field: SerializeField] public BetterButton PlayerConfirmSequenceButton { get; private set; }
    [field: SerializeField] public AbilitySequenceGroup PlayerAbilitySequenceGroup { get; private set; }

    [field: SerializeField] public TextDisplay TurnDisplay { get; private set; }


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
        DisablePlayerInteractions();
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
                battleStateChangesQueue[currentChangeIndex].ParseChange(this, battleController);

                currentChangeTime = 0;

                timeForCurrentChange = battleTimingProfile.GetTime(battleStateChangesQueue[currentChangeIndex]);

                currentChangeIndex++;
                if (currentChangeIndex >= battleStateChangesQueue.Count)
                {
                    EnablePlayerInteractions();
                }
            }
        }
    }

    public Vector3 WorldToCanvasPoint(Vector3 position)
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(position);
        screenPosition.x *= CanvasRect.rect.width / (float)Camera.main.pixelWidth;
        screenPosition.y *= CanvasRect.rect.height / (float)Camera.main.pixelHeight;
        screenPosition.x = screenPosition.x - CanvasRect.sizeDelta.x / 2f;
        screenPosition.y = screenPosition.y - CanvasRect.sizeDelta.y / 2f;
        return screenPosition;
    }
}
