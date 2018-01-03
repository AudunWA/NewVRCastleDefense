using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowVelocity : MonoBehaviour
{
	public GameObject followObGameObject;
	private float tolerance = 100.0f;
	private bool collision = false;
	public GameObject dummy;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	
	
	//THIS HAS TO BE NORMAL UPDATE, NOT FIXED
	private void Update()
	{
		if (followObGameObject.GetComponent<Rigidbody>().velocity.magnitude + tolerance >
		    gameObject.GetComponent<Rigidbody>().velocity.magnitude || followObGameObject.GetComponent<Rigidbody>().velocity.magnitude - tolerance <
		    gameObject.GetComponent<Rigidbody>().velocity.magnitude && !followObGameObject.GetComponent<DuplicateArrows>().collision && !collision)
		{
			gameObject.transform.rotation = followObGameObject.transform.rotation;
			gameObject.GetComponent<Rigidbody>().velocity = followObGameObject.GetComponent<Rigidbody>().velocity;
		}
	}


	private void OnCollisionEnter(Collision other)
	{
		collision = true;
		gameObject.SetActive(false);
		if (other.gameObject?.GetComponent<MinionController>())
		{
			other.gameObject?.GetComponent<MinionController>().Minion.TakeDamage(20);
			GameObject dummygo = Instantiate(dummy, gameObject.transform.position, gameObject.transform.rotation);
			FixedJoint joint = dummygo.AddComponent<FixedJoint>();
			joint.connectedBody = other.gameObject.GetComponent<Rigidbody>();
		}
		else
		{
			Instantiate(dummy, gameObject.transform.position, gameObject.transform.rotation);
		}
		Destroy(gameObject, 3.0f);
	}
}
