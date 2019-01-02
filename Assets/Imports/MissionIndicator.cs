using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionIndicator : MonoBehaviour
{
    public Transform Target;
    public GameObject Player;
    public bool TickedOn = false;
    private SpriteRenderer SR;
    //private float Alpha;
    //private SpriteRenderer SR;
    //public Color Color;

	// Use this for initialization
	void Start ()
    {
        //SR = gameObject.GetComponent<SpriteRenderer>();
        // Alpha = 255;
        // SR.color = ;
        SR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
            SR.enabled = !SR.enabled;
        transform.position = Player.transform.position;
        //transform.LookAt(Target);
        transform.up = Target.position - transform.position;
    }
}
