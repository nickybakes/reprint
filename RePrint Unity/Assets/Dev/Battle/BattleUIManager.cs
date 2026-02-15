using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleUIManager : MonoBehaviour, IPointerClickHandler
{

    public static BattleUIManager manager;

    private Canvas canvas;

    private RectTransform canvasRect;

    [SerializeField] private RectTransform gamePanel;

    [SerializeField] private CharacterHUDPanel playerHUDPanelPrefab;

    [SerializeField] private CharacterHUDPanel enemyHUDPanelPrefab;

    [SerializeField] private CharacterActionSequencePanel actionSequencePanel;

    private CharacterHUDPanel playerHUDPanel;

    private List<CharacterHUDPanel> enemyHUDPanels;

    void Awake()
    {
        manager = this;

        canvas = GetComponent<Canvas>();
        canvasRect = GetComponent<RectTransform>();
        canvas.worldCamera = Camera.main;
        canvas.planeDistance = 1;

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

    public Vector3 WorldToCanvasPoint(Vector3 position)
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(position);
        screenPosition.x *= canvasRect.rect.width / (float)Camera.main.pixelWidth;
        screenPosition.y *= canvasRect.rect.height / (float)Camera.main.pixelHeight;
        screenPosition.x = screenPosition.x - canvasRect.sizeDelta.x / 2f;
        screenPosition.y = screenPosition.y - canvasRect.sizeDelta.y / 2f;
        return screenPosition;
    }

    public void OpenPlayerActionMenu()
    {
        playerHUDPanel.OpenActionMenu();
    }

    public void ClosePlayerActionMenu()
    {
        playerHUDPanel.CloseActionMenu();
    }

    /// <summary>
    /// Refreshed the action menu AND the sequence panel
    /// </summary>
    public void RefreshPlayerActionMenu()
    {
        playerHUDPanel.RefreshActionMenu();
        actionSequencePanel.RefreshActionSequence();
    }

    public void OpenActionSequencePanel()
    {
        actionSequencePanel.RefreshActionSequence();
        actionSequencePanel.gameObject.SetActive(true);
    }

    public void CloseActionSequencePanel()
    {
        actionSequencePanel.gameObject.SetActive(false);
    }

    public void EnableEnemySelection()
    {
        foreach (CharacterHUDPanel enemyHUD in enemyHUDPanels)
        {
            enemyHUD.EnableSelection();
        }
    }

    public void DisableEnemySelection()
    {
        foreach (CharacterHUDPanel enemyHUD in enemyHUDPanels)
        {
            enemyHUD.DisableSelection();
        }
    }

    public void SubmitPlayerActionSequence()
    {

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            BattleManager.battle.BackInput();
        }
    }
}
