using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleController : MonoBehaviour, IPointerClickHandler
{
    /// <summary>
    /// Reference to the Battle Manager in the scene.
    /// </summary>
    [SerializeField] private BattleManager battleManager;

    /// <summary>
    /// Reference to the Battle View in the scene.
    /// </summary>
    [SerializeField] private BattleView battleView;


    [SerializeField] private List<CharacterActionDisplay> actionDisplays;
    [SerializeField] private List<CharacterTargetDisplay> targetDisplays;

    // [SerializeField] private ActionButton actionButtons;

    public void AddActionDisplays(List<CharacterAction> actions)
    {
        for (int i = 0; i < actions.Count && i < actionDisplays.Count; i++)
        {
            actionDisplays[i].DisplayAction(actions[i], this);
        }
    }

    public void AddTargetDisplays(Team targets)
    {
        for (int i = 0; i < targets.Members.Count && i < targetDisplays.Count; i++)
        {
            targetDisplays[i].DisplayTarget(targets.Members[i], this);
        }
    }


    public void ActionSubmit(CharacterAction action)
    {
        battleManager.PlayerSubmitAction(action);
    }

    public void TargetSubmit(Character target)
    {
        battleManager.PlayerSubmitTarget(target);
    }

    public void BackInput()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            BackInput();
        }
    }
}
