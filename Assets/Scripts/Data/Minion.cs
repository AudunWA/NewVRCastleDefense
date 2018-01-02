using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class Minion : GameEntity{

    public enum minionState { Moving, Fighting, Dead, Waiting, EnemyFound };
    [SerializeField] protected float armor;
    [SerializeField] protected int level;
    [SerializeField] protected float range;
    [SerializeField] protected float damage;
    [SerializeField] protected float movementspeed;
    [SerializeField] protected float attackCooldownTime;
    [SerializeField] private int bounty;
    [SerializeField] private int cost;
    [SerializeField] private minionState state;
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

    public Minion(Player player, MinionStat stat, Vector3 position) : base(player, stat.Health, position)
    {
        spawnType = stat.SpawnType;
        level = stat.GetLevel();
        armor = stat.Armor;
        range = stat.Range;
        damage = stat.Damage;
        movementspeed = stat.Movementspeed;
        attackCooldownTime = stat.AttackCooldownTime;
        cost = stat.Cost;
        bounty = stat.Bounty;
    }
    public minionState State
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
    public override void TakeDamage(float baseDamage)
    {
        const float armorMultiplier = 0.2f;
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

