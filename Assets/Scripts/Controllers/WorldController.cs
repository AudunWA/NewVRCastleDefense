using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class WorldController : MonoBehaviour
{
    // Players
    public Player evilPlayer = new Player("PlayerEvil", PlayerType.Evil, new List<Minion>(),
    new Castle(5000, new Vector3(0.0f, 15.0f, -200.0f)), 1, new Vector3(0.0f, 0.0f, -150.0f));
    public Player goodPlayer = new Player("PlayerGood", PlayerType.Good, new List<Minion>(),
    new Castle(5000, new Vector3(0.0f, 15.0f, 200.0f)), 1, new Vector3(0.0f, 0.0f, 150.0f));

    private List<MinionAttribute> MinionAttributes = new List<MinionAttribute>
    {
        MinionAttribute.Armor,MinionAttribute.Health,
        MinionAttribute.AttackCooldownTime, MinionAttribute.Damage,
        MinionAttribute.Movementspeed, MinionAttribute.Range
    };
    public string[] FighterLevels = new string[6];
    public string[] TankLevels = new string[6];
    public string[] MageLevels = new string[6];
    public string[] ArcherLevels = new string[6];
    public string[] EvilFighterLevels = new string[6];
    public string[] EvilTankLevels = new string[6];
    public string[] EvilMageLevels = new string[6];
    public string[] EvilArcherLevels = new string[6];

    private int gameAILevel;
    private float aiTimer = 0.0f;
    public int GameAILevel
    {
        get { return gameAILevel; }
        set { gameAILevel = value; }
    }
    private bool aiActive = true;
    public bool AiActive
    {
        get { return aiActive; }
        set { aiActive = value; }
    }
    private bool friendlyAi = false;

    public bool FriendlyAi
    {
        get { return friendlyAi; }
        set { friendlyAi = value; }
    }
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

    private void DisplayMinionLevelsInspector()
    {
        int i = 0;
        foreach (KeyValuePair<MinionAttribute, int> ma in GoodPlayer.MinionStatistics[SpawnType.Fighter].Levels)
        {
            if (i >= FighterLevels.Length) break;
            FighterLevels[i] = ma.Key + ": " + ma.Value;
            i++;
        }
        i = 0;
        foreach (KeyValuePair<MinionAttribute, int> ma in EvilPlayer.MinionStatistics[SpawnType.Fighter].Levels)
        {
            if (i >= FighterLevels.Length) break;
            EvilFighterLevels[i] = ma.Key + ": " + ma.Value;
            i++;
        }
        i = 0;
        foreach (KeyValuePair<MinionAttribute, int> ma in GoodPlayer.MinionStatistics[SpawnType.Tank].Levels)
        {
            if (i >= TankLevels.Length) break;
            TankLevels[i] = ma.Key + ": " + ma.Value;
            i++;
        }
        i = 0;
        foreach (KeyValuePair<MinionAttribute, int> ma in EvilPlayer.MinionStatistics[SpawnType.Tank].Levels)
        {
            if (i >= TankLevels.Length) break;
            EvilTankLevels[i] = ma.Key + ": " + ma.Value;
            i++;
        }
        i = 0;
        foreach (KeyValuePair<MinionAttribute, int> ma in GoodPlayer.MinionStatistics[SpawnType.Archer].Levels)
        {
            if (i >= FighterLevels.Length) break;
            ArcherLevels[i] = ma.Key + ": " + ma.Value;
            i++;
        }
        i = 0;
        foreach (KeyValuePair<MinionAttribute, int> ma in EvilPlayer.MinionStatistics[SpawnType.Archer].Levels)
        {
            if (i >= FighterLevels.Length) break;
            EvilArcherLevels[i] = ma.Key + ": " + ma.Value;
            i++;
        }
        i = 0;
        foreach (KeyValuePair<MinionAttribute, int> ma in GoodPlayer.MinionStatistics[SpawnType.Mage].Levels)
        {
            if (i >= FighterLevels.Length) break;
            MageLevels[i] = ma.Key + ": " + ma.Value;
            i++;
        }
        i = 0;
        foreach (KeyValuePair<MinionAttribute, int> ma in EvilPlayer.MinionStatistics[SpawnType.Mage].Levels)
        {
            if (i >= FighterLevels.Length) break;
            EvilMageLevels[i] = ma.Key + ": " + ma.Value;
            i++;
        }
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
                    armor: 3,
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
                    damage: 6f,
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
                    damage: 8f,
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
                    damage: 8f,
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
                    bounty: 4,
                    damage: 2.2f,
                    movementspeed: 0.28f,
                    attackCooldownTime: -0.01f,
                    cost: 8,
                    health: 15,
                    levelUpgradeCost: new Dictionary<MinionAttribute, int>
                    {
                        {MinionAttribute.Armor, 8},
                        {MinionAttribute.Range, -1},
                        {MinionAttribute.Damage, 8},
                        {MinionAttribute.Movementspeed, 8},
                        {MinionAttribute.AttackCooldownTime, 8},
                        {MinionAttribute.Health, 8}
                    }
                )
            },
            {
                SpawnType.Tank,
                new MinionStat(
                    spawnType: SpawnType.Tank,
                    armor: 4,
                    range: 0,
                    bounty: 6,
                    damage: 1f,
                    movementspeed: 0.28f,
                    attackCooldownTime: -0.01f,
                    cost: 12,
                    health: 30,
                    levelUpgradeCost: new Dictionary<MinionAttribute, int>
                    {
                        {MinionAttribute.Armor, 12},
                        {MinionAttribute.Range, -1},
                        {MinionAttribute.Damage, 12},
                        {MinionAttribute.Movementspeed, 12},
                        {MinionAttribute.AttackCooldownTime, 12},
                        {MinionAttribute.Health, 12}
                    }
                )
            },
            {
                SpawnType.Mage,
                new MinionStat(
                    spawnType: SpawnType.Mage,
                    armor: 1,
                    range: 1f,
                    bounty: 5,
                    damage: 2f,
                    movementspeed: 0.28f,
                    attackCooldownTime: -0.01f,
                    cost: 15,
                    health: 5,
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
                SpawnType.Archer,
                new MinionStat(
                    spawnType: SpawnType.Archer,
                    armor: 1,
                    range: 1f,
                    bounty: 5,
                    damage: 2f,
                    movementspeed: 0.28f,
                    attackCooldownTime: -0.01f,
                    cost: 15,
                    health: 5,
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
            { SpawnType.Fighter, 15 },
            { SpawnType.Tank, 25 },
            { SpawnType.Mage, 20 },
            { SpawnType.Archer, 20 }
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
                {MinionAttribute.Armor, 25},
                {MinionAttribute.Range, 0},
                {MinionAttribute.Damage, 20},
                {MinionAttribute.Movementspeed, 25},
                {MinionAttribute.AttackCooldownTime, 25},
                {MinionAttribute.Health, 25}
            }},
            { SpawnType.Tank,  new Dictionary<MinionAttribute, int>
            {
                {MinionAttribute.Armor, 30},
                {MinionAttribute.Range, 0},
                {MinionAttribute.Damage, 35},
                {MinionAttribute.Movementspeed, 35},
                {MinionAttribute.AttackCooldownTime, 35},
                {MinionAttribute.Health, 30}
            }},
            { SpawnType.Mage,  new Dictionary<MinionAttribute, int>
            {
                {MinionAttribute.Armor, 40},
                {MinionAttribute.Range, 40},
                {MinionAttribute.Damage, 30},
                {MinionAttribute.Movementspeed, 40},
                {MinionAttribute.AttackCooldownTime, 40},
                {MinionAttribute.Health, 40}
            }},
            { SpawnType.Archer,  new Dictionary<MinionAttribute, int>
            {
                {MinionAttribute.Armor, 40},
                {MinionAttribute.Range, 40},
                {MinionAttribute.Damage, 30},
                {MinionAttribute.Movementspeed, 40},
                {MinionAttribute.AttackCooldownTime, 40},
                {MinionAttribute.Health, 40}
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
        aiController = new AIController(EvilPlayer, GoodPlayer, gameAILevel, friendlyAi);
    }

    private void InitPlayers()
    {
        goodPlayer.SpawnLocation = new Vector3(goodPlayer.Castle.Position.x, 0, goodPlayer.Castle.Position.z - castleSize.z);
        evilPlayer.SpawnLocation = new Vector3(goodPlayer.Castle.Position.x, 0, evilPlayer.Castle.Position.z + castleSize.z);
        goodPlayer.Money = 500;
        goodPlayer.MoneyIncrementFactor = 15;
        if (gameAILevel > 2)
        {
            evilPlayer.Money = gameAILevel > 3 ? 4*goodPlayer.Money : 2*goodPlayer.Money;
            evilPlayer.MoneyIncrementFactor = gameAILevel > 3 ? (int)2.7*goodPlayer.MoneyIncrementFactor : (int)1.8*goodPlayer.MoneyIncrementFactor;
        }
        else
        {
            evilPlayer.Money = goodPlayer.Money;
            evilPlayer.MoneyIncrementFactor = goodPlayer.MoneyIncrementFactor;
        }
        goodPlayer.MinionStatistics = new Dictionary<SpawnType, MinionStat>(DeepCopyMinionStats(minionStats));
        evilPlayer.MinionStatistics = new Dictionary<SpawnType, MinionStat>(DeepCopyMinionStats(minionStats));
        GoodPlayer.SpawnController = new SpawnController(gameflowController, CooldownLimits);
        EvilPlayer.SpawnController = new SpawnController(gameflowController, CooldownLimits);
        GoodPlayer.SpawnController.Player = GoodPlayer;
        EvilPlayer.SpawnController.Player = EvilPlayer;
    }

    private void GetSettings()
    {
        Settings Settings = GameObject.FindGameObjectWithTag("SETTINGS").GetComponent<Settings>();
        GameAILevel = Settings.GetLevel();
        FriendlyAi = Settings.IsFreeplay();
    }

    //Use this for initialization
    private void Awake()
    {
        GetSettings();
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
        DisplayMinionLevelsInspector();
        aiController.FriendlyAi = friendlyAi;
        GoodPlayer.SpawnController.GetTimer.UpdateTimers();
        EvilPlayer.SpawnController.GetTimer.UpdateTimers();
        gameflowController.UpdatePlayerMoney(GoodPlayer);
        gameflowController.UpdatePlayerMoney(EvilPlayer);
        if (aiActive)
        {
            aiTimer += Time.deltaTime;
            if (aiTimer > 0.33f) // Ai only needs to evaluate what's happening 3 times per sec
            {
                aiController.PlayAI();
                aiTimer = 0.0f;
            }
           
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
