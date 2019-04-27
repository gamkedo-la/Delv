using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningStaff : AbstractWeapon
{
    public LineRenderer LR;
    private ContactFilter2D damagableLayersFilter;
    private int enemyLayer = 10;
    private int containerLayer = 18;
    private int enemyLayerMask;
    private int containerLayerMask;
    private int combinedLayerMask;
    private RaycastHit2D[] lightningHitResults;

    [FMODUnity.EventRef]
    public string lightningSound;

    private void Awake()
    {
        enemyLayerMask = 1 << enemyLayer;
        containerLayerMask = 1 << containerLayer;
        combinedLayerMask = enemyLayerMask | containerLayerMask;
        damagableLayersFilter.layerMask.value = combinedLayerMask;
        damagableLayersFilter.useLayerMask = true;
    }

    public override void Fire1()
    {
        lightningHitResults = new RaycastHit2D[1];
        Debug.Log("Shooting");
        GunCD = fireDelay;
        Vector3 offset = transform.rotation * bulletOffset;
        //Here is where the lightning is supposed to go.
        if (ShotParticleSystem != null)
        {
            ShotParticleSystem.Emit(20);
        }
        /*RaycastHit2D hit =*/ Physics2D.Raycast(gameObject.transform.position,transform.up, damagableLayersFilter, lightningHitResults, CastRange);
        Debug.DrawRay(transform.position, transform.up*5, Color.yellow,3f);
        if (lightningHitResults[0])
        {
            GameObject HitEnemy = lightningHitResults[0].transform.gameObject;
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
            if(lightningHitResults[0].collider.tag == "Breakable")
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/Objects/wood_box_break", transform.position);
            }
        }
        StopCD1();
        CDzeroed = false;

        if (!lightningHitResults[0])
        {
            //return;
        }

        FMODUnity.RuntimeManager.PlayOneShot(lightningSound, transform.position);
    }

    IEnumerator LineCD()
    {
        yield return new WaitForSeconds(.2f);
        LR.enabled = false;
    }
}
