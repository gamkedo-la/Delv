using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFeelAlive : MonoBehaviour
{
	public float maxWalkTime = 1f;
	public float maxWaitTime = 10f;
	public float speed = 1f;
	public bool flipWhenLeft = false;
	public bool toLeft = true;

	private bool once = false;
	private float timer = 0f;
	private float waitTimer = 0f;

	private SpriteRenderer spRend;

    void Start()
    {
		spRend = GetComponent<SpriteRenderer>();
    }
	
    void Update()
    {
		if (timer <= 0f)
		{
			if (!once)
			{
				toLeft = !toLeft;

				if (flipWhenLeft)
					spRend.flipX = toLeft;
				else
					spRend.flipX = !toLeft;

				once = true;
			}

			if (waitTimer <= 0f)
			{
				timer = Random.Range(0f, maxWalkTime);
				waitTimer = Random.Range(0f, maxWaitTime);
				once = false;
			}
			else
			{
				waitTimer -= Time.deltaTime;
			}
		}
		else if (toLeft)
		{
			Vector3 pos = transform.position;
			pos.x -= speed * Time.deltaTime;
			transform.position = pos;
		}
		else
		{
			Vector3 pos = transform.position;
			pos.x += speed * Time.deltaTime;
			transform.position = pos;
		}

		timer -= Time.deltaTime;
    }
}
