using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPotionScript : MonoBehaviour
{
    public float Amount = 25;
    private PlayerController pc;
    [FMODUnity.EventRef]
    public string potionPickupSound;

    private void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("Potion hit by " + col);
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("Potion touched player" + col);
            pc = col.gameObject.GetComponent<PlayerController>();
            col.gameObject.SendMessage("DamageEnergy", -(Amount));
            FMODUnity.RuntimeManager.PlayOneShot(potionPickupSound, transform.position);
            Die();
        }
    }
    void Die()
    {
        Destroy(gameObject);
    }

}
