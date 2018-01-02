using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;

public class SpawnController
{
    private SpawnController.Timer timer;
    private GameflowController gameflowController;
    private Player player;
    public SpawnController(GameflowController gameflowController, Dictionary<SpawnType, float> cooldownLimits)
    {
        this.gameflowController = gameflowController;
        timer = new Timer(cooldownLimits);
    }

    public void UpgradeMinionType(SpawnType spawnType, MinionAttribute attr)
    {
        gameflowController.UpgradeMinionStat(spawnType, player, attr);
    }

    public Player Player
    {
        get { return player; }
        set { player = value; }
    }

    public Timer GetTimer
    {
        get { return timer; }
    }
    public Dictionary<SpawnType, bool> GetAvailableSpawnTypeTimers()
    {
        return timer.AvailableSpawnTypeTimers;
    }

    private void GenerateSingleMinion(Minion minion)
    {
        string prefab = minion.SpawnType.ToString();
        GameObject goMinion = (GameObject)Object.Instantiate(Resources.Load(prefab));
        MinionController controller = goMinion.GetComponent<MinionController>();
        controller.Minion = minion;
        controller.Owner = player;
        controller.StateChanged += gameflowController.OnMinionKilled; // Gameflowhandler subscribes to state changed event

        // Make the minion a archery target (for player's longbow)
        ArcheryTarget target = goMinion.AddComponent<ArcheryTarget>();
        target.onTakeDamage = new UnityEvent();
        target.onTakeDamage.AddListener(controller.OnHitByPlayerArrow);

        int minionNumber = player.Minions.Count + 1;

        goMinion.name = player.PlayerType + "_"+minion.SpawnType+"_" + minionNumber+"_Level_"+minion.Level;
        goMinion.transform.Translate(minion.Position);
        player.AddMinion(minion);
        minion.gameObject = goMinion;
    }


    public void Spawn(SpawnType spawnType, Vector3? position = null)
    {
        bool isAvailableSpawnType = timer.IsAvailableSpawnType(spawnType);
        if (!isAvailableSpawnType && position == null) return;
        MinionStat stat = player.MinionStatistics[spawnType];
        int cost = stat.Cost;
        if (!player.WithdrawMoney(cost))
        {
            return;
        }

        Minion minion;
        Vector3 spawnPosition = position ?? player.SpawnLocation;
        switch (spawnType)
        {
            case SpawnType.Tank:
                minion = new Tank(player, stat, spawnPosition);
                break;
            case SpawnType.Mage:
                minion = new Mage(player, stat, spawnPosition);
                break;
            case SpawnType.Archer:
                minion = new Archer(player, stat, spawnPosition);
                break;
            default:
                minion = new Fighter(player, stat, spawnPosition);
                break;
        }
        timer.StartTimer(spawnType);
        GenerateSingleMinion(minion);
    }


    public class Timer
    {
        private Dictionary<SpawnType, float> timers;
        private Dictionary<SpawnType, float> cooldownLimits;
        private Dictionary<SpawnType, bool> availableSpawnTypeTimers;
        public float moneyTimer { get; set; }
        public float moneyTimerLim { get; set; }
        private List<SpawnType> spawnTypes;
        public Timer(Dictionary<SpawnType, float> cooldownLimits)
        {
            this.cooldownLimits = cooldownLimits;
            timers = new Dictionary<SpawnType, float>(cooldownLimits); // Copy of cooldownLimits
            availableSpawnTypeTimers = new Dictionary<SpawnType, bool>();
            spawnTypes = cooldownLimits.Keys.ToList();
            moneyTimer = 1.0f;
            moneyTimerLim = 1.0f;
        }

        public Dictionary<SpawnType, bool> AvailableSpawnTypeTimers
        {
            get { return availableSpawnTypeTimers; }
            set { availableSpawnTypeTimers = value; }
        }

        public Dictionary<SpawnType, float> Timers
        {
            get { return timers; }
            set { timers = value; }
        }

        public bool IsIncrementMoneyAvailable()
        {
            return moneyTimer >= moneyTimerLim;
        }

        private void UpdateMoneyTimer()
        {
            moneyTimer += Time.deltaTime;
        }

        public void StartMoneyTimer()
        {
            moneyTimer = 0;
        }

        public Dictionary<SpawnType, float> CooldownLimits
        {
            get { return cooldownLimits; }
        }

        public List<SpawnType> SpawnTypes
        {
            get { return spawnTypes; }
            set { spawnTypes = value; }
        }

        private void UpdateAvailableSpawnTypes()
        {
            foreach (SpawnType s in spawnTypes)
            {
                availableSpawnTypeTimers[s] = IsAvailableSpawnType(s);
            }
        }

        public void UpdateTimers()
        {
            foreach (SpawnType s in spawnTypes)
            {
                if (timers[s] < cooldownLimits[s])
                {
                    timers[s] += Time.deltaTime;
                }
            }
            UpdateMoneyTimer();
            UpdateAvailableSpawnTypes();
        }

        public void ResetAllTimers()
        {
            foreach (SpawnType s in spawnTypes)
            {
                timers[s] = cooldownLimits[s];
            }

        }

        public void StartAllTimers()
        {
            foreach (SpawnType s in spawnTypes)
            {
                timers[s] = 0.0f;
            }
        }

        public void ResetTimer(SpawnType spawnType)
        {
            timers[spawnType] = cooldownLimits[spawnType];
        }

        public float GetTimer(SpawnType spawnType)
        {
            return timers[spawnType];
        }

        public void StartTimer(SpawnType spawnType)
        {
            timers[spawnType] = 0.0f;
        }

        public bool IsAvailableSpawnType(SpawnType spawnType)
        {
            return timers[spawnType] >= cooldownLimits[spawnType];
        }
    }
}


