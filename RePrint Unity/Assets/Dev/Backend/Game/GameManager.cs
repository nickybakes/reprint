using UnityEngine;

public class GameManager : MonoBehaviour
{

    /// <summary>
    /// The current Game Manager
    /// </summary>
    public static GameManager game;

    private BattleManager battleManager;

    [SerializeField] private CharacterData playerData;

    [SerializeField] private ModData[] playerMods;

    private PlayerCharacter player;

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
        player = new PlayerCharacter(playerData, playerMods);
    }

    public void GoToBattleScene()
    {
        AppManager.app.SwitchToScene(SceneIndex.BattleTest, BattleSceneLoaded);
    }

    public void BattleSceneLoaded()
    {
        battleManager = FindAnyObjectByType<BattleManager>();
        battleManager.SetupBattle(player, battleData);
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
