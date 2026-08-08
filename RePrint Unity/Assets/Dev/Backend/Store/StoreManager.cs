using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StoreManager : MonoBehaviour
{
    /// <summary>
    /// Sends out the current list of game changes in this event.
    /// </summary>
    [SerializeField] private UnityEvent<List<StoreStateChange>> submitChangesEvent;

    /// <summary>
    /// List of changes to the battle that have been calculated but not shown to the player yet.
    /// </summary>
    private List<StoreStateChange> pendingStoreChanges;

    /// <summary>
    /// Invokes the Submit Changes Event and starts a new list of changes.
    /// </summary>
    public virtual void SubmitChanges()
    {
        submitChangesEvent.Invoke(pendingStoreChanges);
        pendingStoreChanges = new List<StoreStateChange>();
    }

    public void SetupStore(PlayerCharacter player)
    {
        pendingStoreChanges = new List<StoreStateChange>
        {
            new StoreInitialized(player)
        };

        SubmitChanges();
    }

    public void PlayerSubmitGoToBattle()
    {
        GameManager.game.GoToBattleScene();
    }
}
