using System;
using UnityEngine;
[Serializable]
public abstract class GameEntity {

    [SerializeField] private Player player;
    [SerializeField] protected float health;
    [SerializeField] private Vector3 position;
    public GameObject gameObject;
    private Collider collider;

    public GameEntity(Player player, float health, Vector3 position){
        this.player = player;
        this.health = health;
        this.position = position;
    }
    public GameEntity(float health, Vector3 position)
    {
        this.health = health;
        this.position = position;
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

    public float Health
    {
        get
        {
            return health;
        }

        set
        {
            health = value;
        }
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
    public bool IsAlive
    {
        get
        {
            return Health > 0;
        }
    }

    /// <summary>
    /// Calculates the position that should be set as destination for the nav mesh agents
    /// </summary>
    /// <param name="attackerPosition">The position of the entity that is attacking this one</param>
    public Vector3 GetAttackPosition(Vector3 attackerPosition)
    {
        if (collider == null)
            collider = gameObject.GetComponent<Collider>();
        return collider.ClosestPoint(attackerPosition);
    }

    /// <summary>
    /// Applies damage to this minion.
    /// Override this method to add more specific calculations (e.g. armor)
    /// </summary>
    /// <param name="baseDamage">The damage to apply</param>
    public virtual void TakeDamage(float baseDamage)
    {
        Health = Mathf.Max(Health - baseDamage, 0);
    }
}
