using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCheck : MonoBehaviour
{
    public List<GameObject> EnemyList = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        EnemyList.RemoveAll(GameObject => GameObject == null);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            var enemy = collision.gameObject;
            EnemyList.Add(enemy);
        }
    }
}
