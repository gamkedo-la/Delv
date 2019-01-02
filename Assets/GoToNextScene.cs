using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToNextScene : MonoBehaviour {

    private GameManagerScript GM;
    private bool SequenceFired = false;

	// Use this for initialization
	void Start ()
    {
        GameObject GMGO = GameObject.Find("GameManager");
        GM = GMGO.GetComponent<GameManagerScript>();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && SequenceFired == false)
        {
            SequenceFired = true;
            GM.SendMessage("GoToNextScene");
            Debug.Log("Sent GoToNextScene message");
        }
        
    }
}
