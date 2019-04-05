using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayActivation : MonoBehaviour
{
	public float delay = 1f;
	public GameObject[] delayedObjects;

	private float _delay = 0f;
	private bool done = false;

	void Start()
	{
		_delay = delay;
		done = false;
	}

	void OnEnable()
    {
		_delay = delay;
		done = false;
    }
	
    void Update()
    {
		if (!done)
		{
			if (_delay <= 0f)
			{
				foreach (var o in delayedObjects)
					o.SetActive(true);
				done = true;
			}
			else
				_delay -= Time.deltaTime;
		}
    }
}
