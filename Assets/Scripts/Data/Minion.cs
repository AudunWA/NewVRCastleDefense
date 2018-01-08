﻿using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Minion : GameEntity{

    public enum MinionState { Moving, Fighting, Dead, Waiting, EnemyFound };
    [SerializeField] protected float armor;
    [SerializeField] protected int level;
    [SerializeField] private Dictionary<MinionAttribute, int> levels; // Levels on different abilities
    [SerializeField] protected float range;
    [SerializeField] protected float damage;
    [SerializeField] protected float movementspeed;
    [SerializeField] protected float attackCooldownTime;
    [SerializeField] private int bounty;
    [SerializeField] private int cost;
    [SerializeField] private MinionState state;
    [SerializeField] private SpawnType spawnType;

    public Minion( SpawnType spawnType,Player player, int level, float armor, float range, float damage, float movementspeed, float attackCooldownTime, float health, Vector3 position):base(player,health,position)
    {
        this.spawnType = spawnType;
        this.level = level;
        this.armor = armor;
        this.range = range;
        this.damage = damage;
        this.movementspeed = movementspeed;
        this.attackCooldownTime = attackCooldownTime;
    }

    public Minion(Player player, MinionStat stat, Vector3 position) : base(player, stat.Abilities[MinionAttribute.Health], position)
    {
        spawnType = stat.SpawnType;
        level = stat.GetLevel();
        armor = stat.Abilities[MinionAttribute.Armor];
        range = stat.Abilities[MinionAttribute.Range];
        damage = stat.Abilities[MinionAttribute.Damage];
        movementspeed = stat.Abilities[MinionAttribute.Movementspeed];
        attackCooldownTime = stat.Abilities[MinionAttribute.AttackCooldownTime];
        cost = stat.Cost;
        levels = stat.Levels;
        bounty = stat.Bounty;
    }
    public MinionState State
    {
        get { return state; }
        set { state = value; }
    }

    public int Cost
    {
        get { return cost; }
        set { cost = value; }
    }



    public SpawnType SpawnType
    {
        get { return spawnType; }
        set { spawnType = value; }
    }

    public Minion(Player player, int level, Vector3 position, SpawnType spawnType) : base(player, level, position)
    {
        SpawnType = spawnType;
    }

    public Dictionary<MinionAttribute, int> Levels
    {
        get { return levels; }
        set { levels = value; }
    }

    public int Bounty
    {
        get { return bounty; }
        set { bounty = value; }
    }


    public int Level
    {
        get { return level; }
        set { level = value; }
    }
    public float Armor
    {
        get
        {
            return armor;
        }

        set
        {
            armor = value;
        }
    }

    public float Range
    {
        get
        {
            return range;
        }

        set
        {
            range = value;
        }
    }

    public float Damage
    {
        get
        {
            return damage;
        }

        set
        {
            damage = value;
        }
    }

    public float Movementspeed
    {
        get
        {
            return movementspeed;
        }

        set
        {
            movementspeed = value;
        }
    }

    public float AttackCooldownTime
    {
        get
        {
            return attackCooldownTime;
        }

        set
        {
            attackCooldownTime = value;
        }
    }

    /// <summary>
    /// Applies damage to this minion. The damage gets reduced by this minion's armor
    /// </summary>
    /// <param name="baseDamage">The damage to apply</param>
    public override void TakeDamage(float baseDamage, SpawnType attackerSpawnType)
    {
        float armorMultiplier;
        if (SpawnType == SpawnType.Tank && attackerSpawnType == SpawnType.Mage) armorMultiplier = 0.0f;
        else armorMultiplier = 0.33f;
        if (baseDamage <= armorMultiplier * armor)
        {
            base.TakeDamage(1, attackerSpawnType); // Armor nullifies damage
        }
        else
        {
            base.TakeDamage(baseDamage - armorMultiplier * armor, attackerSpawnType);
        }
    }

    /// <summary>
    /// Applies damage to this minion. The damage gets reduced by this minion's armor
    /// </summary>
    /// <param name="baseDamage">The damage to apply</param>
    public override void TakeDamage(float baseDamage)
    {
        float armorMultiplier = 0.33f;
        if (baseDamage <= armorMultiplier * armor)
        {
            base.TakeDamage(1); // Armor nullifies damage
        }
        else
        {
            base.TakeDamage(baseDamage - armorMultiplier * armor);
        }
    }
}

