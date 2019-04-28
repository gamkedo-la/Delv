using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnContact : MonoBehaviour
{
    public float rawDMG;
    public bool DamageDealt;
    public float TimeTilRedamage = 0;
    public float RedamageClock;


    void OnTriggerStay2D(Collider2D coll)
    {
        if ((coll.gameObject.tag == "Player") && (DamageDealt == false)) //This is the damage event
        {
            coll.SendMessage("DamageHealth", rawDMG);
            TimeTilRedamage = RedamageClock;
            DamageDealt = true;
            

        }
    }

    private void FixedUpdate()
    {
        if (TimeTilRedamage >= -1)
        {
            TimeTilRedamage -= Time.deltaTime;
        }
        if (TimeTilRedamage <= 0)
        {
            DamageDealt = false;
        }

    }

}
