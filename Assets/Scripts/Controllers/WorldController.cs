using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[Serializable]
public class WorldController : MonoBehaviour
{
    // Players
    public Player evilPlayer = new Player("PlayerEvil", PlayerType.Evil, new List<Minion>(),
    new Castle(5000, new Vector3(0.0f, 15.0f, -200.0f)), 1, new Vector3(0.0f, 0.0f, -150.0f));
    public Player goodPlayer = new Player("PlayerGood", PlayerType.Good, new List<Minion>(),
    new Castle(5000, new Vector3(0.0f, 15.0f, 200.0f)), 1, new Vector3(0.0f, 0.0f, 150.0f));

    private bool aiActive = false;
    private bool gameFinished = false;
    public bool SoundEffectsActive = false;
    // Controllers
    public CastleController GoodCastleController { get; set; }
    public CastleController EvilCastleController { get; set; }
    public GuiController GuiController { get; set; }

    // Castles
    public GameObject GoGoodCastle { get; set; }
    public GameObject GoEvilCastle { get; set; }

    // Handlers
    public SpawnController spawnController;
    private AIController aiController;
    private GameflowController gameflowController;
    private Vector3 castleSize = new Vector3(50, 50, 50); // FIXME: Change to actually getting the GO and use its size

    //Dictionaries
    private Dictionary<SpawnType, float> cooldownLimits;
    private Dictionary<SpawnType, int> bounties;
    private Dictionary<SpawnType, int> costs;
    private Dictionary<SpawnType, MinionStat> minionStats;
    private Dictionary<SpawnType, MinionStat> minionStatAdditions;
    private Dictionary<SpawnType, Dictionary<MinionAttribute, int>> upgradeCosts;
    // Getters and setters
    public Dictionary<SpawnType, float> CooldownLimits
    {
        get { return cooldownLimits; }
        set { cooldownLimits = value; }
    }

    [SerializeField]public Player EvilPlayer
    {
        get { return evilPlayer; }

        set { evilPlayer = value; }
    }

   [SerializeField] public Player GoodPlayer
    {
        get { return goodPlayer; }

        set { goodPlayer = value; }
    }

    public bool AiActive
    {
        get { return aiActive; }
        set { aiActive = value; }
    }
    public Player GetOtherPlayer(Player player)
    {
        if (player.PlayerType == PlayerType.Evil) return GoodPlayer;
        return EvilPlayer;
    }

