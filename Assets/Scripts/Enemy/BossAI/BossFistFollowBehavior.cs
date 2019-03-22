using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFistFollowBehavior : StateMachineBehaviour
{
	public int playerIndex = -1;

	public float delay = 5.5f;

	private BossEvent bossEvent;

	private float timer = 0f;

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		bossEvent = animator.gameObject.GetComponent<BossEvent>();
		bossEvent.EnableFists(playerIndex);
		timer = delay;
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (timer <= 0f)
		{
			bossEvent.DisableFists();
			animator.SetBool("VulnerableSwitch", true);
			animator.SetBool("Player1", playerIndex != 0);

			timer = delay;
		}
		else
		{
			timer -= Time.deltaTime;
		}
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//	
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove()
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//    // Implement code that processes and affects root motion
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK()
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//    // Implement code that sets up animation IK (inverse kinematics)
	//}
}
