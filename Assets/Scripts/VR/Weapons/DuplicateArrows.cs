using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using NUnit.Framework.Constraints;
using UnityEngine;
using Random = UnityEngine.Random;

public class DuplicateArrows : MonoBehaviour
{
	public GameObject copy;
	public int copyAmount;
	private float spaceBetweenArrows = 0.4f;
	public bool collision = false;
	[SerializeField] private GameObject arrowRainParticleEffect;
	private float startDistance;
	private bool split = false;
	
	
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (GetDistance() > 20 && startDistance > 0 && !split)
		{
			Invoke(nameof(Duplicate), 0.5f);
			split = true;
		}
		
	}

	void ArrowFired()
	{
		startDistance = gameObject.transform.position.z;
		//Invoke(nameof(Duplicate), 0.5f);
	}

    void Duplicate()
    {
	    Instantiate(arrowRainParticleEffect, gameObject.transform.position, gameObject.transform.rotation);
	    for (int i = 0; i < copyAmount; i++)
	    {
		    float randomX = Random.Range(-25, 25);
		    float randomY = Random.Range(-25, 25);
		    float randomZ = Random.Range(-25, 25);
		    
		    GameObject go = Instantiate(copy,
			    new Vector3(gameObject.transform.position.x + (spaceBetweenArrows * randomX), gameObject.transform.position.y + (spaceBetweenArrows * randomY),
				    gameObject.transform.position.z + (spaceBetweenArrows * randomZ)), gameObject.transform.rotation);
		    go.GetComponent<FollowVelocity>().followObGameObject = gameObject;
	    }
    }

	private void OnCollisionEnter(Collision other)
	{
		CancelInvoke(nameof(Duplicate));
		collision = true;
	}

	private float GetDistance()
	{
		float distanceFromPlayer = startDistance - gameObject.transform.position.z;
		return distanceFromPlayer;
	}
}
