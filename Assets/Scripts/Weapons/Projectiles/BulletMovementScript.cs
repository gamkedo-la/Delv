using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovementScript : MonoBehaviour {

    public float maxSpeed = 5f;
    public float lifetime = 10f;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 pos = transform.position;
        Vector3 velocity = new Vector3(0, maxSpeed * Time.deltaTime, 0);
        pos += (transform.rotation * velocity);
        transform.position = pos;

        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
            Die();
	}

    void Die()
    {
        Destroy(gameObject);
    }
}
