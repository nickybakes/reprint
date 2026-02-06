using System.Collections.Generic;
using UnityEngine;

public class BattleUIManager : MonoBehaviour
{

    private Canvas canvas;

    private RectTransform canvasRect;

    [SerializeField]
    private RectTransform gamePanel;

    [SerializeField]
    private CharacterHUDPanel playerHUDPanelPrefab;

    [SerializeField]
    private CharacterHUDPanel enemyHUDPanelPrefab;

    private CharacterHUDPanel playerHUDPanel;

    private List<CharacterHUDPanel> enemyHUDPanels;

    void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvasRect = GetComponent<RectTransform>();

        enemyHUDPanels = new List<CharacterHUDPanel>();
    }

    public void SpawnCharacterHUDPanel(Character character)
    {
        CharacterHUDPanel prefab = character.IsPlayerControlled ? playerHUDPanelPrefab : enemyHUDPanelPrefab;

        CharacterHUDPanel panel = Instantiate(prefab, gamePanel.transform).GetComponent<CharacterHUDPanel>();

        panel.SetUpHUDPanel(character, canvas);

        if (character.IsPlayerControlled)
        {
            playerHUDPanel = panel;
        }
        else
        {
            enemyHUDPanels.Add(panel);
        }
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
