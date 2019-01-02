using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomActivator : MonoBehaviour {

    public GameObject player;
    public bool active;
    public DumbFollowAI[] AI;
    public GameObject[] Enemies;
    public bool IsSecret;
    public bool HasEnemies;
    [SerializeField]
    private GameObject RoomVeil;


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (((col.gameObject.tag == "Player")&& HasEnemies)&& !active)
        {
            ActivateRoomEnemies();
        }
        if (((col.gameObject.tag == "Player")&& IsSecret)&& !active)
        {
            ActivateRoomSecret();
        }
    }

    // Use this for initialization
    void Start ()
    {
        if (Enemies[0] != null)
        {
            AI[0] = Enemies[0].GetComponent<DumbFollowAI>();
            AI[1] = Enemies[1].GetComponent<DumbFollowAI>();
            AI[2] = Enemies[2].GetComponent<DumbFollowAI>();
        }

    }

    // Update is called once per frame
    void Update ()
    {
		
	}

    //I actually couldn't get the Array to work. I wanted to do a Foreach (enemies != null) then send an alert message to them all. I also wanted to get a collider for the room to populate this array of enemies, but that failed too.
    // I also plan to lock the player into the room when entering it so they must resolve the room first. I also plan for puzzle rooms and secret rooms.
    void ActivateRoomEnemies()
    {
        Debug.Log("Room " + this.gameObject + " Has been activated");
        active = true;
        if (Enemies[0] != null)
        {
            AI[0].SendMessage("Alert");
            AI[1].SendMessage("Alert");
            AI[2].SendMessage("Alert");
        }
    }
    void ActivateRoomSecret()
    {
        Debug.Log("SecretRoom " + this.gameObject + " Has been activated");
        active = true;
        RoomVeil.SetActive(false);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       
    }
}
