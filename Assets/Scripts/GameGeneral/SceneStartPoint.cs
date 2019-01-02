using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneStartPoint : MonoBehaviour {

    public GameObject Player;
    public bool HasPlayerBeenBrought = false;


	// Use this for initialization
	void Start ()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!Player)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }
        if ((!HasPlayerBeenBrought) && (Player != null))
        {
            Player.SendMessage("GoToStart");
        }
	}

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            HasPlayerBeenBrought = true;
        }
    }
}
