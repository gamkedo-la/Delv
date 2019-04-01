using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerToggle : MonoBehaviour
{
	public string colliderTag = "Player";
	public GameObject[] disable;
	public GameObject[] enable;
	
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == colliderTag)
		{
			for (int i = 0; i < disable.Length; i++)
				disable[i].SetActive(false);

			for (int i = 0; i < enable.Length; i++)
				enable[i].SetActive(true);
		}
	}
}
