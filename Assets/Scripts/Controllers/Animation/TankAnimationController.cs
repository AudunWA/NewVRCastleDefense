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
	    float velocityMagnitude = controller.Agent.velocity.sqrMagnitude;
	    animator.SetBool("IsWalking", velocityMagnitude > 0 && controller.Minion.State != Minion.minionState.Fighting);

	    if (controller.Minion.State == Minion.minionState.Fighting)
	        velocityMagnitude = 1;

        switch (controller.Minion.SpawnType)
	    {
	        case SpawnType.Archer:
            case SpawnType.Fighter:
	            animator.SetBool("IsAttacking", controller.Minion.State == Minion.minionState.Fighting);
	            animator.SetFloat("SpeedMultiply", velocityMagnitude / 8f);
	            animator.SetBool("IsDead", controller.Minion.State == Minion.minionState.Dead);

                break;
	        case SpawnType.Tank:
	            animator.SetBool("StandAttack", controller.Minion.State == Minion.minionState.Fighting);
	            animator.SetBool("Death", controller.Minion.State == Minion.minionState.Dead);
	            animator.SetFloat("SpeedMultiply", velocityMagnitude * 0.5f);

                break;
	        case SpawnType.Mage:
	            animator.SetFloat("SpeedMultiply", velocityMagnitude * 0.5f);
                //if (controller.Minion.State == Minion.minionState.Fighting)
                //    animator.SetTrigger("throw");
                if (controller.Minion.State == Minion.minionState.Dead)
	                animator.SetTrigger("die");

	            break;
	        default:
	            throw new ArgumentOutOfRangeException();
	    }
	}
}
