using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningStaff : AbstractWeapon
{
    public LineRenderer LR;

    public override void Fire1()
    {
        Debug.Log("Shooting");
        GunCD = fireDelay;
        Vector3 offset = transform.rotation * bulletOffset;
        //Here is where the lightning is supposed to go.
        if (ShotParticleSystem != null)
        {
            ShotParticleSystem.Emit(20);
        }
        RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position,transform.up, CastRange, 1<<10);
        Debug.DrawRay(transform.position, transform.up*5, Color.yellow,3f);
        if (hit)
        {
            GameObject HitEnemy = hit.transform.gameObject;
            HitEnemy.SendMessage("DamageHealth", ShockDamage);
            Debug.Log("Enemy " + HitEnemy + " Has been hit");
            StartCoroutine("LineCD");
            if (!LR)
            {
                LR = this.GetComponent<LineRenderer>();

            }
            LR.enabled = true;
            LR.SetPosition(1, HitEnemy.transform.position);
            LR.SetPosition(0, transform.position);
        }
        StopCD1();
        CDzeroed = false;

        if (!hit)
        {
            //return;
        }
    }

    IEnumerator LineCD()
    {
        yield return new WaitForSeconds(.2f);
        LR.enabled = false;
    }
}
