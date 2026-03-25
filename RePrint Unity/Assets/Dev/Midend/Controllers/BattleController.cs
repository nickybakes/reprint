using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class BattleController : MonoBehaviour
{
    /// <summary>
    /// Reference to the Battle Manager in the scene.
    /// </summary>
    [SerializeField] private BattleManager battleManager;

    /// <summary>
    /// Reference to the Battle View in the scene.
    /// </summary>
    [SerializeField] private BattleView battleView;

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
        battleManager.PlayerSubmitBack();
    }

    public void OnCancel()
    {
        BackInput();
    }
}
