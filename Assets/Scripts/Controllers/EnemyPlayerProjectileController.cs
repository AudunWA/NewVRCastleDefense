using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.Assertions;
using Valve.VR.InteractionSystem;
using Player = Valve.VR.InteractionSystem.Player;
using Random = UnityEngine.Random;

public class EnemyPlayerProjectileController : MonoBehaviour
{

    public GameEntity targetMinion;
    public GameObject parentGameObject;
    public EnemyPlayerController enemyPlayer;
    private Vector3 targetPosition;
    private float arrowDamage;

    public bool moving;
    public float enemyMovementspeed;

    private float distance;
    // Velcoity needs to be hard coded for this, improvements possible
    private float velocity = 50f;

    private float yValueAboveMinion = 3.0f;

    private WorldController wc;


    public void Awake()
    {
    }

    // Use this for initialization
    void Start()
    {
        Invoke("Destroy", 10.0f);
        if (enemyPlayer?.gameObject != null)
        {
            arrowDamage = enemyPlayer.Damage;
            if (targetMinion != null)
            {
                distance = FindDistanceToTarget();
                ShootArrow();
            }
            if (enemyPlayer.Level > 2) velocity = 70f;
        }
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameObject.GetComponent<Rigidbody>().velocity != Vector3.zero)
        {
            // ArrowController looks 1.08 times steeper than the velocity for more realistic impacts
            transform.rotation = Quaternion.LookRotation(gameObject.GetComponent<Rigidbody>().velocity * 1.15f);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == parentGameObject)
        {
            return;
        }
        gameObject.SetActive(false);
        if (collision.gameObject.CompareTag("minion"))
        {
            // DO damage
            if (gameObject?.GetComponent<ExplodeOnCollision>() || gameObject?.GetComponent<DuplicateArrows>())
            {
                
            }
            else
            {
                MinionController controller = collision.gameObject.GetComponent<MinionController>();
                controller.Minion.TakeDamage(arrowDamage);
            }
        }
        Destroy(gameObject);
    }

    private void ShootArrow()
    {
        float angle = -TrajectoryCalculations();

        Vector3 arrowposition = new Vector3(0, yValueAboveMinion, 0);
        targetPosition = targetMinion.Position;
        arrowposition = arrowposition + enemyPlayer.transform.position;

        // Rotate to position of parent minion

        // Decides if the archer should hit its target

        Quaternion angleToMinion = Quaternion.Euler(0, 90f - AngleCalculator(parentGameObject.transform.position, targetPosition), 0);

        // Rotate upwards

        if (float.IsNaN(angle))
        {
            Debug.Log("ArrowController path can't be calculated for: " + parentGameObject.name + ". This means the targeted attack position is too far out of range. Known issue on castles.");
        }
        else
        {
            angleToMinion *= Quaternion.Euler(angle, 0, 0);
        }

        transform.rotation = angleToMinion;//Quaternion.Slerp(transform.rotation, angleToMinion, Time.deltaTime);

        transform.position = arrowposition;

        gameObject.GetComponent<Rigidbody>().velocity = gameObject.transform.forward * velocity * 1.02f;
        gameObject.GetComponent<Rigidbody>().AddForce(Physics.gravity * gameObject.GetComponent<Rigidbody>().mass);
    }

    private float TrajectoryCalculations()
    {
        // NOTE! this is set in the prefab under constant force
        float gravity = 4 * 9.81f;

        // + 0.5 to target a little above minion
        float yValueTarget = targetPosition.y - (yValueAboveMinion + enemyPlayer.transform.position.y);

        // This calculates the angle in radians it has to shoot to hit its target
        float thetaRad = Mathf.Atan((Mathf.Pow(velocity, 2.0f) - Mathf.Sqrt(
            Mathf.Pow(velocity, 4.0f) - gravity *
            (gravity * Mathf.Pow(distance, 2.0f) +
            2 * yValueTarget * Mathf.Pow(velocity, 2.0f)))) /
            (gravity * distance));
        float theta = thetaRad * Mathf.Rad2Deg;

        //float theta = 0.5f * Mathf.Asin(gravity * distance / Mathf.Pow(velocity, 2.0f));
        return theta;
    }

    // This calculates the distance to target, used as the x value in addition to angle to target to reduce angle calculation to 2 dimentions.
    private float FindDistanceToTarget()
    {

        Vector3 targetPositionDist = new Vector3(targetMinion.Position.x, targetMinion.Position.y, targetMinion.Position.z);

        if (moving)
        {
            if (targetMinion.Player.PlayerType == PlayerType.Good)
            {
                targetPositionDist.z -= enemyMovementspeed;
            }
            else
            {
                targetPositionDist.z += enemyMovementspeed;
            }
        }

        return Vector3.Distance(parentGameObject.transform.position, targetPositionDist);
    }

    public float AngleCalculator(Vector3 thisObject, Vector3 targetObject)
    {
        Vector2 vec1 = new Vector2(thisObject.x, thisObject.z);
        Vector2 vec2 = new Vector2(targetObject.x, targetObject.z);

        Vector2 diference = vec2 - vec1;
        float sign = (vec2.y < vec1.y) ? -1.0f : 1.0f;
        return Vector2.Angle(Vector2.right, diference) * sign;
    }
}
