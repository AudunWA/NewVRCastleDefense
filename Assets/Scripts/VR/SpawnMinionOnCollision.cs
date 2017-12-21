using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SpawnMinionOnCollision : MonoBehaviour
{
    [SerializeField] private SpawnType spawnType;
    private WorldController worldController;

    void Awake()
    {
        worldController = GameObject.FindGameObjectWithTag("World").GetComponent<WorldController>();
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(gameObject);
        try
        {
            Vector3 groundPosition = new Vector3(transform.position.x, 0, transform.position.z);
            worldController.GoodPlayer.SpawnController.Spawn(spawnType, groundPosition);
            Destroy(gameObject);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

    }
}
