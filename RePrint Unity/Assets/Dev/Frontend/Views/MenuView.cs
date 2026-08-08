using UnityEngine;

public class MenuView : MonoBehaviour
{

    public void StartGameButton()
    {
        GameManager.game.GoToBattleScene();
    }

    public void OpenStoreButton()
    {
        GameManager.game.GoToStoreScene();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // if (AppManager.app)
        //     StartGameButton();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
