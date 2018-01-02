using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

// TODO: FIXME: Use stuct for stat instead of class?
public struct MinionStat
{
    public SpawnType SpawnType { get; }
    public Dictionary<MinionAttribute, int> Level { get; }
    public int Bounty { get; }
    public Dictionary<MinionAttribute, float> Attributes { get; }
    public int Cost { get; }
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
        Level = level;
        Bounty = bounty;
        Cost = cost;
        LevelUpgradeCost = levelUpgradeCost;
        Attributes = new Dictionary<MinionAttribute, float>
        {
            { MinionAttribute.Armor, armor},
            { MinionAttribute.Range, range},
            { MinionAttribute.Damage, damage },
            { MinionAttribute.Movementspeed, movementspeed },
            { MinionAttribute.AttackCooldownTime, attackCooldownTime },
            { MinionAttribute.Health, health }
        };
    }

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
    public MinionStat(SpawnType spawnType, Dictionary<MinionAttribute, int> level, int bounty, int cost,  Dictionary<MinionAttribute, int> levelUpgradeCost, Dictionary<MinionAttribute, float> attributes)
    {
        SpawnType = spawnType;
        Level = level;
        Bounty = bounty;
        Cost = cost;
        LevelUpgradeCost = levelUpgradeCost;
        Attributes = attributes;
    }

    // Initial
    public MinionStat(SpawnType spawnType, float armor, float range, int bounty, float damage, float movementspeed, float attackCooldownTime, int cost, float health, Dictionary<MinionAttribute, int> levelUpgradeCost)
    {
        SpawnType = spawnType;
        Level = new Dictionary<MinionAttribute, int>
        {
            {MinionAttribute.Armor, 1},
            {MinionAttribute.Range,1},
            {MinionAttribute.Damage,1},
            {MinionAttribute.Movementspeed,1},
            {MinionAttribute.AttackCooldownTime,1},
            {MinionAttribute.Health,1}
        };
        Attributes = new Dictionary<MinionAttribute, float>
        {
            { MinionAttribute.Armor, armor},
            { MinionAttribute.Range, range},
            { MinionAttribute.Damage, damage },
            { MinionAttribute.Movementspeed, movementspeed },
            { MinionAttribute.AttackCooldownTime, attackCooldownTime },
            { MinionAttribute.Health, health }
        };
        Bounty = bounty;
        Cost = cost;
        LevelUpgradeCost = levelUpgradeCost;
    }

    public int GetLevel()
    {
        return Level.Sum(x => x.Value)-Level.Count() +1;
    }

    public static MinionStat Upgrade(MinionStat minionstat, MinionStat additionStat, MinionAttribute attr)
    {
        minionstat.Level[attr]++;
        minionstat.LevelUpgradeCost[attr] += additionStat.LevelUpgradeCost[attr];
        minionstat.Attributes[attr] += additionStat.Attributes[attr];
        return new MinionStat(
           minionstat.SpawnType,
           minionstat.Level,
           minionstat.Bounty,
           minionstat.Cost,
           minionstat.LevelUpgradeCost,
           minionstat.Attributes
           );
    }
   
}

public enum MinionAttribute
{
    Armor, Range, Health, Damage, Movementspeed, AttackCooldownTime
}