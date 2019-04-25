using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerToggle : MonoBehaviour
{
	public string colliderTag = "Player";
	public GameObject[] disable;
	public GameObject[] enable;

    private GameObject ProjectileManager;
    private GameObject ParticleManager;

    private void Start()
    {
        if (ProjectileManager == null)
        {
            Debug.Log("ProjectileManager is not present - spawning shots in heirarchy");
        }

        ParticleManager = GameObject.Find("ParticleManager");
        if (ParticleManager == null)
        {
            Debug.Log("ParticleManager is not present - spawning shots in heirarchy");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == colliderTag && !collision.gameObject.GetComponent<PlayerController>().isBot)
		{
			for (int i = 0; i < disable.Length; i++)
				disable[i].SetActive(false);

			for (int i = 0; i < enable.Length; i++)
				enable[i].SetActive(true);
		}

    }
}
