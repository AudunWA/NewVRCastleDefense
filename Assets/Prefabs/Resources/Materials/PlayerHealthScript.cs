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
		UpdateHealthBar();
	}

    public void UpdateHealthBar()
    {
        currentHealth = Valve.VR.InteractionSystem.Player.instance.TargetablePlayer.Health;
        renderer.material.SetFloat("_Cutoff",1 - currentHealth/maxHealth);
    }
}
