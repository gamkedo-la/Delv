using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSound : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerProjectile"))
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Objects/wood_box_break", transform.position);
        }
    }
}
