using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPLS : MonoBehaviour
{
    public float speed;
    public Transform player;
    public GameObject playerGO;
    private Rigidbody2D rb;
    //private float distance = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if ((player) == null)
        {
            playerGO = GameObject.FindGameObjectWithTag("Player");
            player = playerGO.transform;
        }
    }

    void FixedUpdate()
    {   
        float z = Mathf.Atan2((player.transform.position.y - transform.position.y), (player.transform.position.x - transform.position.x)) * Mathf.Rad2Deg - 90;

        transform.eulerAngles = new Vector3(0, 0, z);

        
        rb.AddForce(gameObject.transform.up * speed);
    }
}
