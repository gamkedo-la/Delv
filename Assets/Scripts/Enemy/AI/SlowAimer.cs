using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowAimer : MonoBehaviour {


    public Transform Target;
    public float RotationSpeed = 5;
    private Rigidbody2D rb;
    public bool alert;
    public GameObject Projectile;
    public Vector3 offset;
    public Quaternion rotoffset = new Quaternion (0,0,90,0);
    [Space]
    public float TimeBetweenShots = 1;
    public float TimeBetweenBursts = 3;


    // Use this for initialization
    void Start ()
    {
        Target = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(FireSequence());
		
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if ((Target) && (alert))
        {
            Vector2 direction = (Vector2)Target.position - rb.position;
            direction.Normalize();
            float RotateAmount = Vector3.Cross(direction, transform.right).z;
            rb.angularVelocity = RotateAmount * RotationSpeed;
        }
	}

    private void Update()
    {
        if (!Target)
        {
            Target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }
    IEnumerator FireSequence()
    {
        Instantiate(Projectile, transform.position + offset, transform.rotation);
        yield return new WaitForSeconds(TimeBetweenShots);
        Instantiate(Projectile, transform.position + offset, transform.rotation);
        yield return new WaitForSeconds(TimeBetweenShots);
        Instantiate(Projectile, transform.position + offset, transform.rotation);
        yield return new WaitForSeconds(TimeBetweenBursts);
        StartCoroutine(FireSequence());

    }
}
