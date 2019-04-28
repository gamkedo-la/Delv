using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reviver : MonoBehaviour
{
    [SerializeField] private GameObject hintCanvas;

	//[SerializeField] private PlayerController player;
    public PlayerController p2;

	private CircleCollider2D coll;

    [SerializeField] private PlayerController revivablePlayerController;
	
    void Start()
    {
		coll = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            RevivePlayer();
        }

        if (Input.GetKeyUp(KeyCode.K))
        {
            if (p2 == null)
            {
                return;
            }
            p2.SendMessage("DamageHealth", 110f);
        }
    }

    PlayerController otherPlayer;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "Player") {
            return;
        }

        otherPlayer = other.gameObject.GetComponent<PlayerController>();

        if (otherPlayer == null) {
            Debug.Log("otherPlayer is null");
            return;
        }

        if (!otherPlayer.isDead)
        {
            //Debug.Log("otherPlayer is not dead");
            // fixme this always logs even though one player is clearly dead and the isDead is true. 
            return;
        }

        revivablePlayerController = otherPlayer;
        // @todo replace with SendMessage or something to a hint-script?
        hintCanvas.SetActive(true);

        Text t = hintCanvas.GetComponentInChildren<Text>();
        t.text = "Press R to revive the other player";
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") {
			//revivablePlayerController = null;
            hintCanvas.SetActive(false);
        }
    }

    private void PlayerDied()
    {
        Debug.Log("PlayerDied function called");
		coll.enabled = true;
    }

    private void RevivePlayer()
    {
        Debug.Log("Attempting to revive");

        if (revivablePlayerController == null) {
            return;
        }

        hintCanvas.SetActive(false);
		coll.enabled = false;
        revivablePlayerController.enabled = true;
        revivablePlayerController.SendMessage("Revive");
        revivablePlayerController = null;
    }
}
