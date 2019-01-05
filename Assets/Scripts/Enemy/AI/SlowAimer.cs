using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowAimer : MonoBehaviour {


    public Transform Target;
    public float RotationSpeed = 5;
    private Rigidbody2D rb;
    public bool alert;


	// Use this for initialization
	void Start ()
    {
        Target = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
		
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
}
