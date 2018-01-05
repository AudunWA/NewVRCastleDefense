using System;
using System.Collections.Generic;
using UnityEngine;

public class Player{
    string name;
    PlayerType playerType;
    public List<Minion> minions;
    private int money;
    Castle castle;
    int level;
    Vector3 position;
    private Vector3 spawnLocation;
    public Dictionary<SpawnType, MinionStat> MinionStatistics { get; set; }
    public SpawnController SpawnController { get; set; }
    public int MoneyIncrementFactor { get; set; }
    public Player(string name, PlayerType playerType, List<Minion> minions, Castle castle, int level, Vector3 position, int moneyIncrementFactor)
    {
        this.name = name;
        this.playerType = playerType;
        this.minions = minions;
        this.castle = castle;
        this.level = level;
        this.position = position;
        MoneyIncrementFactor = moneyIncrementFactor;
    }
    public Player(string name, PlayerType playerType, List<Minion> minions, Castle castle, int level, Vector3 position)
    {
        this.name = name;
        this.playerType = playerType;
        this.minions = minions;
        this.castle = castle;
        this.level = level;
        this.position = position;
    }
    public int Money
    {
        get { return money; }
        set { money = value; }
    }

    public Vector3 Position
    {
        get
        {
            return position;
        }

        set
        {
            position = value;
        }
    }

    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
        }
    }

    public Vector3 SpawnLocation
    {
        get { return spawnLocation; }
        set { spawnLocation = value; }
    }

    public PlayerType PlayerType
    {
        get
        {
            return playerType;
        }

        set
        {
            playerType = value;
        }
    }

    public void IncrementMoney()
    {
        Money += MoneyIncrementFactor;
    }

    public bool WithdrawMoney(int value)
    {
        if (value > Money) return false;
        Money -= value;
        return true;
    }

    public List<Minion> Minions
    {
        get
        {
            return minions;
        }

        set
        {
            minions = value;
        }
    }


    public Castle Castle
    {
        get
        {
            return castle;
        }

        set
        {
            castle = value;
        }
    }

    public bool AddMinion(Minion minion)
    {
        if (minion != null)
        {
            minions.Add(minion);
            return true;
        }
        return false;
            
    }
    
    public bool RemoveMinion(Minion minion)
    {
        return minions.Remove(minion);
    }
}

public enum PlayerType
{
    Evil, Good
}
