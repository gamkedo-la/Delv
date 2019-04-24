using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reviver : MonoBehaviour
{
    [SerializeField] private CircleCollider2D reviveCollider;

    [SerializeField] private GameObject hintCanvas;

	[SerializeField] private PlayerController player;

    private GameObject revivablePlayer;

    // @todo DEBUG! remove when completed reviving
    public PlayerController p2;

    void Start()
    {
        player = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            RevivePlayer();
        }

        // @todo DEBUG! remove when completed reviving
        if (Input.GetKeyDown(KeyCode.K)) {
            p2.SendMessage("DamageHealth", 110f);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "Player") {
            return;
        }

        PlayerController otherPlayer = other.gameObject.GetComponent<PlayerController>();
        if (otherPlayer == null || !otherPlayer.isDead) {
            return;
        }

        revivablePlayer = other.gameObject;

        // @todo replace with SendMessage or something to a hint-script?
        hintCanvas.SetActive(true);

        Text t = hintCanvas.GetComponentInChildren<Text>();
        t.text = "Press R to revive the other player";
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") {
            revivablePlayer = null;

            hintCanvas.SetActive(false);
        }
    }

    private void PlayerDied()
    {
        reviveCollider.enabled = true;
    }

    private void RevivePlayer()
    {
        if (revivablePlayer == null) {
            return;
        }

        hintCanvas.SetActive(false);
        reviveCollider.enabled = false;
        revivablePlayer.SendMessage("Revive");
    }
}
