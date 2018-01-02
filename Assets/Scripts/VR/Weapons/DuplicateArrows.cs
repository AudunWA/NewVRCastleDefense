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
	private float spaceBetweenArrows = 0.3f;
	public bool collision = false;
	
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		if (collision)
		{
			gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, -9.81f, 0);
		}
		
	}

	void ArrowFired()
	{
		Invoke(nameof(Duplicate), 0.5f);
	}

    void Duplicate()
    {
	    
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
}
