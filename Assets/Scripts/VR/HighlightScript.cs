using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightScript : MonoBehaviour {

    public bool highlight = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (highlight)
        {
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, 0.03f, 0);
            gameObject.transform.rotation *= Quaternion.Euler(0, 1f, 0);
        } else
        {
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, 0, 0);
            gameObject.transform.rotation = Quaternion.identity;
        }
	}
}
