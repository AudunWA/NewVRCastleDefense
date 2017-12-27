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
    private int saveMoneyGoal;
    public AIAction CurrentAction;
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

    public int SaveMoneyGoal
    {
        get { return saveMoneyGoal; }
        set { saveMoneyGoal = value; }
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
        SpawnType maxDiffAction = SpawnType.Fighter;
        int maxDiff = Int32.MinValue;
        foreach (SpawnType s in spawnTypes)
        {
            if (!availableAction[s]) continue;
            int diff = otherCount[s] - count[s];
            if (diff > maxDiff)
            {
                maxDiff = diff;
                maxDiffAction = s;
            }
        }
        return maxDiffAction;
    }

    private void CountOtherMinions(Dictionary<SpawnType, int> evilMinionsCounts)
    {
        foreach (Minion m in OtherPlayer.Minions)
        {
            otherMinionCounts[m.SpawnType]++;
        }
    }
    private void CountMinions(Dictionary<SpawnType, int> minionsCounts)
    {
        foreach (Minion m in Player.Minions)
        {
            minionsCounts[m.SpawnType]++;
        }
    }

    private void CompareLevels()
    {
        int sum = 0, otherSum = 0;
        foreach (SpawnType s in otherPlayer.MinionStatistics.Keys.ToList())
        {
            otherSum += otherPlayer.MinionStatistics[s].Level;
            sum += player.MinionStatistics[s].Level;
        }
        CountOtherMinions(otherMinionCounts);
        CountMinions(minionCounts);
        if (sum <= otherSum && (double)player.Minions.Count / otherPlayer.Minions.Count > 0.5) CurrentAction = AIAction.Upgrade;
    }

    private void FindIdealUpgradeType()
    {
        int minCost = 10000000;
        foreach (SpawnType s in Player.MinionStatistics.Keys.ToList())
        {
            int cost = Player.MinionStatistics[s].LevelUpgradeCost;
            if (minCost > cost)
            {
                minCost = cost;
                CurrentUpgradeType = s;
            }
        }
        if (minCost == 10000000) minCost = 0;
        saveMoneyGoal = minCost;
    }
    /*
     * Get the action the AI wants to do 
     */
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
        //Default
        FindIdealSpawnType();
        FindIdealUpgradeType();
        CompareLevels();
        if (CurrentAction == AIAction.Upgrade)
        {
            if (player.MinionStatistics[CurrentUpgradeType].LevelUpgradeCost <= player.Money || (double)player.Minions.Count / otherPlayer.Minions.Count < 0.5)
            {
                CurrentAction = AIAction.Spawn;
            }
        }
    }
}
