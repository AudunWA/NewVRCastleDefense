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
    void Start()
    {
        controller = GetComponentInParent<MinionController>();
        if (controller == null)
            throw new NullReferenceException("TankAnimationController need a parent with a MinionController.");
        animator = GetComponent<Animator>();
        if (controller.Minion.SpawnType == SpawnType.Tank)
            animator.SetFloat("WalkAttackFactor", 0.5f);
    }

    // Update is called once per frame
	void Update () {
	    animator.SetBool("IsWalking", controller.Agent.velocity.sqrMagnitude > 0 && controller.Minion.State != Minion.minionState.Fighting);
	    animator.SetFloat("SpeedMultiply", controller.Agent.velocity.sqrMagnitude * 0.5f);

        switch (controller.Minion.SpawnType)
	    {
	        case SpawnType.Fighter:
	            break;
	        case SpawnType.Tank:
	            animator.SetBool("StandAttack", controller.Minion.State == Minion.minionState.Fighting);
	            animator.SetBool("Death", controller.Minion.State == Minion.minionState.Dead);
	            break;
	        case SpawnType.Mage:
	            //if (controller.Minion.State == Minion.minionState.Fighting)
	            //    animator.SetTrigger("throw");
	            if (controller.Minion.State == Minion.minionState.Dead)
	                animator.SetTrigger("die");

	            break;
	        case SpawnType.Archer:
	            break;
	        default:
	            throw new ArgumentOutOfRangeException();
	    }
	}
}
