using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaWorm_AttackWhileOnGroundBehaviour : StateMachineBehaviour
{
	public bool redActive = false;

	public int switchAfterHits = 5;

	private Boss2Event bossEvent;

	private GameObject redHead;
	private GameObject blueHead;
	private GameObject greenHead;

	private SpriteRenderer redHeadRenderer;
	private SpriteRenderer blueHeadRenderer;
	private SpriteRenderer greenHeadRenderer;

	private BoxCollider2D redCollider;
	private BoxCollider2D blueCollider;
	private BoxCollider2D greenCollider;

	static private bool done = false;

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
		bossEvent = animator.gameObject.GetComponent<Boss2Event>();

		redHead = bossEvent.brains[0].gameObject.transform.GetChild(0).gameObject;
		blueHead = bossEvent.brains[1].gameObject.transform.GetChild(0).gameObject;
		greenHead = bossEvent.brains[2].gameObject.transform.GetChild(0).gameObject;

		redHeadRenderer = redHead.GetComponent<SpriteRenderer>();
		blueHeadRenderer = blueHead.GetComponent<SpriteRenderer>();
		greenHeadRenderer = greenHead.GetComponent<SpriteRenderer>();

		redCollider = bossEvent.brains[0].gameObject.GetComponent<BoxCollider2D>();
		blueCollider = bossEvent.brains[1].gameObject.GetComponent<BoxCollider2D>();
		greenCollider = bossEvent.brains[2].gameObject.GetComponent<BoxCollider2D>();

		MegaWormBrain.ResetDamageCounter();

		if (!done)
		{
			bossEvent.brains[0].InitHP();
			done = true;
		}

		if (redActive)
		{
			bossEvent.brains[0].BrainStart();

			redHeadRenderer.enabled = true;
			redCollider.enabled = true;
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
			blueHeadRenderer.enabled = true;
			blueCollider.enabled = true;
			greenHeadRenderer.enabled = true;
			greenCollider.enabled = true;
		}
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
		MegaWormBrain.countDamage = true;

		if (redActive)
		{
			bossEvent.brains[0].BrainUpdate();
            
            redHead.SetActive(true);
		}
		else
		{
			bossEvent.brains[1].BrainUpdate();
			bossEvent.brains[2].BrainUpdate();

			greenHead.SetActive(true);
			blueHead.SetActive(true);
		}

		if (MegaWormBrain.GetDamageCounter() >= switchAfterHits)
		{
            
			animator.SetTrigger("Damage");
			MegaWormBrain.ResetDamageCounter();
			MegaWormBrain.countDamage = false;
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
