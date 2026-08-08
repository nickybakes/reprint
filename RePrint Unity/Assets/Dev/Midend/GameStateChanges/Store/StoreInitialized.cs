using UnityEngine;


public class StoreInitialized : StoreStateChange
{

    private PlayerCharacter playerCharacter;

    public StoreInitialized(PlayerCharacter _playerCharacter)
    {
        playerCharacter = _playerCharacter;
    }

    public override void ParseChange(StoreView view, StoreController controller)
    {
        view.UpdateStats(playerCharacter);
        Debug.Log("Store Initialized.");
    }
}