    private Dictionary<SpawnType, MinionStat> DeepCopyMinionStats(Dictionary<SpawnType, MinionStat> stats)
    {
        Dictionary<SpawnType, MinionStat> copy = new Dictionary<SpawnType, MinionStat>();
        foreach (KeyValuePair<SpawnType, MinionStat> s in stats)
        {
            MinionStat m = s.Value;
            MinionStat newMinionstat = new MinionStat(
                m.SpawnType, 
                m.Levels, 
                m.Bounty, 
                m.Cost, 
                m.LevelUpgradeCost, 
                m.Abilities);
            copy.Add(s.Key, newMinionstat);
        }
        return copy;
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
                    levels: new Dictionary<MinionAttribute, int>
                    {
                        {MinionAttribute.Armor, 1},
                        {MinionAttribute.Range, 1},
                        {MinionAttribute.Damage, 1},
                        {MinionAttribute.Movementspeed, 1},
                        {MinionAttribute.AttackCooldownTime, 1},
                        {MinionAttribute.Health, 1}
                    },
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
                    levels: new Dictionary<MinionAttribute, int>
                    {
                        {MinionAttribute.Armor, 1},
                        {MinionAttribute.Range,1},
                        {MinionAttribute.Damage,1},
                        {MinionAttribute.Movementspeed,1},
                        {MinionAttribute.AttackCooldownTime,1},
                        {MinionAttribute.Health,1}
                    },
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
                    levels: new Dictionary<MinionAttribute, int>
                    {
                        {MinionAttribute.Armor, 1},
                        {MinionAttribute.Range,1},
                        {MinionAttribute.Damage,1},
                        {MinionAttribute.Movementspeed,1},
                        {MinionAttribute.AttackCooldownTime,1},
                        {MinionAttribute.Health,1}
                    },
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
                    levels: new Dictionary<MinionAttribute, int>
                    {
                        {MinionAttribute.Armor, 1},
                        {MinionAttribute.Range,1},
                        {MinionAttribute.Damage,1},
                        {MinionAttribute.Movementspeed,1},
                        {MinionAttribute.AttackCooldownTime,1},
                        {MinionAttribute.Health,1}
                    },
                    range: 80f,
                    bounty: bounties[SpawnType.Archer],
                    damage: 6f,
                    movementspeed: 2.8f,
                    attackCooldownTime: 2f,
                    cost: costs[SpawnType.Archer],
                    health: 50,
                    levelUpgradeCost: upgradeCosts[SpawnType.Archer]
                )
            }
        };
        // Stats to add for each added levels
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
                    levelUpgradeCost: new Dictionary<MinionAttribute, int>
                    {
                        {MinionAttribute.Armor, 15},
                        {MinionAttribute.Range, 15},
                        {MinionAttribute.Damage, 15},
                        {MinionAttribute.Movementspeed, 15},
                        {MinionAttribute.AttackCooldownTime, 15},
                        {MinionAttribute.Health, 15}
                    }
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
                    levelUpgradeCost: new Dictionary<MinionAttribute, int>
                    {
                        {MinionAttribute.Armor, 25},
                        {MinionAttribute.Range, 25},
                        {MinionAttribute.Damage, 25},
                        {MinionAttribute.Movementspeed, 25},
                        {MinionAttribute.AttackCooldownTime, 25},
                        {MinionAttribute.Health, 25}
                    }
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
                    cost: 30,
                    health: 10,
                    levelUpgradeCost: new Dictionary<MinionAttribute, int>
                    {
                        {MinionAttribute.Armor, 30},
                        {MinionAttribute.Range, 30},
                        {MinionAttribute.Damage, 30},
                        {MinionAttribute.Movementspeed, 30},
                        {MinionAttribute.AttackCooldownTime, 30},
                        {MinionAttribute.Health, 30}
                    }
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
                    cost: 30,
                    health: 10,
                    levelUpgradeCost: new Dictionary<MinionAttribute, int>
                    {
                        {MinionAttribute.Armor, 30},
                        {MinionAttribute.Range, 30},
                        {MinionAttribute.Damage, 30},
                        {MinionAttribute.Movementspeed, 30},
                        {MinionAttribute.AttackCooldownTime, 30},
                        {MinionAttribute.Health, 30}
                    }
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
        upgradeCosts = new Dictionary<SpawnType, Dictionary<MinionAttribute,int>>
        {
            { SpawnType.Fighter,  new Dictionary<MinionAttribute, int>
            {
                {MinionAttribute.Armor, 50},
                {MinionAttribute.Range, 50},
                {MinionAttribute.Damage, 50},
                {MinionAttribute.Movementspeed, 50},
                {MinionAttribute.AttackCooldownTime, 50},
                {MinionAttribute.Health, 50}
            }},
            { SpawnType.Tank,  new Dictionary<MinionAttribute, int>
            {
                {MinionAttribute.Armor, 70},
                {MinionAttribute.Range, 70},
                {MinionAttribute.Damage, 70},
                {MinionAttribute.Movementspeed, 70},
                {MinionAttribute.AttackCooldownTime, 70},
                {MinionAttribute.Health, 70}
            }},
            { SpawnType.Mage,  new Dictionary<MinionAttribute, int>
            {
                {MinionAttribute.Armor, 80},
                {MinionAttribute.Range,80},
                {MinionAttribute.Damage, 80},
                {MinionAttribute.Movementspeed, 80},
                {MinionAttribute.AttackCooldownTime, 80},
                {MinionAttribute.Health, 80}
            }},
            { SpawnType.Archer,  new Dictionary<MinionAttribute, int>
            {
                {MinionAttribute.Armor, 80},
                {MinionAttribute.Range,80},
                {MinionAttribute.Damage, 80},
                {MinionAttribute.Movementspeed, 80},
                {MinionAttribute.AttackCooldownTime, 80},
                {MinionAttribute.Health, 80}
            }}
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
        gameflowController = new GameflowController(EvilPlayer, GoodPlayer);
        gameflowController.MinionStatAdditions = minionStatAdditions;
        gameflowController.WorldController = this;
        aiController = new AIController(EvilPlayer, GoodPlayer);
    }

    private void InitPlayers()
    {
        goodPlayer.SpawnLocation = new Vector3(goodPlayer.Castle.Position.x, 0, goodPlayer.Castle.Position.z - castleSize.z);
        evilPlayer.SpawnLocation = new Vector3(goodPlayer.Castle.Position.x, 0, evilPlayer.Castle.Position.z + castleSize.z);
        goodPlayer.Money = 1000;
        evilPlayer.Money = 1000;
        goodPlayer.MinionStatistics = new Dictionary<SpawnType, MinionStat>(DeepCopyMinionStats(minionStats));
        evilPlayer.MinionStatistics = new Dictionary<SpawnType, MinionStat>(DeepCopyMinionStats(minionStats));
        GoodPlayer.SpawnController = new SpawnController(gameflowController, CooldownLimits);
        EvilPlayer.SpawnController = new SpawnController(gameflowController, CooldownLimits);
        GoodPlayer.SpawnController.Player = GoodPlayer;
        EvilPlayer.SpawnController.Player = EvilPlayer;
    }
    //Use this for initialization
    private void Awake()
    {
        
        SetBounties();
        SetCosts();
        InitMinionStats();
        InitTimers();
        InitControllers();
        InitPlayers();
    }

    // Update is called once per frame
    private void Update()
    {

        if (gameFinished) return;

        if (GoEvilCastle != null && GoGoodCastle != null && GoodCastleController == null)
        {
            InitCastles();
        }

        GoodPlayer.SpawnController.GetTimer.UpdateTimers();
        EvilPlayer.SpawnController.GetTimer.UpdateTimers();
        gameflowController.UpdatePlayerMoney(GoodPlayer);
        gameflowController.UpdatePlayerMoney(EvilPlayer);
        if (aiActive)
        {
            aiController.PlayAI();
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
