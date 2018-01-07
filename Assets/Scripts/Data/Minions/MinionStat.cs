using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public struct MinionStat
{
    public SpawnType SpawnType { get; }
    public Dictionary<MinionAttribute, int> Levels { get; }
    public int Bounty { get; }
    public Dictionary<MinionAttribute, float> Abilities { get; }
    public int Cost { get; }
    public Dictionary<MinionAttribute, int> LevelUpgradeCost { get; }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="spawnType"></param>
    /// <param name="armor"></param>
    /// <param name="levels"></param>
    /// <param name="range"></param>
    /// <param name="bounty"></param>
    /// <param name="damage"></param>
    /// <param name="movementspeed"></param>
    /// <param name="attackCooldownTime"></param>
    /// <param name="cost"></param>
    /// <param name="health"></param>
    public MinionStat(SpawnType spawnType, float armor, Dictionary<MinionAttribute, int> levels, float range, int bounty, float damage, float movementspeed, float attackCooldownTime, int cost, float health, Dictionary<MinionAttribute, int> levelUpgradeCost)
    {
        SpawnType = spawnType;
        Levels = levels;
        Bounty = bounty;
        Cost = cost;
        LevelUpgradeCost = levelUpgradeCost;
        Abilities = new Dictionary<MinionAttribute, float>
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
    /// <param name="levels"></param>
    /// <param name="range"></param>
    /// <param name="bounty"></param>
    /// <param name="damage"></param>
    /// <param name="movementspeed"></param>
    /// <param name="attackCooldownTime"></param>
    /// <param name="cost"></param>
    /// <param name="health"></param>
    public MinionStat(SpawnType spawnType, Dictionary<MinionAttribute, int> levels, int bounty, int cost,  Dictionary<MinionAttribute, int> levelUpgradeCost, Dictionary<MinionAttribute, float> abilities)
    {
        SpawnType = spawnType;
        Levels = new Dictionary<MinionAttribute, int>(levels);
        Bounty = bounty;
        Cost = cost;
        LevelUpgradeCost = new Dictionary<MinionAttribute, int>(levelUpgradeCost);
        Abilities = new Dictionary<MinionAttribute, float>(abilities);
    }

    // Initial
    public MinionStat(SpawnType spawnType, float armor, float range, int bounty, float damage, float movementspeed, float attackCooldownTime, int cost, float health, Dictionary<MinionAttribute, int> levelUpgradeCost)
    {
        SpawnType = spawnType;
        Levels = new Dictionary<MinionAttribute, int>
        {
            {MinionAttribute.Armor, 1},
            {MinionAttribute.Range,1},
            {MinionAttribute.Damage,1},
            {MinionAttribute.Movementspeed,1},
            {MinionAttribute.AttackCooldownTime,1},
            {MinionAttribute.Health,1}
        };
      
        Abilities = new Dictionary<MinionAttribute, float>
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
        LevelUpgradeCost = new Dictionary<MinionAttribute, int>(levelUpgradeCost);
    }

    public int GetLevel()

    {
        return Levels.Sum(x => x.Value)-Levels.Count() +1;
    }

    public MinionStat Upgrade(MinionStat minionstat, MinionStat additionStat, MinionAttribute attr)
    {
        if (LevelUpgradeCost[attr] < 0) return minionstat;
        minionstat.Levels[attr]++;
        minionstat.LevelUpgradeCost[attr] += additionStat.LevelUpgradeCost[attr];
        minionstat.Abilities[attr] += additionStat.Abilities[attr];
        return new MinionStat(
           minionstat.SpawnType,
           minionstat.Levels,
           minionstat.Bounty + additionStat.Bounty,
           minionstat.Cost + additionStat.Cost,
           minionstat.LevelUpgradeCost,
           minionstat.Abilities
           );
    }
   
}

public enum MinionAttribute
{
    Armor, Range, Health, Damage, Movementspeed, AttackCooldownTime
}