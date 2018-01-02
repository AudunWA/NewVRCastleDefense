using System.Collections.Generic;
using System;
using System.Linq;

public class GameAI
{
    public enum AIAction
    {
        Wait, Spawn, Upgrade, Shoot
    }
    private Dictionary<SpawnType, bool> availableAction = new Dictionary<SpawnType, bool>();
    private Dictionary<SpawnType, int> minionCounts;
    private Dictionary<SpawnType, int> otherMinionCounts;
    public int SaveMoneyGoal { get; set; }
    public AIAction CurrentAction { get; private set; }
    public SpawnType CurrentSpawnType { get; private set; }
    public SpawnType CurrentUpgradeType { get; private set; }
    private Player player;
    private Player otherPlayer;
    public GameAI()
    {
        InitAvailableActions();
        InitCounts();
        CurrentAction = AIAction.Upgrade;
    }
    private void InitAvailableActions()
    {
        availableAction.Add(SpawnType.Fighter, true);
        availableAction.Add(SpawnType.Tank, true);
        availableAction.Add(SpawnType.Mage, true);
        availableAction.Add(SpawnType.Archer, true);
    }
    private void InitCounts()
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

    private void CountOtherMinions(Dictionary<SpawnType, int> evilMinionsCounts)
    {
        foreach (Minion m in OtherPlayer.Minions)
        {
            otherMinionCounts[m.SpawnType]++;
        }
    }
    private void CountMinions(Dictionary<SpawnType, int> minionCounts)
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
    private void CompareMinionLevels()
    {
        int sum = 0, otherSum = 0;
        foreach (SpawnType s in otherPlayer.MinionStatistics.Keys.ToList())
        {
            otherSum += otherPlayer.MinionStatistics[s].GetLevel();
            sum += player.MinionStatistics[s].GetLevel();
        }
        CountOtherMinions(otherMinionCounts);
        CountMinions(minionCounts);
        if (sum <= otherSum && otherPlayer.Minions.Count < player.Minions.Count*2) CurrentAction = AIAction.Upgrade;
    }
    // Finds the type which has the lowest cost to upgrade
    private void FindIdealUpgradeType(MinionAttribute attr)
    {
        int minCost = 10000000;
        foreach (SpawnType s in Player.MinionStatistics.Keys.ToList())
        {
            int cost = Player.MinionStatistics[s].LevelUpgradeCost[attr];
            if (minCost > cost)
            {
                minCost = cost;
                CurrentUpgradeType = s;
            }
        }
        if (minCost == 10000000) minCost = 0;
        SaveMoneyGoal = minCost;
    }

    private MinionAttribute FindIdealUpgradeAttr()
    {
        var min = MinionAttribute.Armor;
        foreach (KeyValuePair<SpawnType, MinionStat> stat in Player.MinionStatistics)
        {
            // Here the AI seeks to level up the minions evenly distributed
            min = stat.Value.Level.Aggregate((l, r) => l.Value < r.Value ? l : r).Key; // Get key, in other words attr of the minimun element. 

        }

        return min;
    }

    /// <summary>
      /// Finds the type of unit to spawn next
      /// </summary>
    private void FindIdealSpawnType()
    {
        CountOtherMinions(otherMinionCounts);
        CountMinions(minionCounts);
        SpawnType biggestDiff = FindBiggestDifference(otherMinionCounts, minionCounts);
        InitCounts();
        CurrentSpawnType = biggestDiff;
    }
    public void FindNextAction()
    {
        MinionAttribute attr = MinionAttribute.Armor;
        FindIdealSpawnType();
        attr = FindIdealUpgradeAttr();
        FindIdealUpgradeType(attr);
        CompareMinionLevels();
        if (CurrentAction == AIAction.Upgrade)
        {
            // If more than sufficient amount of money, or enemy has more than twice as many minions, spawn instead of saving money
            if (player.Money >= SaveMoneyGoal || otherPlayer.Minions.Count >= player.Minions.Count*2)
            {
                CurrentAction = AIAction.Spawn;
            }
        }
    }
}
