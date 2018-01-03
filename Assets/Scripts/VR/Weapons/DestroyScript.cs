using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyScript : MonoBehaviour
{
	
	// Use this for initialization
	void Start ()
	{
		float random = Random.Range(0, 20);
		Invoke(nameof(Destroy),random);
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	void Destroy()
	{
		Destroy(gameObject);
	}
}
