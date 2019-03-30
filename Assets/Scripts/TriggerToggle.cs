using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerToggle : MonoBehaviour
{
	public GameObject[] disable;
	public GameObject[] enable;
	
	private void OnTriggerEnter2D(Collider2D collision)
	{
		for (int i = 0; i < disable.Length; i++)
			disable[i].SetActive(false);

		for (int i = 0; i < enable.Length; i++)
			enable[i].SetActive(true);
	}
}
