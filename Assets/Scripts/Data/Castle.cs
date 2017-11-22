using UnityEngine;

public class Castle : GameEntity
{
    public Vector3 size { get; set; }
    public Castle(float health, Vector3 position):base(health, position)
    {
    }

}
