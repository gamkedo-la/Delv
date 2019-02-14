using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public Animator Animator;
    public EnemyManager spawner;
    public GameObject SpawnerParticle;
    public Transform Eye;

    // Start is called before the first frame update
    void Start()
    {
        Animator = GetComponent<Animator>();
        Eye = this.transform;
    }

    void SpawnEnemy()
    {

        GameObject EnemyInstance = Instantiate(EnemyPrefab, Eye.position, Eye.rotation);
        EnemyHealth EH = EnemyInstance.GetComponent<EnemyHealth>();
        EH.spawner = spawner;
        StartCoroutine(Die());
        
    }

    void SpawnParticle()
    {
        if (GameManagerScript.instance.ParticleIntensity >= 2)
        {
            Instantiate(SpawnerParticle, transform.position, transform.rotation);
        }
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(.1f);
        Destroy(gameObject);
    }


}
