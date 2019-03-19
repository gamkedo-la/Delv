using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneStartPoint : MonoBehaviour {

    public GameObject[] Player;
    public bool HasPlayerBeenBrought = false;

	// Use this for initialization
	void Start ()
    {
        Player = new GameObject[2];
        Player = GameObject.FindGameObjectsWithTag("Player");
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!Player[0])
        {
            Player = GameObject.FindGameObjectsWithTag("Player");
        }
        if ((!HasPlayerBeenBrought) && (Player[0] != null))
        {
            foreach (GameObject pl in Player)
            {
                pl.SendMessage("GoToStart");
            }
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
