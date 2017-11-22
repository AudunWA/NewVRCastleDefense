using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongbowArrowScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        if (gameObject.GetComponent<Rigidbody>().velocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(gameObject.GetComponent<Rigidbody>().velocity * 1.15f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject != GameObject.Find("Longbow"))
        {
            if (collision.gameObject.tag.Contains("minion"))
            {
                collision.gameObject.GetComponent<MinionController>().Minion.TakeDamage(50);
            }
            Destroy(gameObject);
        }
    }
}
