using UnityEngine;

public class MenuManager : MonoBehaviour
{

    public void StartGameButton()
    {
        GameManager.game.GoToBattleScene();
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
