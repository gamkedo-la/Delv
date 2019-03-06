using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fist : MonoBehaviour
{
    public Transform StartPos;
    public BigBones BigBones;
    public Transform target;
    public Animator Ani;

    public Collider2D DMGcollider;

    public bool isChasing;
    public bool isSlammed;
    public bool isLifting;
    public float speed;

    // Start is called before the first frame update
    void OnEnable()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isChasing)
        {
            float step = speed * Time.deltaTime; //just a basic speed setup, plans to make the skeletons shamble with a Sine wave and some math, but my brain hurts.        
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);

        }
    }

    IEnumerator Slam()
    {
        DMGcollider.enabled = true;
        yield return new WaitForSeconds(.5f);
        DMGcollider.enabled = false;
        isLifting = true;
    }

    void StartChase()
    {
        isChasing = true;
    }

    void StopChase()
    {
        isChasing = false;
    }
}
