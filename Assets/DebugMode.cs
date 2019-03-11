using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// First git commit using Git Bash -- will remove
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
    }

}
