using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aimer : MonoBehaviour {



    public GameObject Player;
    public bool TickedOn = true;
    private SpriteRenderer SR;
    [SerializeField]


    // Use this for initialization
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {
        //makes the indicator always follow the player, and hide if shift is pressed
        //transform.position = Player.transform.position;
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            TickedOn = !TickedOn;
            this.enabled = !this.enabled;
        }


    }

    private void FixedUpdate()
    {
        // Gets mouse position and turns the object to it.
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Quaternion rot = Quaternion.LookRotation(transform.position - mousePosition, Vector3.forward);
        transform.rotation = rot;
        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);
    }
}
