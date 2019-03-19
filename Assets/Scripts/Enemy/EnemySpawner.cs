using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public Animator Animator;
    public EnemyManager spawner;
    public Transform Eye;
    public GameObject Particle;

    // Start is called before the first frame update
    void Start()
    {
        Animator = GetComponent<Animator>();
        Eye = this.transform;

		if (spawner == null)
			spawner = FindObjectOfType<EnemyManager>();
    }

    void SpawnEnemy()
    {
        GameObject EnemyInstance = Instantiate(EnemyPrefab, Eye.position, Eye.rotation);
        EnemyHealth EH = EnemyInstance.GetComponent<EnemyHealth>();
        EH.spawner = spawner;
        Die();
    }
    void SpawnParticle()
    {
        if (GameManagerScript.instance.ParticleIntensity >= 2)
        {
            Instantiate(Particle, transform.position, transform.rotation);
        }
    }
    void Die()
    {
        new WaitForSeconds(1);
        Destroy(gameObject);
    }

}
