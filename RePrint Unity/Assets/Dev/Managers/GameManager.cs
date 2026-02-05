using UnityEngine;

public class GameManager : MonoBehaviour
{

    /// <summary>
    /// The current Game Manager
    /// </summary>
    public static GameManager game;

    /// <summary>
    /// A reference to the Battle Manager prefab. the Game Manager will spawn a new Battle Manager when starting a battle.
    /// </summary>
    [SerializeField]
    private BattleManager battleManagerPrefab;

    /// <summary>
    /// Input whatever battle data you want and that will be the battle you play.
    /// </summary>
    [SerializeField]
    private BattleData battleData;

    private void Awake()
    {
        if (game != null && game != this)
        {
            Destroy(this);
        }
        else
        {
            game = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void GoToBattleScene()
    {
        AppManager.app.SwitchToScene(SceneIndex.BattleTest, BattleSceneLoaded);
    }

    public void BattleSceneLoaded()
    {
        Instantiate(battleManagerPrefab);
    }

    public BattleData GetBattleData()
    {
        return battleData;
    }

    // Update is called once per frame
    void Update()
    {

    }

}
