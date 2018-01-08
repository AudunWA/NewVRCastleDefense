using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthScript : MonoBehaviour
{

    private float maxHealth = 100.0f;
    private float currentHealth = 100.0f;

    private Renderer renderer;

    // Use this for initialization
    void Start ()
    {
        renderer = gameObject.GetComponent<Renderer>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateHealthBar(float health)
    {
        currentHealth = health;
        renderer.material.SetFloat("_Cutof",currentHealth/maxHealth);
    }
}
