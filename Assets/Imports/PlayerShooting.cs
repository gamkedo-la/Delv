using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour {

    public GameObject Bullet;
    public float GunCD = 0.0f;
    public float fireDelay = 0.25f;
    public Vector3 bulletOffset = new Vector3(0, 0.5f, 0);

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        GunCD -= Time.deltaTime;

		if(Input.GetButton("Fire1") && GunCD <= 0)
        {
            
            Debug.Log ("Shooting");
            GunCD = fireDelay;
            Vector3 offset = transform.rotation * bulletOffset;
            Instantiate(Bullet, transform.position + offset, transform.rotation);
        }
	}
}
