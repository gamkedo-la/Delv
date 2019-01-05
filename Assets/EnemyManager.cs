using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public int EnemiesPerWave;
    public GameObject Enemy;
}

public class EnemyManager : MonoBehaviour
{
    public Wave[] Waves; // class to hold information per wave
    public Transform[] SpawnPoints;
    public float TimeBetweenEnemies = 2f;

    private int _totalEnemiesInCurrentWave;
    private int _enemiesInWaveLeft;
    private int _spawnedEnemies;

    private int _currentWave;
    private int _totalWaves;

    public GameObject Reward;
    public Transform RewardPoint;

    public bool LockType;
    public GameObject RoomLockGO;
    public Transform[] RoomLockPoints;
    public bool RoomActive = false;
    public GameObject[] Locks;

    void Start()
    {
        _currentWave = -1; // avoid off by 1
        _totalWaves = Waves.Length - 1; // adjust, because we're using 0 index


    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if ((col.tag == "Player") && !RoomActive)
        {
            RoomActive = true;
            if (LockType)
            {
                LockRoom();
            }
            StartNextWave();
        }
    }

    void StartNextWave()
    {
        _currentWave++;

        // win
        if (_currentWave > _totalWaves)
        {
            RoomComplete();
            return;
        }

        _totalEnemiesInCurrentWave = Waves[_currentWave].EnemiesPerWave;
        _enemiesInWaveLeft = 0;
        _spawnedEnemies = 0;

        StartCoroutine(SpawnEnemies());
    }

    // Coroutine to spawn all of our enemies
    IEnumerator SpawnEnemies()
    {
        GameObject enemy = Waves[_currentWave].Enemy;
        while (_spawnedEnemies < _totalEnemiesInCurrentWave)
        {
            _spawnedEnemies++;
            _enemiesInWaveLeft++;

            int spawnPointIndex = Random.Range(0, SpawnPoints.Length);

            // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
             GameObject EnemyInstance = Instantiate(enemy, SpawnPoints[spawnPointIndex].position, SpawnPoints[spawnPointIndex].rotation);
            EnemyHealth EH = EnemyInstance.GetComponent<EnemyHealth>();
            EH.spawner = this;
            yield return new WaitForSeconds(TimeBetweenEnemies);
        }
        yield return null;
    }

    // called by an enemy when they're defeated
    public void EnemyDefeated()
    {
        _enemiesInWaveLeft--;

        // We start the next wave once we have spawned and defeated them all
        if (_enemiesInWaveLeft == 0 && _spawnedEnemies == _totalEnemiesInCurrentWave)
        {
            StartNextWave();
        }
    }

    public void LockRoom()
    {
        if ((RoomLockPoints[3]) && (RoomLockGO))
        {
            Locks = new GameObject[4];
            Debug.Log("Array Length set at" + Locks.Length);
            GameObject Lock0 = Instantiate(RoomLockGO, RoomLockPoints[0].position, RoomLockPoints[0].rotation);
            GameObject Lock1 = Instantiate(RoomLockGO, RoomLockPoints[1].position, RoomLockPoints[1].rotation);
            GameObject Lock2 = Instantiate(RoomLockGO, RoomLockPoints[2].position, RoomLockPoints[2].rotation);
            GameObject Lock3 = Instantiate(RoomLockGO, RoomLockPoints[3].position, RoomLockPoints[3].rotation);
            Locks[0] = Lock0;
            Locks[1] = Lock1;
            Locks[2] = Lock2;
            Locks[3] = Lock3;
        }
    }
    public void UnlockRoom()
    {
        foreach (GameObject L in Locks)
        {
            Destroy(L);
        }
    }

    public void RoomComplete()
    {
        //Add a possible reward here, BOTH NEED TO BE SET BEFORE THIS WORKS.
        if ((Reward != null) && RewardPoint != null)
        {
            Instantiate(Reward, RewardPoint.position, RewardPoint.rotation);
        }
        if ((RoomLockPoints[0]) && (LockType))
        {
            UnlockRoom();
        }
        Destroy(gameObject);
    }
}