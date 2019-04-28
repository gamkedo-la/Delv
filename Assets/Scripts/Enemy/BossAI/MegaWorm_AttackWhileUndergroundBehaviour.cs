using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaWorm_AttackWhileUndergroundBehaviour : StateMachineBehaviour
{
	public GameObject wormTrap;

	public float initialDistance = 25f;
	public float attackDelay = 10f;

	private GameObject[] players;
	private int playerToAttackIndex = 0;
	private float attackTimer = 0f;

    

    void Start()
    {
        
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
		players = GameObject.FindGameObjectsWithTag("Player");
	}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
		if (players.Length > 1)
		{
			if (players[playerToAttackIndex].GetComponent<PlayerController>().Health <= 0f)
				playerToAttackIndex = (playerToAttackIndex == 0 ? 1 : 0);
		}
		else
			playerToAttackIndex = 0;

		if (attackTimer <= 0f)
		{
            
            
            float angle = Random.Range(0f, 360f);
			Vector3 position = players[playerToAttackIndex].transform.position
				+ new Vector3(initialDistance * Mathf.Cos(angle * Mathf.Deg2Rad), initialDistance * Mathf.Sin(angle * Mathf.Deg2Rad), 0f);

			Instantiate(wormTrap, position, Quaternion.Euler(0f, 0f, angle));

			attackTimer = attackDelay;
			playerToAttackIndex = (playerToAttackIndex == 0 ? 1 : 0);
		}

		attackTimer -= Time.deltaTime;
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
