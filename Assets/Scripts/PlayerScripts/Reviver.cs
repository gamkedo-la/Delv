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
            p2.SendMessage("DamageHealth", 110f);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "Player") {
            return;
        }

        PlayerController otherPlayer = other.gameObject.GetComponent<PlayerController>();
        Debug.Log(otherPlayer , otherPlayer);
        if (otherPlayer == null) {
            Debug.Log("otherPlayer is null");
            return;
        }

        if (!otherPlayer.isDead)
        {
            Debug.Log("otherPlayer is not dead");
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
			revivablePlayerController = null;

            hintCanvas.SetActive(false);
        }
    }

    private void PlayerDied()
    {
		coll.enabled = true;
    }

    private void RevivePlayer()
    {
        if (revivablePlayerController == null) {
            return;
        }

        hintCanvas.SetActive(false);
		coll.enabled = false;
        revivablePlayerController.enabled = true;
        revivablePlayerController.Revive();
    }
}
