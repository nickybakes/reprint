using UnityEngine;

public class CharacterHUDPanel : MonoBehaviour
{

    [SerializeField]
    private StatDisplay healthDisplay;

    private Character characterReference;

    /// <summary>
    /// The currently displayed states. When a character's stats get updated, we can reference these old stats to
    /// do unique effects like playing specific HUD animations for losing or gaining health.
    /// </summary>
    private CharacterStats displayedStats;

    private Canvas canvas;

    public void SetUpHUDPanel(Character character, Canvas _canvas)
    {
        characterReference = character;
        canvas = _canvas;
        UpdateStats(characterReference.Stats, true);
        UpdatePosition();
    }

    public void UpdatePosition()
    {
        Vector3 position = Camera.main.WorldToScreenPoint(characterReference.Visual.MeshCenter);
        transform.position = position;
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
