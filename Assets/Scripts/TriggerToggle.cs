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
        ProjectileManager = GameObject.Find("ProjectileManager");
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
        //int count = 0;
		if (collision.gameObject.tag == colliderTag && !collision.gameObject.GetComponent<PlayerController>().isBot)
		{
			for (int i = 0; i < disable.Length; i++)
				disable[i].SetActive(false);

			for (int i = 0; i < enable.Length; i++)
				enable[i].SetActive(true);

            foreach (Transform child in ParticleManager.transform)
            {
                Destroy(child.gameObject);
                //count++;
            }

            //Debug.Log(count + " particles destroyed");
            //count = 0;

            foreach (Transform child in ProjectileManager.transform)
            {
                Destroy(child.gameObject);
                //count++;
            }
            //Debug.Log(count + " projectiles destroyed");
        }
    } // end of OnTriggerEnter2D
} // end of TriggerToggle Script
