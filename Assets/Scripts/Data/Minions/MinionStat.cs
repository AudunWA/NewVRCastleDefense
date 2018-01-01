using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// TODO: FIXME: Use stuct for stat instead of class?
public struct MinionStat
{
    private const double MONEY_INDEX = 1.1;
    public SpawnType SpawnType { get; }
    public float Armor { get; } 
    public int Level { get; }
    public float Range { get; }
    public int Bounty { get; }
    public float Damage { get; }
    public float Movementspeed { get; }
    public float AttackCooldownTime { get; }
    public int Cost { get; }
    public float Health { get; }
    public int LevelUpgradeCost { get; }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="spawnType"></param>
    /// <param name="armor"></param>
    /// <param name="level"></param>
    /// <param name="range"></param>
    /// <param name="bounty"></param>
    /// <param name="damage"></param>
    /// <param name="movementspeed"></param>
    /// <param name="attackCooldownTime"></param>
    /// <param name="cost"></param>
    /// <param name="health"></param>
    public MinionStat(SpawnType spawnType, float armor, int level, float range, int bounty, float damage, float movementspeed, float attackCooldownTime, int cost, float health, int levelUpgradeCost)
    {
        SpawnType = spawnType;
        Armor = armor;
        Level = level;
        Range = range;
        Bounty = bounty;
        Damage = damage;
        Movementspeed = movementspeed;
        AttackCooldownTime = attackCooldownTime;
        Cost = cost;
        Health = health;
        LevelUpgradeCost = levelUpgradeCost;
    }

    private static int MoneyIndex(int val)
    {
        return (int) (val * MONEY_INDEX);
    }

    // Initial
    public MinionStat(SpawnType spawnType, float armor, float range, int bounty, float damage, float movementspeed, float attackCooldownTime, int cost, float health, int levelUpgradeCost)
    {
        SpawnType = spawnType;
        Armor = armor;
        Level = 1;
        Range = range;
        Bounty = bounty;
        Damage = damage;
        Movementspeed = movementspeed;
        AttackCooldownTime = attackCooldownTime;
        Cost = cost;
        Health = health;
        LevelUpgradeCost = levelUpgradeCost;
    }

    public static MinionStat operator +(MinionStat left, MinionStat right)
    {
        return new MinionStat(
            left.SpawnType, 
            left.Armor + right.Armor, 
            left.Level + 1,
            left.Range + right.Range, 
            left.Bounty + right.Bounty, 
            left.Damage + right.Damage,
            left.Movementspeed + right.Movementspeed, 
            left.AttackCooldownTime + right.AttackCooldownTime, 
            left.Cost + right.Cost, 
            left.Health + right.Health, 
            left.LevelUpgradeCost+right.LevelUpgradeCost);
    }

    public static MinionStat Upgrade(MinionStat minionstat, MinionStat additionStat, MinionAttribute attr)
    {
        return new MinionStat(
            minionstat.SpawnType,
            minionstat.Armor + (attr==MinionAttribute.Armor?additionStat.Armor:0),
            minionstat.Level + 1,
            minionstat.Range + (attr==MinionAttribute.Range?additionStat.Range:0),
            minionstat.Bounty + additionStat.Bounty,
            minionstat.Damage + (attr==MinionAttribute.Damage?additionStat.Damage:0),
            minionstat.Movementspeed + (attr==MinionAttribute.Movementspeed?additionStat.Movementspeed:0),
            minionstat.AttackCooldownTime + (attr==MinionAttribute.AttackCooldownTime?additionStat.AttackCooldownTime:0),
            minionstat.Cost + additionStat.Cost,
            minionstat.Health + (attr==MinionAttribute.Health?additionStat.Health:0),
            minionstat.LevelUpgradeCost + additionStat.LevelUpgradeCost);
    }
   
}

public enum MinionAttribute
{
    Armor, Range, Health, Damage, Movementspeed, AttackCooldownTime
}