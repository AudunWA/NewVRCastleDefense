using System.Collections.Generic;
using System;
using System.Linq;

public class GameAI
{
    public enum AIAction
    {
        Wait, Spawn, Upgrade, Shoot
    }
    public int Level { get; set; }
    private int spawnCost;
    private Dictionary<SpawnType, bool> availableAction = new Dictionary<SpawnType, bool>();
    private Dictionary<SpawnType, int> minionCounts;
    private Dictionary<SpawnType, int> otherMinionCounts;
    public Dictionary<SpawnType, bool> AvailableTimers { get; set; }
    public int UpgradeMoneyGoal { get; private set; }
    public int SpawnMoneyGoal { get; private set; }
    public AIAction CurrentAction { get; private set; }
    public SpawnType CurrentSpawnType { get; private set; }
    private SpawnType cheapestSpawnType = SpawnType.Fighter;
    public SpawnType CurrentUpgradeType { get; private set; }
    public MinionAttribute CurrentAttribute { get; private set; }
    private Player player;
    private Player otherPlayer;
    public GameAI()
    {
        InitAvailableActions();
        ResetCounts();
        CurrentAction = AIAction.Spawn;
        spawnCost = 50;
    }
    private void InitAvailableActions()
    {
        availableAction.Add(SpawnType.Fighter, true);
        availableAction.Add(SpawnType.Tank, true);
        availableAction.Add(SpawnType.Mage, true);
        availableAction.Add(SpawnType.Archer, true);
    }
    private void ResetCounts()
    {
        minionCounts = new Dictionary<SpawnType, int>();
        otherMinionCounts = new Dictionary<SpawnType, int>();
        minionCounts.Add(SpawnType.Fighter, 0);
        minionCounts.Add(SpawnType.Tank, 0);
        minionCounts.Add(SpawnType.Mage, 0);
        minionCounts.Add(SpawnType.Archer, 0);

        otherMinionCounts.Add(SpawnType.Fighter, 0);
        otherMinionCounts.Add(SpawnType.Tank, 0);
        otherMinionCounts.Add(SpawnType.Mage, 0);
        otherMinionCounts.Add(SpawnType.Archer, 0);
    }

    public Player Player
    {
        get
        {
            return player;
        }

        set
        {
            player = value;
        }
    }

    public Player OtherPlayer
    {
        get
        {
            return otherPlayer;
        }

        set
        {
            otherPlayer = value;
        }
    }

    public Dictionary<SpawnType, bool> AvailableAction
    {
        get
        {
            return availableAction;
        }

        set
        {
            availableAction = value;
        }
    }

    /**
     * Finds the biggest difference in type of unit. 
     * Ex: If the evil has many more archers than me, balance by deploying more archers 
     */
    private SpawnType FindBiggestDifference(Dictionary<SpawnType, int> otherCount, Dictionary<SpawnType, int> count)
    {
        List<SpawnType> spawnTypes = player.MinionStatistics.Keys.ToList();
        SpawnType maxDiffType = SpawnType.Fighter;
        int maxDiff = Int32.MinValue;
        foreach (SpawnType s in spawnTypes)
        {
            if (!availableAction[s]) continue;
            int diff = otherCount[s] - count[s];
            if (diff > maxDiff)
            {
                maxDiff = diff;
                maxDiffType = s;
            }
        }
        return maxDiffType;
    }

    private SpawnType GetRandomSpawnTypeExclude(SpawnType spawnType)
    {
        List<SpawnType> s = new List<SpawnType>{SpawnType.Fighter, SpawnType.Archer, SpawnType.Tank, SpawnType.Mage};
        s.Remove(spawnType);
        return s[UnityEngine.Random.Range(0, 3)];
    }

