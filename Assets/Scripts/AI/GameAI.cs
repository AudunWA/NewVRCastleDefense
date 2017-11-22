using System.Collections.Generic;
using System;
public class GameAI {
    private Dictionary<SpawnType, bool> availableAction = new Dictionary<SpawnType, bool>();
    private Dictionary<SpawnType, int> minionCounts;
    private Dictionary<SpawnType, int> otherMinionCounts;
    public SpawnType CurrentAction { get; private set; }
    private Player player;
    private Player otherPlayer;
    public GameAI()
    {
        InitAvailableActions();
        InitCounts();
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
    private SpawnType FindBiggestDifference(Dictionary<SpawnType,int> otherCount, Dictionary<SpawnType,int> count)
    {
        SpawnType[] spawnTypes = { SpawnType.Fighter, SpawnType.Tank, SpawnType.Mage, SpawnType.Archer};
        SpawnType maxDiffAction = SpawnType.Fighter;
        int maxDiff = Int32.MinValue;
        for (int i = 0; i < spawnTypes.Length; i++)
        {
            SpawnType curr = spawnTypes[i];
            if (!availableAction[curr]) continue;
            int diff = otherCount[curr] - count[curr];
            if (diff > maxDiff)
            {
                maxDiff = diff;
                maxDiffAction = curr;
            }
        }
        return maxDiffAction;
    }

    private void CountOtherMinions(Dictionary<SpawnType,int> evilMinionsCounts)
    {
        foreach (Minion m in OtherPlayer.Minions)
        {
            otherMinionCounts[m.SpawnType]++;
        }
    }
    private void CountMinions(Dictionary<SpawnType,int> minionsCounts)
    {
        foreach (Minion m in Player.Minions)
        {
            minionsCounts[m.SpawnType]++;
        }
    }
    /*
     * Get the action the AI wants to do 
     */
    private SpawnType GetIdealAction()
    {
        CountOtherMinions(otherMinionCounts);
        CountMinions(minionCounts);
        SpawnType biggestDiff = FindBiggestDifference(otherMinionCounts, minionCounts);
        InitCounts();
        return biggestDiff;
    }
    public void FindNextAction()
    {
        //Default
        CurrentAction = GetIdealAction();
    }
}
