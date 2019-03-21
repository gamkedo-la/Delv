using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateInstanceLoop : MonoBehaviour
{
	public GameObject prefab;
	public Vector2 randomOffset = Vector2.zero;
	public float delay = 0.1f;

	private float timer = 0f;

    void Start()
    {
		timer = delay;
    }
	
    void Update()
    {
		if (timer <= 0f)
		{
			Instantiate(prefab, transform.position +
				new Vector3(Random.Range(-randomOffset.x, randomOffset.x),
				Random.Range(-randomOffset.y, randomOffset.y),
				0f),
				Quaternion.Euler(0f,0f,0f));
			timer = delay;
		}
		else
		{
			timer -= Time.deltaTime;
		}
    }
}