    private void CountOtherMinions()
    {
        foreach (Minion m in OtherPlayer.Minions)
        {
            otherMinionCounts[m.SpawnType]++;
        }
    }
    private void CountMinions()
    {
        foreach (Minion m in Player.Minions)
        {
            minionCounts[m.SpawnType]++;
        }
    }
    /// <summary>
    /// Compare total levels of different minion types. In the case of an imbalance,
    ///  ie. the enemy has upgraded something, then upgrade something
    /// </summary>
    private void EvaluateMinionUpgrade()
    {
        int sum = 0, otherSum = 0;
        foreach (SpawnType s in otherPlayer.MinionStatistics.Keys.ToList())
        {

            otherSum += otherPlayer.MinionStatistics[s].GetLevel();
            sum += player.MinionStatistics[s].GetLevel();
        }
        if (sum <= otherSum) CurrentAction = AIAction.Upgrade;
    }
    // Finds the type an minionattribute which has the lowest cost to upgrade
    private void FindIdealUpgradeType()
    {
        int minCost = 10000000;
        List<SpawnType> spawnTypes = Player.MinionStatistics.Keys.ToList();
        
        foreach (SpawnType s in spawnTypes)
        {
            foreach (MinionAttribute attr in Player.MinionStatistics[s].Levels.Keys)
            {
                int cost = Player.MinionStatistics[s].LevelUpgradeCost[attr];
                if ( cost > 0 && minCost > cost)
                {
                    minCost = cost;
                    CurrentUpgradeType = UnityEngine.Random.Range(0,100) > 20?s:GetRandomSpawnTypeExclude(s);
                    CurrentAttribute = attr;
                }
            }
        }
        Dictionary<MinionAttribute, int> levels = Player.MinionStatistics[CurrentUpgradeType].Levels;
        if (CurrentUpgradeType == SpawnType.Tank && levels[MinionAttribute.Health] < 10)
        {
            CurrentAttribute = MinionAttribute.Health;
        }
        else if(levels[MinionAttribute.Damage] < 10)
        {
            CurrentAttribute = MinionAttribute.Damage;
        }
        else if(levels[MinionAttribute.AttackCooldownTime] < 10)
        {
            CurrentAttribute = MinionAttribute.AttackCooldownTime;
        }
        else if ((CurrentUpgradeType == SpawnType.Tank || CurrentUpgradeType == SpawnType.Fighter) &&
                 levels[MinionAttribute.Armor] < 10)
        {
            CurrentAttribute = MinionAttribute.Armor;
        }
        if (minCost == 10000000) minCost = 0;
        UpgradeMoneyGoal = minCost;
    }

    private SpawnType StrongestSpawnType()
    {
        SpawnType highest = SpawnType.Fighter;
        foreach (KeyValuePair<SpawnType, MinionStat> stat in Player.MinionStatistics)
        {
            if (stat.Value.GetLevel() >= Player.MinionStatistics[highest].GetLevel())
            {
                highest = stat.Key;
            }
        }
        SpawnMoneyGoal = player.MinionStatistics[highest].Cost;
        return highest;
    }
    /// <summary>
      /// Finds the type of unit to spawn next
      /// </summary>
    private void FindIdealSpawnType()
    {
        CountOtherMinions();
        CountMinions();
        cheapestSpawnType = FindBiggestDifference(otherMinionCounts, minionCounts);
        ResetCounts();
        CurrentSpawnType = StrongestSpawnType();
        int cheapestCost = player.MinionStatistics[cheapestSpawnType].Cost;
        int rnd = UnityEngine.Random.Range(0, 2);
        if ((Player.Money > SpawnMoneyGoal + cheapestCost && rnd == 0) || (otherPlayer.Minions.Count > player.minions.Count * 3 && Player.Minions.Count > 2))
        {
            CurrentSpawnType = cheapestSpawnType;
        }
    }
    public void FindNextAction()
    {
        FindIdealSpawnType();
        spawnCost = player.MinionStatistics[CurrentSpawnType].Cost;
        if (Level < 2)
        {
            CurrentAction = AIAction.Spawn;
            return;
        }
        FindIdealUpgradeType();
        EvaluateMinionUpgrade();
        int rnd = UnityEngine.Random.Range(0, 2);
        if (CurrentAction == AIAction.Upgrade)
        {
            // If more than sufficient amount of money, or enemy has more than twice as many minions, spawn instead of saving money
            if (player.Money >= UpgradeMoneyGoal+spawnCost && rnd < 1 || otherPlayer.Minions.Count >= player.Minions.Count*2 )
            {
                CurrentAction = AIAction.Spawn;
            }
        }
    }
}
