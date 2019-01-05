using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToNextScene : MonoBehaviour {

    public GameManagerScript GM;
    private bool SequenceFired = false;
    public string NextScene;

	// Use this for initialization
	void Start ()
    {
        GameObject GMGO = GameObject.Find("GameManager");
        GM = GMGO.GetComponent<GameManagerScript>();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && SequenceFired == false && NextScene != null)
        {
            SequenceFired = true;
            GM.SendMessage("GoToScene", NextScene);
            Debug.Log("Sent GoToScene (" + NextScene + ") message");
        }
        
    }
}
