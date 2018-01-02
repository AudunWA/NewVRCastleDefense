using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

// TODO: FIXME: Use stuct for stat instead of class?
public struct MinionStat
{
    public SpawnType SpawnType { get; }
    public float Armor { get; } 
    public Dictionary<MinionAttribute, int> Level { get; }
    public float Range { get; }
    public int Bounty { get; }
    public float Damage { get; }
    public float Movementspeed { get; }
    public float AttackCooldownTime { get; }
    public int Cost { get; }
    public float Health { get; }
    public Dictionary<MinionAttribute, int> LevelUpgradeCost { get; }
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
    public MinionStat(SpawnType spawnType, float armor, Dictionary<MinionAttribute, int> level, float range, int bounty, float damage, float movementspeed, float attackCooldownTime, int cost, float health, Dictionary<MinionAttribute, int> levelUpgradeCost)
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

    // Initial
    public MinionStat(SpawnType spawnType, float armor, float range, int bounty, float damage, float movementspeed, float attackCooldownTime, int cost, float health, Dictionary<MinionAttribute, int> levelUpgradeCost)
    {
        SpawnType = spawnType;
        Armor = armor;
        Level = new Dictionary<MinionAttribute, int>
        {
            {MinionAttribute.Armor, 1},
            {MinionAttribute.Range,1},
            {MinionAttribute.Damage,1},
            {MinionAttribute.Movementspeed,1},
            {MinionAttribute.AttackCooldownTime,1},
            {MinionAttribute.Health,1}
        };
        Range = range;
        Bounty = bounty;
        Damage = damage;
        Movementspeed = movementspeed;
        AttackCooldownTime = attackCooldownTime;
        Cost = cost;
        Health = health;
        LevelUpgradeCost = levelUpgradeCost;
    }

    public int GetLevel()
    {
        return Level.Sum(x => x.Value)-Level.Count() +1;
    }

    public static MinionStat Upgrade(MinionStat minionstat, MinionStat additionStat, MinionAttribute attr)
    {
        minionstat.Level[attr]++;
        minionstat.LevelUpgradeCost[attr]+= additionStat.LevelUpgradeCost[attr];
        return new MinionStat(
            minionstat.SpawnType,
            minionstat.Armor + (attr==MinionAttribute.Armor?additionStat.Armor:0),
            minionstat.Level,
            minionstat.Range + (attr==MinionAttribute.Range?additionStat.Range:0),
            minionstat.Bounty + additionStat.Bounty,
            minionstat.Damage + (attr==MinionAttribute.Damage?additionStat.Damage:0),
            minionstat.Movementspeed + (attr==MinionAttribute.Movementspeed?additionStat.Movementspeed:0),
            minionstat.AttackCooldownTime + (attr==MinionAttribute.AttackCooldownTime?additionStat.AttackCooldownTime:0),
            minionstat.Cost + additionStat.Cost,
            minionstat.Health + (attr==MinionAttribute.Health?additionStat.Health:0),
            minionstat.LevelUpgradeCost);
    }
   
}

public enum MinionAttribute
{
    Armor, Range, Health, Damage, Movementspeed, AttackCooldownTime
}