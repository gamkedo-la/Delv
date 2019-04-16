using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonField : MonoBehaviour
{
	public float healthDepletionPerTick = 5f;
	public float tickDelay = 0.25f;
	private float tickTimer = 0f;
	
	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			if (tickTimer <= 0f)
			{
				PlayerController plCont = collision.gameObject.GetComponent<PlayerController>();
				plCont.DamageHealth(healthDepletionPerTick);
                FMODUnity.RuntimeManager.PlayOneShot("event:/Player/player_damaged_ooze");
				tickTimer = tickDelay;
			}
			else
			{
				tickTimer -= Time.deltaTime;
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			tickTimer = 0f;
		}
	}
}
