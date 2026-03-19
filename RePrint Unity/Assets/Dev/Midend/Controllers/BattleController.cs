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


    [SerializeField] private List<AbilityDisplay> abilityDisplays;
    [SerializeField] private List<TargetDisplay> targetDisplays;

    public void AddAbilityDisplays(List<Ability> abilities)
    {
        for (int i = 0; i < abilities.Count && i < abilityDisplays.Count; i++)
        {
            abilityDisplays[i].DisplayAbility(abilities[i], this);
        }
    }

    public void AddTargetDisplays(Team targets)
    {
        for (int i = 0; i < targets.Members.Count && i < targetDisplays.Count; i++)
        {
            targetDisplays[i].DisplayTarget(targets.Members[i], this);
        }
    }


    public void SubmitAbility(Ability ability)
    {
        battleManager.PlayerSubmitAbility(ability);
    }

    public void SubmitTarget(Character target)
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
