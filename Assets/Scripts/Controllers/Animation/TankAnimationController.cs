using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TankAnimationController : MonoBehaviour
{
    private MinionController controller;
    private Animator animator;

	// Use this for initialization
	void Start ()
	{
	    controller = GetComponentInParent<MinionController>();
        if(controller == null) throw new NullReferenceException("TankAnimationController need a parent with a MinionController.");
	    animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		animator.SetBool("IsWalking", controller.Moving);
	    animator.SetBool("StandAttack", controller.Minion.State == Minion.minionState.Fighting);
	    animator.SetBool("Death", controller.Minion.State == Minion.minionState.Dead);
	}
}
