using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class CharacterActionMenu : MonoBehaviour
{

    [SerializeField]
    private SplineContainer splineContainer;

    [SerializeField]
    private Transform buttonsParent;

    [SerializeField] private Transform actionStatsDisplay;

    [SerializeField] private CharacterActionButton buttonPrefab;

    [SerializeField][Range(0.0f, 1.0f)] float unselectedButtonSizeNormalized;
    [SerializeField][Range(0.0f, 1.0f)] float selectedButtonSizeNormalized;
    [SerializeField] Vector2 actionStatsOffset;

    [SerializeField] float rotationTop;
    [SerializeField] float rotationBottom;

    private List<CharacterActionButton> buttons;

    private float splineLength;

    private int currentSelectedButtonIndex;

    private int currentActiveButtonIndex = -1;
    private int currentActiveOverclock = -1;

    private float[] buttonOffsets;

    private RectTransform actionStatsRect;


    public int CurrentSelectedButtonIndex
    {
        set
        {
            currentSelectedButtonIndex = value;
            buttons[currentSelectedButtonIndex].transform.SetAsLastSibling();
        }
    }

    public void SubmitCharacterActionButton(int index)
    {
        BattleManager.battle.PlayerCreateActionSequenceBattleState.SubmitAction(index);
    }

    public void RefreshButtonStates()
    {
        List<SelectedAction> sequence = BattleManager.battle.PlayerCreateActionSequenceBattleState.SelectedActionSequence;
        if (sequence.Count == 0)
        {
            currentActiveButtonIndex = -1;
            currentActiveOverclock = -1;
            BattleManager.battle.ui.DisableEnemySelection();
        }
        else
        {
            SelectedAction action = sequence[sequence.Count - 1];
            currentActiveButtonIndex = action.actionIndex;
            currentActiveOverclock = action.overclock;

            if (action.enemyIndex != -1)
            {
                currentActiveButtonIndex = -1;
                currentActiveOverclock = -1;
                BattleManager.battle.ui.DisableEnemySelection();
            }
            else
            {
                BattleManager.battle.ui.EnableEnemySelection();
            }
        }


        for (int i = 0; i < buttons.Count; i++)
        {
            CharacterActionButton button = buttons[i];

            if (i == currentActiveButtonIndex)
            {
                button.RefreshButtonState(true, currentActiveOverclock);
            }
            else
            {
                button.RefreshButtonState(false, -1);
            }

        }
    }

    public void SetupActionMenu(Character character)
    {
        actionStatsRect = actionStatsDisplay.GetComponent<RectTransform>();

        splineLength = splineContainer.Spline.GetLength();

        buttons = new List<CharacterActionButton>(character.CharacterActions.Count);

        for (int i = 0; i < character.CharacterActions.Count; i++)
        {
            CharacterActionButton button = Instantiate(buttonPrefab, buttonsParent).GetComponent<CharacterActionButton>();
            button.name = "Action Button " + i;
            button.SetupActionButton(character.CharacterActions[i], i, this);
            buttons.Add(button);

            if (i == 0)
            {
                actionStatsRect.SetParent(button.actionStatsParent);
            }
        }

        buttonOffsets = new float[buttons.Count];

        UpdateButtonPositions();
    }

    public void OpenMenu()
    {
        // We can add a proper animation for this later
        gameObject.SetActive(true);
        RefreshButtonStates();
    }

    public void CloseMenu()
    {
        // We can add a proper animation for this later
        gameObject.SetActive(false);
    }

    public void UpdateButtonPositions()
    {
        // Calculate offset
        for (int i = 0; i < buttons.Count; i++)
        {
            buttonOffsets[i] = 0;
            for (int j = 0; j < buttons.Count; j++)
            {
                CharacterActionButton button = buttons[j];
                float offset = (selectedButtonSizeNormalized - unselectedButtonSizeNormalized) * button.sizeTransition * .5f;
                offset *= Math.Sign(i - j);
                buttonOffsets[i] += offset;
            }
        }

        // Calculate starting point
        int halfCount = buttons.Count / 2;
        float evenOffset = 0;
        if (buttons.Count % 2 == 0)
        {
            evenOffset = unselectedButtonSizeNormalized * .5f;
        }
        float startPoint = evenOffset + .5f - (halfCount * unselectedButtonSizeNormalized);

        // Combine starting point and offset to position the buttons
        for (int i = 0; i < buttons.Count; i++)
        {
            CharacterActionButton current = buttons[i];
            float point = startPoint + (i * unselectedButtonSizeNormalized) + buttonOffsets[i];
            Vector3 position = splineContainer.Spline.EvaluatePosition(point);
            position.z = 0;
            current.GetRect().anchoredPosition = position;
            current.GetRect().localRotation = Quaternion.AngleAxis(Mathf.Lerp(rotationTop, rotationBottom, point), Vector3.forward);
        }

        actionStatsRect.anchoredPosition = actionStatsOffset;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateButtonPositions();
    }
}
