using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

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
		    GameObject go = Instantiate(copy,
			    new Vector3(gameObject.transform.position.x + (spaceBetweenArrows * i), gameObject.transform.position.y + (spaceBetweenArrows * i),
				    gameObject.transform.position.z + (spaceBetweenArrows * i)), gameObject.transform.rotation);
		    go.GetComponent<FollowVelocity>().followObGameObject = gameObject;
	    }
    }

	private void OnCollisionEnter(Collision other)
	{
		collision = true;
	}
}
