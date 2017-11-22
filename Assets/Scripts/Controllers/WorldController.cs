using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    // Players
    private Player evilPlayer = new Player("PlayerEvil", PlayerType.Evil, new List<Minion>(),
        new Castle(5000, new Vector3(0.0f, 15.0f, -200.0f)), 1, 100, new Vector3(0.0f, 0.0f, -150.0f));
    private Player goodPlayer = new Player("PlayerGood", PlayerType.Good, new List<Minion>(),
        new Castle(5000, new Vector3(0.0f, 15.0f, 200.0f)), 1, 100, new Vector3(0.0f, 0.0f, 150.0f));
    private bool singlePlayer = false;
    private bool gameFinished = false;

    // Controllers
    public CastleController GoodCastleController { get; set; }
    public CastleController EvilCastleController { get; set; }
    public GuiController GuiController { get; set; }

    // Castles
    public GameObject GoGoodCastle { get; set; }
    public GameObject GoEvilCastle { get; set; }

    // Handlers
    public SpawnController spawnController;
    private AIController _aiController;
    private GameflowController gameflowController;
    private Vector3 castleSize = new Vector3(50, 50, 50); // FIXME: Change to actually getting the GO and use its size

    //Dictionaries
    private Dictionary<SpawnType, float> cooldownLimits;
    private Dictionary<SpawnType, int> bounties;
    private Dictionary<SpawnType, int> costs;
    private Dictionary<SpawnType, MinionStat> minionStats;
    private Dictionary<SpawnType, MinionStat> minionStatAdditions;
    private Dictionary<SpawnType, int> upgradeCosts;
    // Getters and setters
    public Dictionary<SpawnType, float> CooldownLimits
    {
        get { return cooldownLimits; }
        set { cooldownLimits = value; }
    }

    public Player EvilPlayer
    {
        get { return evilPlayer; }

        set { evilPlayer = value; }
    }

    public Player GoodPlayer
    {
        get { return goodPlayer; }

        set { goodPlayer = value; }
    }

    public bool SinglePlayer
    {
        get { return singlePlayer; }
        set { singlePlayer = value; }
    }
    public Player GetOtherPlayer(Player player)
    {
        if (player.PlayerType == PlayerType.Evil) return GoodPlayer;
        return EvilPlayer;
    }
    // Initializations
    private void InitMinionStats()
    {
        minionStats = new Dictionary<SpawnType, MinionStat>
        {
            {
                SpawnType.Fighter,
                new MinionStat(
                    spawnType: SpawnType.Fighter,
                    armor: 2,
                    level: 1,
                    range: 5f,
                    bounty: bounties[SpawnType.Fighter],
                    damage: 4f,
                    movementspeed: 3.0f,
                    attackCooldownTime: 1f,
                    cost: costs[SpawnType.Fighter],
                    health: 100,
                    levelUpgradeCost: upgradeCosts[SpawnType.Fighter]
                )    
            },
            {
                SpawnType.Tank,
                new MinionStat(
                    spawnType: SpawnType.Tank,
                    armor: 8,
                    level: 1,
                    range: 6f,
                    bounty: bounties[SpawnType.Tank],
                    damage: 12f,
                    movementspeed: 1.5f,
                    attackCooldownTime: 3f,
                    cost: costs[SpawnType.Tank],
                    health: 300,
                    levelUpgradeCost: upgradeCosts[SpawnType.Tank]
                )
            },
            {
                SpawnType.Mage,
                new MinionStat(
                    spawnType: SpawnType.Mage,
                    armor: 0,
                    level: 1,
                    range: 60f,
                    bounty: bounties[SpawnType.Mage],
                    damage: 10f,
                    movementspeed: 2.5f,
                    attackCooldownTime: 2.9f,
                    cost: costs[SpawnType.Mage],
                    health: 60,
                    levelUpgradeCost: upgradeCosts[SpawnType.Mage]
                )
            },
            {
                SpawnType.Archer,
                new MinionStat(
                    spawnType: SpawnType.Archer,
                    armor: 0,
                    level: 1,
                    range: 80f,
                    bounty: bounties[SpawnType.Archer],
                    damage: 4f,
                    movementspeed: 2.8f,
                    attackCooldownTime: 2f,
                    cost: costs[SpawnType.Archer],
                    health: 50,
                    levelUpgradeCost: upgradeCosts[SpawnType.Archer]
                )
            }
        };
        // Stats to add for each added level
        minionStatAdditions = new Dictionary<SpawnType, MinionStat>
        {
            {
                SpawnType.Fighter,
                new MinionStat(
                    spawnType: SpawnType.Fighter,
                    armor: 2,
                    range: 0,
                    bounty: 20,
                    damage: 2f,
                    movementspeed: 0.05f,
                    attackCooldownTime: -0.02f,
                    cost: 15,
                    health: 20,
                    levelUpgradeCost: 65
                )
            },
            {
                SpawnType.Tank,
                new MinionStat(
                    spawnType: SpawnType.Tank,
                    armor: 4,
                    range: 0,
                    bounty: 30,
                    damage: 2f,
                    movementspeed: 0.05f,
                    attackCooldownTime: -0.02f,
                    cost: 25,
                    health: 60,
                    levelUpgradeCost: 85
                )
            },
            {
                SpawnType.Mage,
                new MinionStat(
                    spawnType: SpawnType.Mage,
                    armor: 1,
                    range: 1f,
                    bounty: 40,
                    damage: 2f,
                    movementspeed: 0.05f,
                    attackCooldownTime: -0.02f,
                    cost: 35,
                    health: 10,
                    levelUpgradeCost: 110
                )
            },
            {
                SpawnType.Archer,
                new MinionStat(
                    spawnType: SpawnType.Archer,
                    armor: 1,
                    range: 2f,
                    bounty: 40,
                    damage: 2f,
                    movementspeed: 0.05f,
                    attackCooldownTime: -0.01f,
                    cost: 35,
                    health: 10,
                    levelUpgradeCost: 110
                )
            }
        };
    }
    private void InitTimers()
    {
        CooldownLimits = new Dictionary<SpawnType, float>
        {
            { SpawnType.Fighter, 0.8f },
            { SpawnType.Tank, 0.8f },
            { SpawnType.Mage, 0.8f },
            { SpawnType.Archer, 0.8f },
        };
        gameflowController = new GameflowController(EvilPlayer, GoodPlayer);
        gameflowController.MinionStatAdditions = minionStatAdditions;
        gameflowController.WorldController = this;
        _aiController = new AIController(EvilPlayer, GoodPlayer);
    }

    private void SetBounties()
    {
        bounties = new Dictionary<SpawnType, int>
        {
            { SpawnType.Fighter, 60 },
            { SpawnType.Tank, 90 },
            { SpawnType.Mage, 100 },
            { SpawnType.Archer, 100 }
        };
    }
    private void SetCosts()
    {
        costs = new Dictionary<SpawnType, int>
        {
            { SpawnType.Fighter, 50 },
            { SpawnType.Tank, 70 },
            { SpawnType.Mage, 80},
            { SpawnType.Archer, 80 },
        };
        upgradeCosts = new Dictionary<SpawnType, int>
        {
            { SpawnType.Fighter, 300},
            { SpawnType.Tank, 400},
            { SpawnType.Mage, 500},
            { SpawnType.Archer, 500}
        };
    }

    private void InitCastles()
    {
        evilPlayer.Castle.Player = evilPlayer;
        goodPlayer.Castle.Player = goodPlayer;
        GoodCastleController = GoGoodCastle.GetComponent<CastleController>();
        EvilCastleController = GoEvilCastle.GetComponent<CastleController>();
        if (GoodCastleController.Castle == null)
        {
            GoodCastleController.Castle = goodPlayer.Castle;
        }
        if (EvilCastleController.Castle == null)
        {
            EvilCastleController.Castle = evilPlayer.Castle;
        }
        evilPlayer.Castle.gameObject = GoEvilCastle;
        goodPlayer.Castle.gameObject = GoGoodCastle;
    }

    private void InitControllers()
    {
        GuiController = GetComponent<GuiController>();
        GuiController.EvilPlayer = EvilPlayer;
        GuiController.GoodPlayer = GoodPlayer;
    }

    private void InitPlayers()
    {
		    goodPlayer.SpawnLocation = GameObject.Find("LeftCastle/Spawnspot").transform.position;
        evilPlayer.SpawnLocation = GameObject.Find("RightCastle/Spawnspot").transform.position;
        goodPlayer.Money = 5000;
        evilPlayer.Money = 5000;
        goodPlayer.MinionStatistics = new Dictionary<SpawnType, MinionStat>(minionStats);
        evilPlayer.MinionStatistics = new Dictionary<SpawnType, MinionStat>(minionStats);
        GoodPlayer.SpawnController = new SpawnController(gameflowController, CooldownLimits);
        EvilPlayer.SpawnController = new SpawnController(gameflowController, CooldownLimits);
        GoodPlayer.SpawnController.Player = GoodPlayer;
        EvilPlayer.SpawnController.Player = EvilPlayer;
    }
    //Use this for initialization
    private void Start()
    {
        SetBounties();
        SetCosts();
        InitMinionStats();
        InitCastles();
        InitTimers();
        InitControllers();
        InitPlayers();
    }

    // Update is called once per frame
    private void Update()
    {
        if (gameFinished) return;

        GoodPlayer.SpawnController.GetTimer.UpdateTimers();
        EvilPlayer.SpawnController.GetTimer.UpdateTimers();
        gameflowController.UpdatePlayerMoney(GoodPlayer);
        gameflowController.UpdatePlayerMoney(EvilPlayer);
        if (singlePlayer)
        {
            _aiController.PlayAI();
        }
        if (goodPlayer.Castle.Health <= 0)
        {
            //GAME OVER
            GameObject instance = Instantiate(Resources.Load("LoseCanvas", typeof(GameObject))) as GameObject;
            Time.timeScale = 0.1f;
            gameFinished = true;
        }
        else if (evilPlayer.Castle.Health <= 0)
        {
            // GAME OVER
            GameObject instance = Instantiate(Resources.Load("WinCanvas", typeof(GameObject))) as GameObject;
            Time.timeScale = 0.1f;
            gameFinished = true;
        }
    }
}

public enum SpawnType
{
    Fighter,
    Tank,
    Mage,
    Archer
}
