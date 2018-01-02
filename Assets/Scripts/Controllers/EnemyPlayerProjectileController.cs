﻿using System;
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
    private float velocity = 70f;

    private float yValueAboveMinion = 3.0f;

    private ObjectPooling dummyArrowPool;

    private WorldController wc;


    public void Awake()
    {
        dummyArrowPool = GameObject.Find("DummyArrowPool").GetComponent<ObjectPooling>();
    }

    // Use this for initialization
    private void OnEnable()
    {
        Invoke("Destroy", 10.0f);
        //float random = Random.Range(0.98f, 1.01f);
        if (enemyPlayer?.gameObject != null)
        {
            arrowDamage = enemyPlayer.damage;
            if (targetMinion != null)
            {
                distance = FindDistanceToTarget();
                ShootArrow();
            }
        }
    }

    private void Destroy()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {

        CancelInvoke();
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
        GameObject dummyArrow = dummyArrowPool.GetPooledObject();
        dummyArrow.transform.position = gameObject.transform.position;
        dummyArrow.transform.rotation = gameObject.transform.rotation;
        dummyArrow.SetActive(true);
        gameObject.SetActive(false);
        if (collision.gameObject.tag == "minion")
        {
            // DO damage
            MinionController controller = collision.gameObject.GetComponent<MinionController>();

            controller.Minion.TakeDamage(arrowDamage);
            if (controller.Minion.Health > 0)
            {
                dummyArrow.GetComponent<Transform>().SetParent(collision.gameObject.transform);
            }
            
        }
        else if (collision.gameObject.tag.Contains("Castle"))
        {
            CastleController controller = collision.gameObject.GetComponent<CastleController>();

            controller.Castle.TakeDamage(enemyPlayer.damage);


        }
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