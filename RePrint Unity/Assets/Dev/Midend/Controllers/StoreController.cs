using UnityEngine;

public class StoreController : MonoBehaviour
{
    /// <summary>
    /// Reference to the Store Manager in the scene.
    /// </summary>
    [SerializeField] private StoreManager storeManager;

    /// <summary>
    /// Reference to the Store View in the scene.
    /// </summary>
    [SerializeField] private StoreView storeView;


    public void StartGameButton()
    {
        storeManager.PlayerSubmitGoToBattle();
    }
}