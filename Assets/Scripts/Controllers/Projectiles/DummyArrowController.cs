using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyArrowController : MonoBehaviour
{

	// Use this for initialization
	void Start () {
	}

    private void OnEnable()
    {
        Invoke("Destroy", 30.0f);
    }

    private void Destroy()
    {
        gameObject.transform.parent = GameObject.Find("DummyArrowPool").transform;
        gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void unParent()
    {
        gameObject.transform.parent = GameObject.Find("DummyArrowPool").transform;
    }
}
