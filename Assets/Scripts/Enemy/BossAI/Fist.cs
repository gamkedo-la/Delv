using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fist : MonoBehaviour
{
    public Transform StartPos;
    public BigBones BigBones;
    public Transform target;
    public Vector3 targetOffset;
    public Animator Ani;
    public GameObject ActualFist;
    public bool Active;

    public Collider2D DMGcollider;

    public bool isChasing;
    public bool isSlammed;
    public bool isLifting;
    public float speed;
    public float timeBetweenSlams;

    public GameObject[] players;

	private Vector3 targetSet = Vector3.zero;
	private int playerIndex = -1;

    // Start is called before the first frame update


    public void Activate( int pi = -1 )
    {
		playerIndex = pi;
        CheckTargets(playerIndex);
        StartChase();
        StartCoroutine(SlamLoop());
        Active = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Active)
        {
            if (isChasing)
            {
                float step = speed * Time.deltaTime; //just a basic speed setup, plans to make the skeletons shamble with a Sine wave and some math, but my brain hurts.        
                transform.position = Vector3.MoveTowards(transform.position, target.position + targetOffset, step);
            }
            else
            {
                float step = speed * 2f * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, targetSet, step);
			}

            if (target == null)
            {
                CheckTargets(playerIndex);
            }
            if (ActualFist == null)
            {
                Destroy(gameObject);
            }

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
        CheckTargets(playerIndex);
	}

    IEnumerator SlamLoop()
    {
		while (true)
		{
			yield return new WaitForSeconds(timeBetweenSlams);
			StartCoroutine(Slam());
		}
        //StartCoroutine(SlamLoop());
    }

    void StartChase()
    {
        isChasing = true;
    }

    void StopChase()
    {
        isChasing = false;
		targetSet = target.position;
	}
	
    void CheckTargets( int playerIndex = -1 )
    {
		players = GameObject.FindGameObjectsWithTag("Player");
		if (players.Length > 1)
		{
			if (playerIndex <= -1)
				target = players[Random.Range(0, 2)].transform;
			else
				target = players[playerIndex].transform;
		}
		else
		{
			target = players[0].transform;
		}
    }
}
