using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaWorm_AttackWhileOnGroundBehaviour : StateMachineBehaviour
{
	public bool redActive = false;

	private Boss2Event bossEvent;

	private SpriteRenderer redHeadRenderer;
	private SpriteRenderer blueHeadRenderer;
	private SpriteRenderer greenHeadRenderer;

	private BoxCollider2D redCollider;
	private BoxCollider2D blueCollider;
	private BoxCollider2D greenCollider;

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
		bossEvent = animator.gameObject.GetComponent<Boss2Event>();

		redHeadRenderer = bossEvent.brains[0].gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
		blueHeadRenderer = bossEvent.brains[1].gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
		greenHeadRenderer = bossEvent.brains[2].gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();

		redCollider = bossEvent.brains[0].gameObject.GetComponent<BoxCollider2D>();
		blueCollider = bossEvent.brains[1].gameObject.GetComponent<BoxCollider2D>();
		greenCollider = bossEvent.brains[2].gameObject.GetComponent<BoxCollider2D>();

		if (redActive)
		{
			bossEvent.brains[0].BrainStart();

			blueHeadRenderer.enabled = false;
			blueCollider.enabled = false;
			greenHeadRenderer.enabled = false;
			greenCollider.enabled = false;
		}
		else
		{
			bossEvent.brains[1].BrainStart();
			bossEvent.brains[2].BrainStart();

			redHeadRenderer.enabled = false;
			redCollider.enabled = false;
		}
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
		if (redActive)
		{
			bossEvent.brains[0].BrainUpdate();
		}
		else
		{
			bossEvent.brains[1].BrainUpdate();
			bossEvent.brains[2].BrainUpdate();
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
