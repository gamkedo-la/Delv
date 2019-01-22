using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovementScript : MonoBehaviour {

    public float Speed = 5f;
    public float lifetime = 10f;
    public bool up;
    public bool right;
    private Rigidbody2D rb;

	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (up)
        {
            rb.AddForce(transform.up * Speed);
        }

        if (right)
        {
            rb.AddForce(-transform.right * Speed);
        }

        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
            Die();
	}

    void Die()
    {
        Destroy(gameObject);
    }
}
