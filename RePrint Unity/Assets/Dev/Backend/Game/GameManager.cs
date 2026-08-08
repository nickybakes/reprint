using UnityEngine;

public class GameManager : MonoBehaviour
{

    /// <summary>
    /// The current Game Manager
    /// </summary>
    public static GameManager game;

    private BattleManager battleManager;
    private StoreManager storeManager;

    [SerializeField] private CharacterData playerData;

    [SerializeField] private ModData[] playerMods;

    public PlayerCharacter Player { get; private set; }

    /// <summary>
    /// Input whatever battle data you want and that will be the battle you play.
    /// </summary>
    [SerializeField] private BattleData battleData;

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
            SetupGameSession();
        }
    }

    public void SetupGameSession()
    {
        Player = new PlayerCharacter(playerData, playerMods);
    }

    public void GoToBattleScene()
    {
        AppManager.app.SwitchToScene(SceneIndex.BattleTest, BattleSceneLoaded);
    }

    public void GoToStoreScene()
    {
        AppManager.app.SwitchToScene(SceneIndex.StoreTest, StoreSceneLoaded);
    }

    public void BattleSceneLoaded()
    {
        battleManager = FindAnyObjectByType<BattleManager>();
        battleManager.SetupBattle(Player, battleData);
    }

    public void StoreSceneLoaded()
    {
        storeManager = FindAnyObjectByType<StoreManager>();
        storeManager.SetupStore(Player);
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
