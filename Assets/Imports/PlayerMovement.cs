using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private Input Boost;
    public float BoostSpeed = 75;
    public float speed;
    private Rigidbody2D _rb;
    public Rigidbody2D Playerrb
    { get { return _rb; }
    }

	// Use this for initialization
	void Start ()
    {
        //Gotta designate the RB now
        _rb = GetComponent<Rigidbody2D>();

	}
	
	// Update is called once per frame
	void Update ()
    {
        //This is my boost script, Reminder: Make it on a cooldown later. Pseudocode: Tick bool IsBoosting, tick float BoostJuice down while IsBoosting is on, then when it's <=0 put BoostCD on... etc. 
        if (Input.GetButton("Jump"))
            speed = BoostSpeed;
        if (Input.GetButtonUp("Jump")) 
            speed = 25;		
	}

    private void FixedUpdate ()
    {
        //Var works with any variable I think? So I dont have to specify. This is getting my mouse position and rotating the forward vector to mouse position.
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //The autocode thing in VS did most of this after I tried to turn it normalyl
        Quaternion rot = Quaternion.LookRotation(transform.position - mousePosition, Vector3.forward);
        //This is the declaration of rot and then a lock so I don't make my ship look up and down the wrong axiseseseseseseesssZ.
        transform.rotation = rot;
        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);
        _rb.angularVelocity = 0;

        //Go Forward/Backward if I use up or down.
        var Vinput = Input.GetAxis("Vertical");

        //rigidbody2D.Addforce(gameObject.transform.up * speed * input); This was my reference which kinda worked
        _rb.AddForce(gameObject.transform.up * speed * Vinput);

        //StrafeNShit and if I split speed in half it lets me do the thing
        var Hinput = Input.GetAxis("Horizontal");
        _rb.AddForce(gameObject.transform.right * (speed/2) * Hinput);
            
    }
}
