using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuplicateArrows : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Invoke("Duplicate", 3.0f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Duplicate()
    {
        Instantiate(gameObject, new Vector3(gameObject.transform.position.x + 1, gameObject.transform.position.y + 1, gameObject.transform.position.z + 1), gameObject.transform.rotation);
    }
}
