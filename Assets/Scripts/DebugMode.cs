using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMode : MonoBehaviour
{
    public GameObject[] player;
    public GameObject[] enemy;

    
    // Start is called before the first frame update
    void OnEnable()
    {
        Debug.Log("Debug Mode Initialized");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            enemy = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemies in enemy)
            {
                enemies.SendMessage("DamageHealth", 1000);
            }
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            player = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject players in player)
            {
                players.SendMessage("DamageHealth", -1000);
            }
        }
    }

}
