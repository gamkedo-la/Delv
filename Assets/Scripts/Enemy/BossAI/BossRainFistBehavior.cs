using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRainFistBehavior : StateMachineBehaviour
{
	public float delay = 5.5f;

	public GameObject fistFall;
	public float targetMissFactor = 5f;
	public float fistFallDelay = 1f;
	public float fistFallAmount = 3f;

	private BossEvent bossEvent;

	private float timer = 0f;

	private float fistFallTimer = 0f;

	private GameObject[] players;

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		players = GameObject.FindGameObjectsWithTag("Player");

		bossEvent = animator.gameObject.GetComponent<BossEvent>();
		Destroy(bossEvent.fists[0]);
		Destroy(bossEvent.fists[1]);

		timer = delay;
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (fistFallTimer <= 0f)
		{
			for (int i = 0; i < fistFallAmount; i++)
			{
				if (players.Length > 0)
				{
					Vector3 target = players[0].transform.position
						+ new Vector3(Random.Range(-targetMissFactor, targetMissFactor),
						Random.Range(-targetMissFactor, targetMissFactor),
						0f);

					Instantiate(fistFall, target, Quaternion.Euler(0f, 0f, 0f));
				}
				if (players.Length > 1)
				{
					Vector3 target = players[1].transform.position
						+ new Vector3(Random.Range(-targetMissFactor, targetMissFactor),
						Random.Range(-targetMissFactor, targetMissFactor),
						0f);

					Instantiate(fistFall, target, Quaternion.Euler(0f, 0f, 0f));
				}
			}

			fistFallTimer = fistFallDelay;
		}
		else
		{
			fistFallTimer -= Time.deltaTime;
		}

		if (timer <= 0f)
		{
			animator.SetTrigger("VulnerableSwitch");

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
