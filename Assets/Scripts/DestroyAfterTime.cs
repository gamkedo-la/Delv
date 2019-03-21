using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
	[SerializeField] private float delay = 1f;

    void Update()
    {
		delay -= Time.deltaTime;
		if (delay < 0f) Destroy(gameObject);
    }
}
