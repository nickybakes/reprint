using UnityEngine;
using UnityEngine.EventSystems;

public class BattleController : MonoBehaviour, IPointerClickHandler
{
    /// <summary>
    /// Reference to the Battle Manager in the scene.
    /// </summary>
    [SerializeField] private BattleManager battleManager;

    public void ActionSubmit()
    {

    }

    public void CharacterSubmit()
    {

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
