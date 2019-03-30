using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRocksHazard : MonoBehaviour
{
	[Range(2, 10)]
	[SerializeField] int minTimeout = 2;
	[Range(10, 20)]
	[SerializeField] int maxTimeout = 15;
	[SerializeField] float radius = 2f;
	[SerializeField] GameObject fallingRockPrefab;

	void Start()
	{
		StartCoroutine(spawnRocks());
	}

	IEnumerator spawnRocks()
	{
		while (true)
		{
			yield return new WaitForSeconds(Random.Range(minTimeout, maxTimeout));

			Vector3 position = Random.insideUnitCircle * radius;
			Instantiate(fallingRockPrefab, position, Quaternion.Euler(0f, 0f, 0f));
		}
	}

	void OnDrawGizmosSelected()
	{
		UnityEditor.Handles.color = Color.green;
		UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, radius);
	}
}
