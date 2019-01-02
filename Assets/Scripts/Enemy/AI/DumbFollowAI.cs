using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbFollowAI : MonoBehaviour
{
    public float speed = 25;
    public Transform player;
    private GameObject playerGO;
    private Rigidbody2D rb;
    //Plans to have the dumb AI be alerted to distance and then begin chase. Also maybe a spell that surpresses all enemies alert state.
    public float AlertDistance = 0;
    public bool alerted = false;
    //Damage from contact
    public float rawDMG = 1;
    public string DMGtype = "normal";


    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); //defining the rigidbody2D for stuff and things
        if ((player) == null) //if they do not have a player as target at their start, target one.
        {
            playerGO = GameObject.FindGameObjectWithTag("Player");
            player = playerGO.transform;
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player") //This is the damage event
        {
            Debug.Log("Player has been struck by projectile");
            coll.SendMessage("DamageHealth", rawDMG);
        }
    }
    void FixedUpdate()
    {
        // This was my distance raycast. It's broken, for some reason it won't get any distance. I have no idea why.
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, player.position, Mathf.Infinity, 8);
        //Debug.DrawRay(transform.position, player.position);
        //if (hit.distance <= AlertDistance)
        //{
        //    alerted = true;
        //}
        if (alerted)
        {
            float step = speed * Time.deltaTime; //just a basic speed setup, plans to make the skeletons shamble with a Sine wave and some math, but my brain hurts.        
            transform.position = Vector3.MoveTowards(transform.position, player.position, step);
        }
    }
    void Alert()
    {
        alerted = true;
    }
}
