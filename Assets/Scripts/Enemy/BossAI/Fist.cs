using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fist : MonoBehaviour
{
    public Transform StartPos;
    public BigBones BigBones;
    public Transform target;
    public Animator Ani;
    public GameObject ActualFist;

    public Collider2D DMGcollider;

    public bool isChasing;
    public bool isSlammed;
    public bool isLifting;
    public float speed;
    public float timeBetweenSlams;

    public GameObject[] players;

    // Start is called before the first frame update
    void OnEnable()
    {

        CheckTargets();
        ActivateFist();

    }

    void ActivateFist()
    {
        StartChase();
        CheckTargets();
        StartCoroutine(SlamLoop());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isChasing)
        {
            float step = speed * Time.deltaTime; //just a basic speed setup, plans to make the skeletons shamble with a Sine wave and some math, but my brain hurts.        
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);

        }
        if (target == null)
        {
            CheckTargets();

        }
        if (ActualFist == null)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator Slam()
    {
        Debug.Log("Slamming now...");
        StopChase();
        Ani.SetTrigger("StartSlam");
        yield return new WaitForSeconds(.5f);
        Debug.Log("Lifting now...");
        Ani.SetTrigger("StartLift");
        CheckTargets();

    }

    IEnumerator SlamLoop()
    {
        StartCoroutine(Slam());
        yield return new WaitForSeconds(timeBetweenSlams);
        StartCoroutine(SlamLoop());
    }

    void StartChase()
    {
        isChasing = true;
    }

    void StopChase()
    {
        isChasing = false;
    }

    void RandTarget()
    {
        target = players[Random.Range(0, 1)].transform;
    }
    void CheckTargets()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length > 1)
        {
            RandTarget();

        }
        if (players.Length == 1)
        {
            target = players[0].transform;
        }
    }
}
