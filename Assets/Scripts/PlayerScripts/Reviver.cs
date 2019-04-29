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
    public PlayerController PC;
    private GameManagerScript GM;

    void Start()
    {
		coll = GetComponent<CircleCollider2D>();
        PC = GetComponentInParent<PlayerController>();
        GM = GameManagerScript.instance;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) || Input.GetButtonDown("Pickup" + PC.ControllerSlot)) {
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
            hintCanvas.SetActive(false);
            //Debug.Log("otherPlayer is not dead");
            // fixme this always logs even though one player is clearly dead and the isDead is true. 
            return;
        }

        revivablePlayerController = otherPlayer;
        // @todo replace with SendMessage or something to a hint-script?

        if (!PC.isBot)
        {
            hintCanvas.SetActive(true);
        }

        Text t = hintCanvas.GetComponentInChildren<Text>();

        if (PC.PlayerIndex == 1)
        {
            if (GM.p1ControllerKind == ControllerKind.Keyboard)
            {
                t.text = "Press R to revive the other player";
            }

            if (GM.p1ControllerKind == ControllerKind.DualShock)
            {
                t.text = "Press triangle to revive the other player";
            }

            //if (GM.p2ControllerKind == ControllerKind.XInput)
            //{
            //    //t.text = "Press ??? to revive the other player";
            // fixme I assume "Y" but have no way to test
            //}
        }

        if (PC.PlayerIndex == 2 && !PC.isBot)
        {
            if (GM.p2ControllerKind == ControllerKind.Keyboard)
            {
                t.text = "Press R to revive the other player";
            }

            if (GM.p2ControllerKind == ControllerKind.DualShock)
            {
                t.text = "Press triangle to revive the other player";
            }

            //if (GM.p2ControllerKind == ControllerKind.XInput)
            //{
            //    //t.text = "Press ??? to revive the other player";
            // fixme I assume "Y" but have no way to test
            //}
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") {
			//revivablePlayerController = null;
            hintCanvas.SetActive(false);
        }
    }

    //  private void PlayerDied()
    //  {
    //      Debug.Log("PlayerDied function called");
	//	    coll.enabled = true;
    //}

    private void RevivePlayer()
    {
        Debug.Log("Attempting to revive");

        if (revivablePlayerController == null) {
            return;
        }

        hintCanvas.SetActive(false);
		//coll.enabled = false;
        revivablePlayerController.enabled = true;
        revivablePlayerController.SendMessage("Revive");
        revivablePlayerController = null;
    }
}
