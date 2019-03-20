using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWhenActivated : MonoBehaviour
{
	public GameObject spawnObject;

	public GameObject[] deactivateOnSpawn;

	void Start()
	{

	}

	void OnEnable()
	{
		Instantiate(spawnObject, transform.position, Quaternion.Euler(0f, 0f, 0f));
	}

	void Update()
	{
		for (int i = 0; i < deactivateOnSpawn.Length; i++)
		{
			deactivateOnSpawn[i].SetActive(false);
		}
	}
}
