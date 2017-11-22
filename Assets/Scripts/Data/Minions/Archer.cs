using UnityEngine;

public class Archer : Minion
{
    public Archer(Player player, MinionStat stat, Vector3 position) : base(player, stat, position)
    {
 
    }
    public Archer(SpawnType spawnType,Player player, int level, float armor, float range, float damage, float movementspeed, float attackCooldownTime, float health, Vector3 position):base(spawnType,player,level,armor,range,damage,movementspeed,attackCooldownTime,health,position){

    }
}
