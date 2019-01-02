using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningStaff : MonoBehaviour
{

    private float GunCD = 0.0f;
    public float fireDelay = 0.25f;
    public Vector3 bulletOffset = new Vector3(0, 0.3f, 0);
    public float EnergyCost1 = 10;
    public float EnergyCost2 = .1f;
    public float CastRange = 5;
    public float ShockDamage = 15;
    public LineRenderer LR;
    //Directors
    private ControllerAimer CA;
    private GameObject Player;
    private PlayerController PC;
    //Failsafe bools
    private bool CDzeroed;
    private bool PLDisconnected;
    private bool Grounded;


    // This is for the particles when the weapon fires (if it needs them)
    [SerializeField]
    private ParticleSystem ShotParticle;

    // Use this for initialization
    void Start()
    {
        if (this.transform.parent != null)
        {
            CA = this.GetComponentInParent<ControllerAimer>();
            Player = CA.PlayerGO;
            PC = Player.GetComponent<PlayerController>();
            LR = this.GetComponent<LineRenderer>();
            PlayerConnect();
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if ((GunCD > 0) && CA != null)
        {
            GunCD -= Time.deltaTime;
        }
        if (((GunCD <= 0) && CDzeroed == false) && CA != null)
        {
            ClearCD1();
            CDzeroed = true;
        }
        //Failsafe Disconnect
        if ((this.transform.parent == null) && PLDisconnected == false)
        {
            PlayerDisconnect();
        }
        //Make this float when grounded. TODO

    }


    public void Fire1()
    {
        Debug.Log("Shooting");
        GunCD = fireDelay;
        Vector3 offset = transform.rotation * bulletOffset;
//Here is where the lightning is supposed to go.
        if (ShotParticle != null)
        {
            ShotParticle.Emit(20);
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
    public void CancelWeapon1()
    {
        //Unused in this slot.
        //Debug.Log("Cancel Weapon 1 is not set on" + this.gameObject);
    }

    public void Fire2()
    {

    }

    public void CancelWeapon2()
    {

    }

    public void PlayerConnect()
    {
        if (this.transform.parent != null)
        {
            CA = this.GetComponentInParent<ControllerAimer>();
            Player = CA.PlayerGO;
            PC = Player.GetComponent<PlayerController>();
            ManaPing();
            Collider2D thiscol = GetComponent<Collider2D>();
            thiscol.enabled = false;
            PLDisconnected = false;
        }

    }
    public void PlayerDisconnect()
    {
        if (CA != null)
        {
            ClearCD1();
            ClearCD2();
            CA = null;
            Player = null;
            PC = null;
            Collider2D thiscol = GetComponent<Collider2D>();
            thiscol.enabled = true;

        }

        PLDisconnected = true;

    }


    //Mana costs pings
    void ManaPing()
    {
        PC.EnergyCost1 = EnergyCost1;
        PC.EnergyCost2 = EnergyCost2;
    }

    //Cooldown pings

    void ClearCD1()
    {
        PC.CD1 = true;
    }
    void ClearCD2()
    {
        PC.CD1 = true;
    }
    void StopCD1()
    {
        PC.CD1 = false;
    }
    void StopCD2()
    {
        PC.CD1 = false;
    }




    IEnumerator LineCD()
    {
        yield return new WaitForSeconds(.2f);
        LR.enabled = false;
    }
}
