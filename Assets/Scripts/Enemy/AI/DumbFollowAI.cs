using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbFollowAI : MonoBehaviour
{
    public float speed = 25;
    public GameObject[] players;
    private Rigidbody2D rb;
    //Plans to have the dumb AI be alerted to distance and then begin chase. Also maybe a spell that surpresses all enemies alert state.
    public float AlertDistance = 0;
    public bool alerted = false;
    //Damage from contact
    public Transform target;
    public float DistToTarget;
    public float DistToP1;
    public float DistToP2;


    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        StartCoroutine(TargetCheck());
        rb = GetComponent<Rigidbody2D>(); //defining the rigidbody2D for stuff and things
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
        if ((alerted) && (target != null))
        {
            float step = speed * Time.deltaTime; //just a basic speed setup, plans to make the skeletons shamble with a Sine wave and some math, but my brain hurts.        
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        }
        if (target == null)
        {
            CheckTargets();
        }
    }
    void Alert()
    {
        alerted = true;
    }

    IEnumerator TargetCheck()
    {
        Debug.Log("Enemy: " + name + "Began TargetCheck Coroutine");
        yield return new WaitForSeconds(10);
            CheckTargets();
        StartCoroutine(TargetCheck());

    }

    void CheckTargets()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        DistToP1 = Vector2.Distance(players[0].transform.position, transform.position);
        if (players.Length > 1)
        {
            DistToP2 = Vector2.Distance(players[1].transform.position, transform.position);

        }
        if ((DistToP1 < DistToP2) && players.Length > 1)
        {
            target = players[0].transform;
        }
        if ((DistToP2 < DistToP1) && players.Length > 1)
        {
            target = players[1].transform;
        }
        if (players.Length == 1)
        {
            target = players[0].transform;
        }
        Debug.Log("Enemy: " + name + "Checked Targets");


    }
}
