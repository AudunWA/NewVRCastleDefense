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
	    animator.SetBool("IsWalking", velocityMagnitude > 0 && controller.Minion.State != Minion.MinionState.Fighting);

	    if (controller.Minion.State == Minion.MinionState.Fighting)
	        velocityMagnitude = 1;

        switch (controller.Minion.SpawnType)
	    {
	        case SpawnType.Archer:
            case SpawnType.Fighter:
	            animator.SetFloat("SpeedMultiply", Math.Min(1f, velocityMagnitude / 8f));
	            animator.SetBool("IsDead", controller.Minion.State == Minion.MinionState.Dead);

                break;
	        case SpawnType.Tank:
	            animator.SetBool("Death", controller.Minion.State == Minion.MinionState.Dead);
	            animator.SetFloat("SpeedMultiply", Math.Min(3f, velocityMagnitude * 0.5f));

                break;
	        case SpawnType.Mage:
	            animator.SetFloat("SpeedMultiply", Math.Min(3f, velocityMagnitude * 0.5f));
                //if (controller.Minion.State == Minion.MinionState.Fighting)
                //    animator.SetTrigger("throw");
                if (controller.Minion.State == Minion.MinionState.Dead)
	                animator.SetTrigger("die");

	            break;
	        default:
	            throw new ArgumentOutOfRangeException();
	    }
	}

    //private static readonly Dictionary<SpawnType, float> attackAnimationMultipliers =
    //    new Dictionary<SpawnType, float>
    //    {
    //        {SpawnType.Tank, 3},
    //        {SpawnType.Archer, 1},
    //        {SpawnType.Fighter, 1},
    //        {SpawnType.Mage, 1}
    //    };

    void OnAttack(float attackCooldown)
    {
        float attacksPerSecond = 1 / attackCooldown;
        animator.SetFloat("AttackSpeed", Math.Max(1, attacksPerSecond) /** attackAnimationMultipliers[controller.Minion.SpawnType]*/);

        switch (controller.Minion.SpawnType)
        {
            case SpawnType.Tank:
            case SpawnType.Archer:
            case SpawnType.Fighter:
                animator.SetTrigger("Attack");
                break;
            case SpawnType.Mage:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
