using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownPlayerMovement : MonoBehaviour {

    public float BoostSpeed = 75;
    public float speed;
    private Rigidbody2D _rb;
    public Rigidbody2D Playerrb
    {
        get { return _rb; }
    }

    // Use this for initialization
    void Start()
    {
        //Gotta designate the RB now
        _rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        //This is my boost script, Reminder: Make it on a cooldown later. Pseudocode: Tick bool IsBoosting, tick float BoostJuice down while IsBoosting is on, then when it's <=0 put BoostCD on... etc. Or use a coroutine idk.
        if (Input.GetButton("Jump"))
        {
            speed = BoostSpeed;
        }

        if (Input.GetButtonUp("Jump"))
        {
            speed = 25;
        }
    }

    private void FixedUpdate()
    {
        //transform.rotation = new Quaternion(0, 0, 0, 0);

        var Vinput = Input.GetAxis("Vertical");
        _rb.AddForce(gameObject.transform.up * speed * Vinput);

        var Hinput = Input.GetAxis("Horizontal");
        _rb.AddForce(gameObject.transform.right * (speed) * Hinput);

        SpriteRenderer SR = this.GetComponent<SpriteRenderer>();
        if (Hinput > 0)
        {
            SR.flipX = false;
        }
        if (Hinput < 0)
        {
            SR.flipX = true;
        }
        


    }
}
