using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormTypeAI : MonoBehaviour
{


    public Transform target;
    public float RotationSpeed = 100;
    public float Speed;
    public bool charging;
    private Rigidbody2D rb;
    public bool alert;
    public GameObject[] players;
    [Space]
    public float AttackRange;
    public float ChargeMult;
    public float DistToTarget;
    public float DistToP1;
    public float DistToP2;




    // Use this for initialization
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        StartCoroutine(TargetCheck());
        rb = GetComponent<Rigidbody2D>();
        rb.rotation = Random.Range(0,360);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if ((target != null) && (alert) && (!charging))
        {
            Vector2 direction = (Vector2)target.position - rb.position;
            direction.Normalize();
            float RotateAmount = Vector3.Cross(direction, transform.right).z;
            rb.angularVelocity = RotateAmount * RotationSpeed;
            rb.AddForce(-transform.right*Speed);
            RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, -transform.right, AttackRange, 1 << LayerMask.NameToLayer("Player"));
            Debug.DrawRay(transform.position, -transform.right * AttackRange, Color.red, .1f);
            if ((hit) && (!charging))
            {
                Debug.Log("Raycast hit " + hit.transform.name);
                StopCoroutine("StartCharge");
                StartCoroutine("StartCharge");
            }



        }
        if (target == null)
        {
            CheckTargets();

        }
        if ((target != null) && (alert) && (charging))
        {
            Vector2 direction = (Vector2)target.position - rb.position;
            direction.Normalize();
            float RotateAmount = Vector3.Cross(direction, transform.right).z;
            rb.angularVelocity = RotateAmount * RotationSpeed;
            rb.AddForce(-transform.right * Speed * ChargeMult);

        }

    }

    IEnumerator StartCharge()
    {
        rb.velocity = new Vector2(0, 0);
        charging = true;
        yield return new WaitForSeconds(1);
        charging = false;

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
    IEnumerator TargetCheck()
    {
        Debug.Log("Enemy: " + name + "Began TargetCheck Coroutine");
        yield return new WaitForSeconds(10);
        CheckTargets();
        StartCoroutine(TargetCheck());

    }




}
