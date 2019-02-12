using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToNextScene : MonoBehaviour
{

    private bool SequenceFired = false;
    public string NextScene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && SequenceFired == false && NextScene != null)
        {
            SequenceFired = true;
            GameManagerScript.instance.SendMessage("GoToScene", NextScene);
            Debug.Log("Sent GoToScene (" + NextScene + ") message");
        }

    }
}
