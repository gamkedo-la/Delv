using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBoy : MonoBehaviour
{
    public float speed;
    public Transform player;
    public GameObject playerGO;
    private Rigidbody2D rb;
    public GameObject Bullet;
    public float GunCD = 0f;
    public float fireDelay = 1f;
    private float LastCD = 3;
    public float GunReload = 3f;
    public Vector3 bulletOffset = new Vector3(0, 0.5f, 0);
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



        GunCD -= Time.deltaTime;
        
        if (GunReload > 0 && GunCD <= 0)
        {
            GunCD = fireDelay;
            GunReload -= 1;
            Vector3 offset = transform.rotation * bulletOffset;
            Instantiate(Bullet, transform.position + offset, transform.rotation);
        }
        //if (LastCD <= 0)
        //{

        //}
        else if (GunReload <= 0)
        {
            GunCD = 1f;
            GunReload = 3f;
            Debug.Log("Bang");
        }

    }
}
