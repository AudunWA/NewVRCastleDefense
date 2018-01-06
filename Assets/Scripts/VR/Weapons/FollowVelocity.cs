using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowVelocity : MonoBehaviour
{
	public GameObject followObGameObject;
	private float tolerance = 100.0f;
	private bool collision = false;
	public GameObject dummy;
	private ObjectPooling pool;
	private float damage = 50;
	
	// Use this for initialization
	void Start ()
	{
		pool = GameObject.Find("DummyArrowRainPool").GetComponent<ObjectPooling>();
		Invoke(nameof(Destroy),20.0f);
	}
	
	// Update is called once per frame

	private void OnEnable()
	{
		Invoke(nameof(Destroy),20.0f);
	}

	//THIS HAS TO BE NORMAL UPDATE, NOT FIXED
	private void Update()
	{
		if (followObGameObject.activeInHierarchy)
		{
			if (followObGameObject?.GetComponent<Rigidbody>().velocity.magnitude + tolerance >
			    gameObject?.GetComponent<Rigidbody>().velocity.magnitude || followObGameObject?.GetComponent<Rigidbody>().velocity.magnitude - tolerance <
			    gameObject?.GetComponent<Rigidbody>().velocity.magnitude && !followObGameObject.GetComponent<DuplicateArrows>().collision && !collision)
			{
				gameObject.transform.rotation = followObGameObject.transform.rotation;
				gameObject.GetComponent<Rigidbody>().velocity = followObGameObject.GetComponent<Rigidbody>().velocity;
			}
			if (followObGameObject.GetComponent<DuplicateArrows>().collision)
			{
				gameObject.GetComponent<Rigidbody>().velocity = gameObject.transform.rotation * Vector3.forward * 30;
			}

		} 
		else 
		{
			gameObject.GetComponent<Rigidbody>().velocity = gameObject.transform.rotation * Vector3.forward * 30;
		}
	}

	private void OnDisable()
	{
		gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
		gameObject.GetComponent<Rigidbody>().angularVelocity = new Vector3(0,0,0);
	}

	void Destroy()
	{
		gameObject.SetActive(false);
	}


	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject?.GetComponent<CastleController>())
		{
			Invoke(nameof(Destroy),0.5f);
			return;
		}
		GameObject dummygo;
		if (pool.GetPooledObject() != null)
		{
			dummygo = pool.GetPooledObject();
		}
		else
		{
			Debug.Log("DUMMY RAIN ARROW ERROR, NULL!");
			return;
		}

		collision = true;
		gameObject.SetActive(false);
		if (other.gameObject?.GetComponent<MinionController>())
		{
			other.gameObject?.GetComponent<MinionController>().Minion.TakeDamage(damage);
		}
		else
		{
			dummygo.transform.position = gameObject.transform.position;
			dummygo.transform.rotation = gameObject.transform.rotation;
		}

		dummygo.SetActive(true);
		Invoke(nameof(Destroy),10.0f);
	}
}
