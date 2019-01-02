using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherControls : MonoBehaviour {
    /// <Preface>
    /// Long and short of this dirty thing is that I don't know what I'm doing. Excuse the mess.
    /// </Preface>
    /// Variables - self explanitory I think.
 
    public float TetherDistance = 5f;
    public float TetherCD = 0;
    public float TetherTimer = 0.25f;//All are just timers and distance modifiers for max dist and CD on the tether.
    RaycastHit2D hit; //The raycast hit biz
    DistanceJoint2D TetherJoint; //The joint needed
    Vector3 targetPos;  //Where I clickyo
    public LayerMask mask;  //Just a layer mask because it's in the syntax for the raycast2D
    public Vector3 LrTarget; //Target in which I want the LR to hold onto.
    private LineRenderer LR;  //The LR
    private Vector3 Base; //Pointless variable to reference the player's current location easily


	// Use this for initialization
	void Start ()
    {
        TetherJoint = GetComponent<DistanceJoint2D>();
        TetherJoint.enabled = false;
        LR = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        Base = transform.position; // Setting our location constantly

        LR.SetPosition(0, Base); //Setting the LR to put Node 0 on player position
        LR.SetPosition(1, LrTarget); //Setting the LR to put Node 1 on the target position of the Hit object (if none it doesn't matter because the LR is disabled right now anyway)


        if (Input.GetButtonDown("Fire2")) //input
        {



            targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //Get mouse position and make targetPos that. then cut out the Z.
            targetPos.z = 0;

            hit = Physics2D.Raycast(transform.position, targetPos - transform.position, TetherDistance, mask); //defining the raycast, Raycast(V3, V3, Float, Mask) Then doing the same with a debug line.
            Debug.DrawRay (transform.position, (targetPos - transform.position), Color.blue, 10f);

            if (hit.collider != null && hit.collider.gameObject.GetComponent<Rigidbody2D>() != null) //If the hit object has a rigidbody continue.
            {
                TetherJoint.enabled = true; //turning on the Tether and the LR
                TetherJoint.connectedBody = hit.collider.gameObject.GetComponent<Rigidbody2D>(); //Making the Tetherjoint actually grab the connected body that was hit. Then setting the distance.
                TetherJoint.distance = Vector2.Distance(transform.position, hit.point);
                LR.enabled = true;
                LrTarget = hit.collider.gameObject.transform.position; //First defining of the LR target, has to be done here first so that when this call is exited the one below can hit it without error.
            }

        }
        if (Input.GetButtonUp("Fire2")) //Just disabling all the things when I let go.
        {
            TetherJoint.enabled = false;
            LR.enabled = false;
        }

        LrTarget = hit.collider.gameObject.transform.position; //A followup that constantly occurs, only works after being defined once in the actual hit event. Kinda hacky.
    }
}
