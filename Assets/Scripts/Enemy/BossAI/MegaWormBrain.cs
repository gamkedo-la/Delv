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
	static public float MaxHP = 1000f;
	static public float HP = 1000f;
	public float initialDistance = 15f;
	public float speed = 5f;
	public float startDelay = 0.25f;
	public float attackDelay = 0.25f;

	[Space]
	public float playfulMaxGap = 10f;
	public float playfulMinGap = 5f;
	private float playfulGap = 10f;
	private bool playfulGapBounceback = false;
	public float playfulGapTransition = 0.25f;
	public float playfulAngleChange = 0.3f; //in rad

	[Space]
	public int playerToAttackIndex = 0;
	public bool alternate = false;
	public bool main = false;

	[Space]
	public GameObject DamagedParticle = null;
	public Animator HeadAni;
	public GameObject HealthBar;

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

	private Boss2Event BossEvent;




	// Start is called before the first frame update
	void OnEnable()
	{
		BossEvent = HeadAni.gameObject.GetComponent<Boss2Event>();
	}

	private void Update()
	{
		if (main)
		{
			if (HealthBar)
			{
				Vector2 barScale = HealthBar.transform.GetChild(0).localScale;
				barScale.x = (HP / MaxHP) * 5f; //5f is max scale of health bar
				HealthBar.transform.GetChild(0).localScale = barScale;
			}

			if (HP <= 0f)
			{
				if (!HeadAni.GetBool("Dead"))
				{
					BossEvent.stateChanged = true;

					HeadAni.SetBool("Dead", true);
					Destroy(HealthBar);
					BossEvent.SetupDeathCutscene();
				}
			}
			/*
			 * change phase as hp goes down...
			 * 
			else if (HP <= MaxHP / 3f)
			{
				if (!HeadAni.GetBool("RainFist"))
				{
					BossEvent.stateChanged = true;

					HeadAni.SetBool("RainFist", true);
				}
			}
			*/
		}
	}

	public void BrainStart()
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

		playfulGap = playfulMaxGap;
    }

	public void BrainUpdate()
	{
		if (startDelay <= 0f)
		{
			if (personality == PersonalityTraits.Aggressive)
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

						if (alternateAngles)
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
			else if (personality == PersonalityTraits.Defensive) //same code as Aggressive for now
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

						if (alternateAngles)
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
			else if (personality == PersonalityTraits.Playful)
			{
				if (target == Vector2.zero)
				{
					target = players[playerToAttackIndex].transform.position;

					if (attackTimer <= 0f)
					{
						attackAngle += playfulAngleChange;
						target += new Vector2(playfulGap * Mathf.Cos(attackAngle), playfulGap * Mathf.Sin(attackAngle));
					}
					else
						attackTimer = 0f;

					if (start)
					{
						attackAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
						transform.position = target + new Vector2(initialDistance * Mathf.Cos(attackAngle), initialDistance * Mathf.Sin(attackAngle));
						head.transform.position = transform.position;

						bodyTrail.Clear();
						dugSlideTrail.Clear();

						start = false;
					}
				}
				else if (Vector2.Distance(transform.position, target) < minimumDistance)
				{
					target = Vector2.zero;
				}
				else
				{
					transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
				}

				if (playfulGapBounceback)
				{
					playfulGap += playfulGapTransition * Time.deltaTime;
					if (playfulGap >= playfulMaxGap) playfulGapBounceback = false;
				}
				else
				{
					playfulGap -= playfulGapTransition * Time.deltaTime;
					if (playfulGap <= playfulMinGap)
					{
						playfulGapBounceback = true;

						attackTimer = 1f; //any positive value will work
						//playful worm attacks once before going to shrink the player worm gap
					}
				}
			}
		}

		startDelay -= Time.deltaTime;
	}

	void DamageHealth(float DMG)
	{
		FloatingTextController.CreateFloatingText(DMG.ToString(), transform, DMG);
		Debug.Log(gameObject + " health damaged by " + DMG);
		HP -= DMG;
		Debug.Log(gameObject + " health is now " + HP);
		//Play Hurt Sound Here
		if (DMG > 10)
		{
			if ((GameManagerScript.instance.ParticleIntensity > 0))
			{
				if(DamagedParticle != null)
					Instantiate(DamagedParticle, transform.position, transform.rotation);
			}
		}

	}
}
