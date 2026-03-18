using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterActionSequenceItem : MonoBehaviour
{

    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameLabel;
    [SerializeField] private TextMeshProUGUI overclockLabel;
    [SerializeField] private TextMeshProUGUI enemyLabel;

    // public void Setup(SelectedAction action)
    // {
    //     CharacterActionData actionData = BattleManager.battle.PlayerCharacter.GetAction(action.actionIndex);
    //     gameObject.SetActive(true);
    //     nameLabel.text = actionData.name;
    //     overclockLabel.text = "OC: " + action.overclock;
    //     enemyLabel.text = "E: " + action.enemyIndex;
    // }
}
