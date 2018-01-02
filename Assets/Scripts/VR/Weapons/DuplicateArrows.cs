using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class DuplicateArrows : MonoBehaviour
{
	public GameObject copy;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
		
	}

	void ArrowFired()
	{
		Debug.Log("fired");
		Invoke(nameof(Duplicate), 0.5f);
	}

    void Duplicate()
    {
        GameObject go = Instantiate(copy, new Vector3(gameObject.transform.position.x + 1, gameObject.transform.position.y + 1, gameObject.transform.position.z + 1), gameObject.transform.rotation);
	    go.GetComponent<Rigidbody>().velocity = gameObject.GetComponent<Rigidbody>().velocity.normalized * 10;
    }
}
