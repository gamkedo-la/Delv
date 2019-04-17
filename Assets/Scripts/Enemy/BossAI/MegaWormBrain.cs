using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MegaWormBrain : MonoBehaviour
{
	public enum PersonalityTraits
	{
		Aggressive,
		Defensive,
		Playful
	}
	public PersonalityTraits personality = PersonalityTraits.Aggressive;
	
	[Space]
	public float initialDistance = 15f;
	public float speed = 5f;
	public float randomOffsetFactor = 1f;
	public float attackDelay = 0.25f;

	[Space]
	public int playerToAttackIndex = 0;
	public bool alternate = false;

	private float attackAngle = 0f;
	private float attackTimer = 0f;
	private float minimumDistance = 0.25f;

	private bool start = true;
	private bool goneFar = false;
	private bool alternateAngles = true;

	private GameObject head;
	private TrailRenderer bodyTrail;
	private TrailRenderer dugSlideTrail;

	private GameObject[] players;
	private Vector2 target = Vector2.zero;
	
    void Start()
    {
		head = transform.GetChild(0).gameObject;

		//To fix the colors that are removed by animator cutscene...
		head.GetComponent<SpriteRenderer>().color = Color.white;

		transform.GetChild(1).gameObject.SetActive(false); //turning off TiledBody

		bodyTrail = head.GetComponent<TrailRenderer>();
		dugSlideTrail = head.transform.GetChild(0).gameObject.GetComponent<TrailRenderer>();
		
		players = GameObject.FindGameObjectsWithTag("Player");

		if (players.Length <= 1)
		{
			playerToAttackIndex = 0;
			alternate = true;
		}
    }
	
    void Update()
    {
		if (target == Vector2.zero)
		{
			target = players[playerToAttackIndex].transform.position;

			if (start)
			{
				attackAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
				transform.position = target + new Vector2(initialDistance * Mathf.Cos(attackAngle), initialDistance * Mathf.Sin(attackAngle));
				head.transform.position = transform.position;

				bodyTrail.Clear();
				dugSlideTrail.Clear();

				start = false;
			}

			attackTimer = attackDelay;
		}
		else if (Vector2.Distance(transform.position, target) < minimumDistance)
		{
			if (!goneFar)
			{
				attackAngle += (Random.Range(-90f, 90f) * Mathf.Deg2Rad);

				if(alternateAngles)
					target -= new Vector2(initialDistance * Mathf.Cos(attackAngle), initialDistance * Mathf.Sin(attackAngle));
				else
					target += new Vector2(initialDistance * Mathf.Cos(attackAngle), initialDistance * Mathf.Sin(attackAngle));
				
				alternateAngles = !alternateAngles;

				goneFar = true;
			}
			else
			{
				goneFar = false;
				target = Vector3.zero;
			}
		}
		else if (attackTimer <= 0f)
		{
			transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
		}

		attackTimer -= Time.deltaTime;
    }
}
