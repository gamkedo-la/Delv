using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateOnCollision : MonoBehaviour
{
    public GameObject activateTarget;
    public bool fired;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if ((col.gameObject.tag == "player") && (fired == false))
        {
            activateTarget.SendMessage("Activate");
            fired = true;
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if ((col.gameObject.tag == "player") && (fired == false))
        {
            activateTarget.SendMessage("Deactivate");
        }
    }
}
