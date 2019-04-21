using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MegaWormBrain : MonoBehaviour
{
	public enum PersonalityTraits
	{
		Aggressive,
		Sharp,
		Playful
	}

	[Header("Essentials")]
	public PersonalityTraits personality = PersonalityTraits.Aggressive;
	public float hitPoints = 1000f;
	public float initialDistance = 15f;
	public float speed = 5f;
	public float startDelay = 0.25f;
	public float attackDelay = 0.25f;

	[Header("Aggressive Settings")]
	public float attackSpeed = 10f;
	public float variationAngle = 50f;
	public float maxAttackDelay = 0.25f;

	[Header("Playful Settings")]
	public float playfulMaxGap = 10f;
	public float playfulMinGap = 5f;
	public float playfulGapTransition = 0.25f;
	public float playfulAngleChange = 0.3f; //in rad
	private float playfulGap = 10f;
	private bool playfulGapBounceback = false;

	[Header("Sharp Settings")]
	public float targetFollow = 2f;
	public float lineShowDelay = 0.5f;
	public float lineMinSize = 0.1f;
	public float lineWidthFactor = 0.1f;
	private float followTimer = 0f;
	private float lineTimer = 0f;

	[Header("Player Selection")]
	public int playerToAttackIndex = 0;
	public bool alternate = false;
	public bool main = false;

	[Header("Additionals")]
	public GameObject DamagedParticle = null;
	public Animator HeadAni;
	public GameObject HealthBar;

	static public float MaxHP = 100000;
	static public float HP = 100000;

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
	private ReturnToVillageEvent returnToVillageEvent;

	private LineRenderer sharpLineRenderer;

	static private int damageCounter = 0;
	static public bool countDamage = true;




	// Start is called before the first frame update
	void OnEnable()
	{
		BossEvent = HeadAni.gameObject.GetComponent<Boss2Event>();
		returnToVillageEvent = HeadAni.gameObject.GetComponent<ReturnToVillageEvent>();
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
					HeadAni.SetBool("Dead", true);
					Destroy(HealthBar);
					BossEvent.SetupDeathCutscene();
					MegaWormDarkMode.TurnOff();
					returnToVillageEvent.Trigger();
				}
			}
			else if (HP <= MaxHP / 3f)
			{
				if (HeadAni.GetInteger("AttackPhase") != 2)
				{
					HeadAni.SetInteger("AttackPhase", 2);
					MegaWormDarkMode.TurnOn();
				}
			}
			else if (HP <= MaxHP / 1.5f)
			{
				if (HeadAni.GetInteger("AttackPhase") != 1)
				{
					HeadAni.SetInteger("AttackPhase", 1);
					MegaWormDarkMode.TurnOn();
				}
			}
		}
	}

	public void InitHP()
	{
		HP = MaxHP = hitPoints;
		ResetDamageCounter();
	}

	public void BrainStart()
	{
		head = transform.GetChild(0).gameObject;

		//To fix the colors that are removed by animator cutscene...
		head.GetComponent<SpriteRenderer>().color = Color.white;

		transform.GetChild(1).gameObject.SetActive(false); //turning off TiledBody

		bodyTrail = head.GetComponent<TrailRenderer>();
		dugSlideTrail = head.transform.GetChild(0).gameObject.GetComponent<TrailRenderer>();

		bodyTrail.enabled = dugSlideTrail.enabled = true;

		players = GameObject.FindGameObjectsWithTag("Player");

		SwitchPlayerTarget();

		playfulGap = playfulMaxGap;

		if (personality == PersonalityTraits.Sharp)
			sharpLineRenderer = GetComponent<LineRenderer>();
	}

	public void BrainUpdate()
	{
		if (players.Length > 1)
		{
			if (players[playerToAttackIndex].GetComponent<PlayerController>().Health <= 0f)
				playerToAttackIndex = (playerToAttackIndex == 0 ? 1 : 0);
		}

		if (startDelay <= 0f)
		{
			if (personality == PersonalityTraits.Aggressive)
			{
				if (start)
				{
					attackAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
					transform.position = target + new Vector2(initialDistance * Mathf.Cos(attackAngle), initialDistance * Mathf.Sin(attackAngle));
					head.transform.position = transform.position;

					bodyTrail.Clear();
					dugSlideTrail.Clear();

					start = false;

					attackTimer = Random.Range(attackDelay, maxAttackDelay);
				}
				else if (Vector2.Distance(transform.position, target) < minimumDistance)
				{
					if (!goneFar)
					{
						attackAngle += (Random.Range(-variationAngle, variationAngle) * Mathf.Deg2Rad);

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
						target = Vector2.zero;

						attackTimer = Random.Range(attackDelay, maxAttackDelay);
					}
				}
				else if (attackTimer <= 0f)
				{
					if (target == Vector2.zero)
					{
						target = players[playerToAttackIndex].transform.position;
						SwitchPlayerTarget();
					}
					
					transform.position = Vector2.MoveTowards(transform.position, target, (goneFar ? speed : attackSpeed) * Time.deltaTime);
				}

				attackTimer -= Time.deltaTime;
			}
			else if (personality == PersonalityTraits.Sharp)
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
					followTimer = targetFollow;
					lineTimer = lineShowDelay;
				}
				else if (attackTimer <= 0f)
				{
					if (followTimer > 0f)
					{
						target = players[playerToAttackIndex].transform.position;

						//float angle = Vector2.Angle(target, transform.position) * Mathf.Deg2Rad;

						Vector2 angleVector = target - new Vector2(transform.position.x, transform.position.y);
						float angle = Mathf.Atan2(angleVector.y, angleVector.x);

						sharpLineRenderer.startWidth = sharpLineRenderer.endWidth = lineMinSize;

						sharpLineRenderer.SetPosition(0, transform.position);

						sharpLineRenderer.SetPosition(1, transform.position + new Vector3((initialDistance * 2f) * Mathf.Cos(angle), (initialDistance * 2f) * Mathf.Sin(angle), 0f));
						
						GetComponent<BoxCollider2D>().enabled = false;

						followTimer -= Time.deltaTime;

						if (followTimer <= 0f)
						{
							GetComponent<BoxCollider2D>().enabled = true;

							target = new Vector2(
								transform.position.x + ((initialDistance * 2f) * Mathf.Cos(angle)),
								transform.position.y + ((initialDistance * 2f) * Mathf.Sin(angle))
								);
						}
					}
					else if (lineTimer > 0f)
					{
						sharpLineRenderer.startWidth += lineWidthFactor * Time.deltaTime;
						sharpLineRenderer.endWidth += lineWidthFactor * Time.deltaTime;

						lineTimer -= Time.deltaTime;
					}
					else if (Vector2.Distance(transform.position, target) < minimumDistance)
					{
						sharpLineRenderer.startWidth = sharpLineRenderer.endWidth = 0f;

						target = Vector3.zero;
					}
					else
					{
						if (sharpLineRenderer.startWidth > 0f)
						{
							sharpLineRenderer.startWidth -= (lineWidthFactor * 2f) * Time.deltaTime;
							sharpLineRenderer.endWidth -= (lineWidthFactor * 2f) * Time.deltaTime;
						}

						transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
					}
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
					if (playfulGap >= playfulMaxGap)
					{
						playfulGapBounceback = false;

						attackTimer = 1f; //any positive value will work
						//playful worm attacks once before going to shrink the player worm gap
					}
				}
				else
				{
					playfulGap -= playfulGapTransition * Time.deltaTime;
					if (playfulGap <= playfulMinGap) playfulGapBounceback = true;
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
		damageCounter++;
		BossEvent.CameraShake();
	}

	static public int GetDamageCounter() { return damageCounter; }
	static public void ResetDamageCounter() { damageCounter = 0; }

	public void SwitchPlayerTarget()
	{
		if (players.Length <= 1)
		{
			playerToAttackIndex = 0;
			alternate = false;
		}
		else if (alternate)
		{
			playerToAttackIndex = (playerToAttackIndex == 0 ? 1 : 0);
		}
	}
}
