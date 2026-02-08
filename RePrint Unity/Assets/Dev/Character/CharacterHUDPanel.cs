using UnityEngine;

public class CharacterHUDPanel : MonoBehaviour
{
    [SerializeField] private StatDisplay healthDisplay;

    [SerializeField] private CharacterActionMenu actionMenu;

    private Character characterReference;

    /// <summary>
    /// The currently displayed states. When a character's stats get updated, we can reference these old stats to
    /// do unique effects like playing specific HUD animations for losing or gaining health.
    /// </summary>
    private CharacterStats displayedStats;

    private Canvas canvas;

    private RectTransform canvasRect;

    private RectTransform rect;

    private Vector3 screenPosition;

    public void SetUpHUDPanel(Character character, Canvas _canvas)
    {
        characterReference = character;
        canvas = _canvas;
        canvasRect = canvas.GetComponent<RectTransform>();
        rect = GetComponent<RectTransform>();

        if (character.IsPlayerControlled)
        {
            actionMenu.SetupActionMenu(character);
        }

        UpdateStats(characterReference.Stats, true);
        UpdatePosition();
    }

    public void UpdatePosition()
    {
        rect.anchoredPosition = BattleUIManager.manager.WorldToCanvasPoint(characterReference.Visual.MeshCenter);
    }

    public void UpdateStats(CharacterStats newStats, bool noAnimations = false)
    {
        healthDisplay.UpdateStatDisplay(newStats.health, newStats.healthMax);

        displayedStats = newStats;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }
}
