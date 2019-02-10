using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reviver : MonoBehaviour
{
    [SerializeField] private CircleCollider2D reviveCollider;

    private PlayerController player;

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
        if (other.gameObject.tag == "Player") {
            revivablePlayer = other.gameObject;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") {
            revivablePlayer = null;
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

        reviveCollider.enabled = false;
        revivablePlayer.SendMessage("Revive");
    }
}
