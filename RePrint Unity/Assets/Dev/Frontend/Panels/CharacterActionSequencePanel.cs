using System.Collections.Generic;
using UnityEngine;

public class CharacterActionSequencePanel : MonoBehaviour
{

    [SerializeField] private Transform listParent;
    [SerializeField] private CharacterActionSequenceItem listItemPrefab;
    [SerializeField] private GameObject listArrowPrefab;

    public void RefreshActionSequence()
    {
        // List<SelectedAction> sequence = BattleManager.battle.PlayerCreateActionSequenceBattleState.GetTrimmedSequence();
        // ClearActions();

        // for (int i = 0; i < sequence.Count; i++)
        // {
        //     if (sequence[i].enemyIndex != -1)
        //     {
        //         AddAction(sequence[i]);

        //         // add decorative arrow between actions
        //         if (i < sequence.Count - 1)
        //         {
        //             GameObject arrow = Instantiate(listArrowPrefab, listParent);
        //             arrow.SetActive(true);
        //         }
        //     }

        // }
    }

    // public void AddAction(SelectedAction action)
    // {
    //     CharacterActionSequenceItem item = Instantiate(listItemPrefab, listParent).GetComponent<CharacterActionSequenceItem>();
    //     item.Setup(action);
    // }

    public void ClearActions()
    {
        foreach (Transform child in listParent.transform)
        {
            Destroy(child.gameObject);
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
